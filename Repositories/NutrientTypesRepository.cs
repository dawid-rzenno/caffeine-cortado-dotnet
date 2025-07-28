using cortado.DTOs;
using cortado.Models;
using cortado.Services;
using Dapper;

namespace cortado.Repositories;

public interface INutrientTypesRepository : ICrudRepository<NutrientType, NutrientTypeDetails>
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

    public async Task<NutrientTypeDetails?> GetByIdAsync(int id)
    {
        var query =
            """
                SELECT nt.*, NULL as MassUnit, mu.*
                FROM NutrientTypes AS nt
                    LEFT JOIN MassUnits AS mu ON nt.MassUnitId = mu.Id
                WHERE nt.Id = @Id AND nt.UserId = @UserId
            """;

        using var connection = context.CreateConnection();
        
        IEnumerable<NutrientTypeDetails> nutrientTypeDetails = await connection.QueryAsync<
            NutrientTypeDetails,
            MassUnit,
            NutrientTypeDetails
        >(
            query,
            (nutrientTypeDetails, massUnit) =>
            {
                nutrientTypeDetails.MassUnit = massUnit;

                return nutrientTypeDetails;
            },
            new { Id = id, UserId = currentUserService.GetUserId() },
            splitOn: "MassUnit"
        );

        return nutrientTypeDetails.FirstOrDefault();
    }

    public async Task<NutrientType> CreateAsync(NutrientType nutrientType)
    {
        var query =
            """
                INSERT INTO NutrientTypes (Name, MassUnitId, Timestamp, UserId) 
                OUTPUT INSERTED.*
                VALUES (@Name, @MassUnitId, @Timestamp, @UserId)
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