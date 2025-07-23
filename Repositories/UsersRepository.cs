using cortado.Models;
using cortado.Services;
using Dapper;

namespace cortado.Repositories;

public interface IUsersRepository : ICrudRepository<User, User>
{
    public Task<IEnumerable<User>> GetAllByTermAsync(string term);
    public Task<User?> GetByUsernameAsync(string username);
    public Task<User> UpdatePasswordAsync(User user);
}

public class UsersRepository(DapperContext context, PasswordService passwordService) : IUsersRepository
{
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        var query = "SELECT * FROM Users";

        using var connection = context.CreateConnection();
        return await connection.QueryAsync<User>(query);
    }

    public async Task<IEnumerable<User>> GetAllByTermAsync(string term)
    {
        var query = "SELECT * FROM Users WHERE Username LIKE @term";

        using var connection = context.CreateConnection();
        return await connection.QueryAsync<User>(query, new { term });
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        var query = """
                        SELECT * FROM Users 
                        WHERE Id = @Id
                    """;

        using var connection = context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<User>(query, new { Id = id });
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        var query = """
                        SELECT * FROM Users 
                        WHERE Username = @Username
                    """;

        using var connection = context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<User>(query, new { Username = username });
    }

    public async Task<User> CreateAsync(User user)
    {
        var createUserQuery = """
                                  INSERT INTO Users (Username, Password, RoleId, Timestamp, UserId) 
                                  OUTPUT INSERTED.*
                                  VALUES (@Username, @Password, @RoleId, @Timestamp, @UserId); 
                                  SELECT CAST(SCOPE_IDENTITY() as int)
                              """;

        user.Timestamp = DateTime.UtcNow;
        user.Password = passwordService.HashPassword(user.Password);

        using var connection = context.CreateConnection();

        var createdUserId = await connection.QuerySingleAsync<int>(createUserQuery, user);

        var updateUserIdQuery = """
                                    UPDATE Users SET UserId = @UserId
                                    OUTPUT INSERTED.*
                                    WHERE Id = @Id
                                """;

        return await connection.QuerySingleAsync<User>(updateUserIdQuery,
            new { Id = createdUserId, UserId = createdUserId });
    }

    public async Task<User> UpdateAsync(User user)
    {
        var query = """
                        UPDATE Users SET Username = @Username, RoleId = @RoleId, Timestamp = @Timestamp, UserId = @UserId
                        OUTPUT INSERTED.*
                        WHERE Id = @Id
                    """;

        user.Timestamp = DateTime.UtcNow;

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<User>(query, user);
    }

    public async Task<User> UpdatePasswordAsync(User user)
    {
        var query = """
                        UPDATE Users SET Password = @Password, Timestamp = @Timestamp, UserId = @UserId
                        OUTPUT INSERTED.*
                        WHERE Id = @Id
                    """;

        user.Timestamp = DateTime.UtcNow;
        user.Password = passwordService.HashPassword(user.Password);

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<User>(query, user);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var query = """
                        DELETE FROM Users 
                        WHERE Id = @Id
                    """;

        using var connection = context.CreateConnection();
        var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
        return affectedRows > 0;
    }
}