using cortado.Models;
using cortado.Services;
using Dapper;

namespace cortado.Repositories;

public interface INutrientNamesRepository : ICrudRepository<NutrientName, NutrientName>
{
    public Task<IEnumerable<NutrientName>> GetAllByTermAsync(string term, bool globalSearch);
}

public class NutrientNamesRepository(DapperContext context, ICurrentUserService currentUserService)
    : INutrientNamesRepository
{
    public async Task<IEnumerable<NutrientName>> GetAllAsync()
    {
        var query = "SELECT * FROM NutrientNames WHERE UserId = @UserId";

        using var connection = context.CreateConnection();
        return await connection.QueryAsync<NutrientName>(query, new { UserId = currentUserService.GetUserId() });
    }

    public async Task<IEnumerable<NutrientName>> GetAllByTermAsync(string term, bool globalSearch)
    {
        var query = globalSearch
            ? "SELECT TOP 10 * FROM NutrientNames WHERE Name LIKE @Term"
            : "SELECT TOP 10 * FROM NutrientNames WHERE Name LIKE @Term AND UserId = @UserId";

        using var connection = context.CreateConnection();

        return await connection.QueryAsync<NutrientName>(
            query,
            new { Term = $"%{term}%", UserId = currentUserService.GetUserId() }
        );
    }

    public async Task<NutrientName?> GetByIdAsync(int id)
    {
        var query =
            """
                SELECT * FROM NutrientNames 
                WHERE Id = @Id AND UserId = @UserId
            """;

        using var connection = context.CreateConnection();


        return await connection.QueryFirstOrDefaultAsync<NutrientName>(
            query,
            new { Id = id, UserId = currentUserService.GetUserId() }
        );
    }

    public async Task<NutrientName> CreateAsync(NutrientName nutrientName)
    {
        var query =
            """
                INSERT INTO NutrientNames (Name, Timestamp, UserId) 
                OUTPUT INSERTED.*
                VALUES (@Name, @Timestamp, @UserId)
            """;

        nutrientName.Timestamp = DateTime.UtcNow;
        nutrientName.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<NutrientName>(query, nutrientName);
    }

    public Task<NutrientName> UpdateAsync(NutrientName nutrientName)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var query =
            """
                DELETE FROM NutrientNames 
                WHERE Id = @Id
            """;

        using var connection = context.CreateConnection();
        var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
        return affectedRows > 0;
    }
}