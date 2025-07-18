using cortado.Models;
using Dapper;

namespace cortado.Repositories;

public class UserRepository(DapperContext context) : ICrudRepository<User>
{
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        var query = "SELECT * FROM Users";

        using var connection = context.CreateConnection();
        return await connection.QueryAsync<User>(query);
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

    public async Task<int> CreateAsync(User user)
    {
        var query = """
                        INSERT INTO Users (Username, Password, Timestamp, UserId) 
                        VALUES (@Username, @Password, @Timestamp, @UserId); 
                        SELECT CAST(SCOPE_IDENTITY() as int)
                    """;

        user.Timestamp = DateTime.UtcNow;

        using var connection = context.CreateConnection();
        return await connection.QuerySingleAsync<int>(query, user);
    }

    public async Task<bool> UpdateAsync(User user)
    {
        var query = """
                        UPDATE Users SET Username = @Username, Password = @Password, Timestamp = @Timestamp, UserId = @UserId
                        WHERE Id = @Id
                    """;

        user.Timestamp = DateTime.UtcNow;

        using var connection = context.CreateConnection();
        var affectedRows = await connection.ExecuteAsync(query, user);
        return affectedRows > 0;
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