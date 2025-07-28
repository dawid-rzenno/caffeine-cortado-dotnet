using cortado.Models;
using cortado.Services;
using Dapper;

namespace cortado.Repositories;

public interface IMassUnitsRepository : ICrudRepository<MassUnit, MassUnit>
{
    public Task<IEnumerable<MassUnit>> GetAllByTermAsync(string term, bool globalSearch);
}

public class MassUnitsRepository(DapperContext context, ICurrentUserService currentUserService) : IMassUnitsRepository
{
    public async Task<IEnumerable<MassUnit>> GetAllAsync()
    {
        var query = "SELECT * FROM MassUnits WHERE UserId = @UserId";

        using var connection = context.CreateConnection();
        return await connection.QueryAsync<MassUnit>(query, new { UserId = currentUserService.GetUserId() });
    }

    public async Task<IEnumerable<MassUnit>> GetAllByTermAsync(string term, bool globalSearch)
    {
        var query = globalSearch
            ? "SELECT TOP 10 * FROM MassUnits WHERE Name LIKE @Term"
            : "SELECT TOP 10 * FROM MassUnits WHERE Name LIKE @Term AND UserId = @UserId";

        using var connection = context.CreateConnection();

        return await connection.QueryAsync<MassUnit>(query,
            new { Term = $"%{term}%", UserId = currentUserService.GetUserId() });
    }

    public async Task<MassUnit?> GetByIdAsync(int id)
    {
        var massUnitQuery =
            """
                SELECT * FROM MassUnits 
                WHERE Id = @Id AND UserId = @UserId
            """;

        using var connection = context.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<MassUnit>(massUnitQuery,
            new { Id = id, UserId = currentUserService.GetUserId() });
    }

    public async Task<MassUnit> CreateAsync(MassUnit massUnit)
    {
        var createMassUnitQuery =
            """
                INSERT INTO MassUnits (Name, Timestamp, UserId) 
                OUTPUT INSERTED.*
                VALUES (@Name, @Timestamp, @UserId)
            """;

        massUnit.Timestamp = DateTime.UtcNow;
        massUnit.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<MassUnit>(createMassUnitQuery, massUnit);
    }

    public async Task<MassUnit> UpdateAsync(MassUnit massUnit)
    {
        var query =
            """
                UPDATE MassUnits SET Name = @Name, Timestamp = @Timestamp, UserId = @UserId
                OUTPUT INSERTED.*
                WHERE Id = @Id AND UserId = @UserId
            """;

        massUnit.Timestamp = DateTime.UtcNow;
        massUnit.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<MassUnit>(query, massUnit);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var query =
            """
                DELETE FROM MassUnits 
                WHERE Id = @Id
            """;

        using var connection = context.CreateConnection();
        var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
        return affectedRows > 0;
    }
}