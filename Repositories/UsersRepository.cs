using System.Data;
using cortado.Models;
using Dapper;

namespace cortado.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetOneByIdAsync(int id);
}

public class UserRepository(DapperContext context) : IUserRepository
{
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        using IDbConnection db = context.CreateConnection();
        
        return await db.QueryAsync<User>("SELECT * FROM Users");
    }

    public async Task<User?> GetOneByIdAsync(int id)
    {
        using IDbConnection db = context.CreateConnection();
        
        return await db.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE Id = @Id", new { Id = id });
    }
}