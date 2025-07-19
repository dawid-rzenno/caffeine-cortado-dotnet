using cortado.Models;
using cortado.Services;
using Dapper;

namespace cortado.Repositories;

public interface IUserRolesRepository : ICrudRepository<UserRole, UserRole>
{
}

public class UserRolesRepository(DapperContext context, ICurrentUserService currentUserService) : IUserRolesRepository
{
    public async Task<IEnumerable<UserRole>> GetAllAsync()
    {
        var query = "SELECT * FROM UserRoles";

        using var connection = context.CreateConnection();
        return await connection.QueryAsync<UserRole>(query);
    }

    public async Task<UserRole?> GetByIdAsync(int id)
    {
        var query = """
                        SELECT * FROM UserRoles 
                        WHERE Id = @Id
                    """;

        using var connection = context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<UserRole>(query, new { Id = id });
    }

    public async Task<UserRole> CreateAsync(UserRole userRole)
    {
        var createUserRoleQuery = """
                                  INSERT INTO UserRoles (Name, Timestamp, UserId) 
                                  OUTPUT INSERTED.*
                                  VALUES (@Name, @Timestamp, @UserId)
                              """;

        userRole.Timestamp = DateTime.UtcNow;
        userRole.UserId = (int)currentUserService.GetUserId()!;

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<UserRole>(createUserRoleQuery, userRole);
    }

    public async Task<UserRole> UpdateAsync(UserRole userRole)
    {
        var query = """
                        UPDATE UserRoles SET Name = @Name, Timestamp = @Timestamp, UserId = @UserId
                        OUTPUT INSERTED.*
                        WHERE Id = @Id
                    """;

        userRole.Timestamp = DateTime.UtcNow;
        userRole.UserId = (int)currentUserService.GetUserId()!;

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<UserRole>(query, userRole);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var query = """
                        DELETE FROM UserRoles 
                        WHERE Id = @Id
                    """;

        using var connection = context.CreateConnection();
        var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
        return affectedRows > 0;
    }
}