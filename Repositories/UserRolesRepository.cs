using System.Data;
using cortado.Models;
using cortado.Repositories.Interfaces;
using Dapper;

namespace cortado.Repositories;

public interface IUserRolesRepository : ICrudRepository<UserRole, UserRole>
{
}

public class UserRolesRepository(DapperContext context) : IUserRolesRepository
{
    public async Task<IEnumerable<UserRole>> GetAllAsync(
        string sort,
        string sortBy,
        int size,
        int page,
        string term,
        bool globalSearch = false
    )
    {
        using var connection = context.CreateConnection();
        return await connection.QueryAsync<UserRole>(
            "SELECT * FROM ufn_GetUserRoles(@Term, @Page, @Size, @SortBy, @Sort)",
            new
            {
                Size = size,
                Page = page,
                Sort = sort,
                SortBy = sortBy,
                Term = term,
            }
        );
    }

    public async Task<UserRole?> GetByIdAsync(int id)
    {
        using var connection = context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<UserRole>(
            "SELECT * FROM ufn_GetUserRole(@Id)",
            new { Id = id }
        );
    }

    public async Task<UserRole> CreateAsync(UserRole userRole)
    {
        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<UserRole>("usp_CreateUserRole", userRole,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<UserRole> UpdateAsync(UserRole userRole)
    {
        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<UserRole>("usp_UpdateUserRole", userRole,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = context.CreateConnection();
        var affectedRows = await connection.ExecuteAsync("usp_DeleteUserRole", new { Id = id },
            commandType: CommandType.StoredProcedure);
        return affectedRows > 0;
    }
}