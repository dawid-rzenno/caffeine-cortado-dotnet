using cortado.Models;
using cortado.Services;
using Dapper;

namespace cortado.Repositories;

public interface INutrientTypesRepository : ICrudRepository<NutrientType, NutrientType>
{
    public Task<IEnumerable<NutrientType>> GetAllByTermAsync(string term, bool globalSearch);
}

public class NutrientTypesRepository(DapperContext context, ICurrentUserService currentUserService)
    : INutrientTypesRepository
{
    public async Task<IEnumerable<NutrientType>> GetAllAsync()
    {
        var query = "SELECT * FROM NutrientTypes WHERE UserId = @UserId";

        using var connection = context.CreateConnection();
        return await connection.QueryAsync<NutrientType>(query, new { UserId = currentUserService.GetUserId() });
    }

    public async Task<IEnumerable<NutrientType>> GetAllByTermAsync(string term, bool globalSearch)
    {
        var query = globalSearch
            ? "SELECT TOP 10 * FROM NutrientTypes WHERE Name LIKE @Term"
            : "SELECT TOP 10 * FROM NutrientTypes WHERE Name LIKE @Term AND UserId = @UserId";

        using var connection = context.CreateConnection();

        return await connection.QueryAsync<NutrientType>(
            query,
            new { Term = $"%{term}%", UserId = currentUserService.GetUserId() }
        );
    }

    public async Task<NutrientType?> GetByIdAsync(int id)
    {
        var query =
            """
                SELECT * FROM NutrientTypes 
                WHERE Id = @Id AND UserId = @UserId
            """;

        using var connection = context.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<NutrientType>(
            query,
            new { Id = id, UserId = currentUserService.GetUserId() }
        );
    }

    public async Task<NutrientType> CreateAsync(NutrientType nutrientType)
    {
        var query =
            """
                INSERT INTO NutrientTypes (Name, Timestamp, UserId) 
                OUTPUT INSERTED.*
                VALUES (@Name, @Timestamp, @UserId)
            """;

        nutrientType.Timestamp = DateTime.UtcNow;
        nutrientType.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<NutrientType>(query, nutrientType);
    }
    
    public Task<NutrientType> UpdateAsync(NutrientType nutrientType)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var query =
            """
                DELETE FROM NutrientTypes 
                WHERE Id = @Id
            """;

        using var connection = context.CreateConnection();
        var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
        return affectedRows > 0;
    }
}