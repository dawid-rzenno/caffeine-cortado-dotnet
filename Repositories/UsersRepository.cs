using System.Data;
using cortado.Models;
using cortado.Repositories.Interfaces;
using cortado.Services;
using Dapper;

namespace cortado.Repositories;

public interface IUsersRepository : ICrudRepository<User, User>
{
    public Task<IEnumerable<User>> GetAllAsync(
        string sort,
        string sortBy,
        int size,
        int page,
        string term
    );

    public Task<User?> GetByUsernameAsync(string username);
    public Task<User> UpdatePasswordAsync(User user);
}

public class UsersRepository(
    DapperContext context,
    PasswordService passwordService,
    ICurrentUserService currentUserService) : IUsersRepository
{
    public async Task<IEnumerable<User>> GetAllAsync(
        string sort,
        string sortBy,
        int size,
        int page,
        string term
    )
    {
        using var connection = context.CreateConnection();
        return await connection.QueryAsync<User>(
            "SELECT * FROM ufn_GetUsers(@Term, @Page, @Size, @SortBy, @Sort)",
            new
            {
                Size = size,
                Page = page,
                Sort = sort,
                SortBy = sortBy,
                Term = term
            }
        );
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        using var connection = context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<User>(
            "SELECT * FROM ufn_GetUserByUsername(@Username)",
            new { Username = username });
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        using var connection = context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<User>(
            "SELECT * FROM ufn_GetUserById(@Id)",
            new { Id = id });
    }

    public async Task<User> CreateAsync(User user)
    {
        user.Timestamp = DateTime.UtcNow;
        user.Password = passwordService.HashPassword(user.Password);

        using var connection = context.CreateConnection();

        int createdUserId =
            await connection.QuerySingleAsync<int>("usp_CreateUser", user, commandType: CommandType.StoredProcedure);

        return await connection.QuerySingleAsync<User>("usp_UpdateUserUserId",
            new { Id = createdUserId, UserId = createdUserId }, commandType: CommandType.StoredProcedure);
    }

    public async Task<User> UpdateAsync(User user)
    {
        user.UserId = currentUserService.GetUserId();
        user.Timestamp = DateTime.UtcNow;

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<User>("usp_UpdateUser", user,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<User> UpdatePasswordAsync(User user)
    {
        user.Timestamp = DateTime.UtcNow;
        user.Password = passwordService.HashPassword(user.Password);

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<User>("usp_UpdateUserPassword", user,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = context.CreateConnection();
        var affectedRows = await connection.ExecuteAsync("usp_DeleteUser", new { Id = id },
            commandType: CommandType.StoredProcedure);
        return affectedRows > 0;
    }
}