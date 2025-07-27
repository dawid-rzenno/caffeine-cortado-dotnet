using cortado.DTOs;
using cortado.Models;
using cortado.Services;
using Dapper;

namespace cortado.Repositories;

public interface INutrientsRepository : ICrudRepository<Nutrient, NutrientDetails>
{
    public Task<IEnumerable<Nutrient>> GetAllByTermAsync(string term, bool globalSearch);
}

public class NutrientsRepository(DapperContext context, ICurrentUserService currentUserService) : INutrientsRepository
{
    public async Task<IEnumerable<Nutrient>> GetAllAsync()
    {
        var query =
            """
            SELECT * 
            FROM Nutrients 
            WHERE UserId = @UserId
            """;

        using var connection = context.CreateConnection();
        return await connection.QueryAsync<Nutrient>(query, new { UserId = currentUserService.GetUserId() });
    }

    public async Task<IEnumerable<Nutrient>> GetAllByTermAsync(string term, bool globalSearch)
    {
        var query = globalSearch
            ? """
              SELECT TOP 10 * 
              FROM Nutrients AS n 
                  LEFT JOIN NutrientNames AS nn ON n.NameId = nn.Id 
              WHERE Name LIKE @Term
              """
            : """
              SELECT TOP 10 n.*, nn.Name
              FROM Nutrients AS n 
                  LEFT JOIN NutrientNames AS nn ON n.NameId = nn.Id 
              WHERE Name LIKE @Term AND n.UserId = @UserId
              """;

        using var connection = context.CreateConnection();

        return await connection.QueryAsync<Nutrient>(query,
            new { Term = $"%{term}%", UserId = currentUserService.GetUserId() });
    }

    public async Task<NutrientDetails?> GetByIdAsync(int id)
    {
        var query =
            """
                SELECT n.*, NULL AS NutrientName, nn.*, NULL AS NutrientType, nt.*
                FROM Nutrients AS n
                    LEFT JOIN NutrientTypes AS nt ON n.TypeId = nt.Id
                    LEFT JOIN NutrientNames AS nn ON n.NameId = nn.Id
                WHERE n.Id = @Id AND n.UserId = @UserId
            """;

        using var connection = context.CreateConnection();

        IEnumerable<NutrientDetails> nutrientDetails = await connection.QueryAsync<
            NutrientDetails,
            NutrientName,
            NutrientType,
            NutrientDetails
        >(
            query,
            (nutrientDetails, nutrientName, nutrientType) =>
            {
                nutrientDetails.Name = nutrientName;
                nutrientDetails.Type = nutrientType;

                return nutrientDetails;
            },
            new { Id = id, UserId = currentUserService.GetUserId() },
            splitOn: "NutrientName, NutrientType"
        );

        return nutrientDetails.SingleOrDefault();
    }

    public async Task<Nutrient> CreateAsync(Nutrient nutrient)
    {
        var createNutrientQuery =
            """
                INSERT INTO Nutrients (NameId, TypeId, Amount, Timestamp, UserId) 
                OUTPUT INSERTED.*
                VALUES (@NameId, @TypeId, @Amount, @Timestamp, @UserId)
            """;

        nutrient.Timestamp = DateTime.UtcNow;
        nutrient.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<Nutrient>(createNutrientQuery, nutrient);
    }

    public async Task<Nutrient> UpdateAsync(Nutrient nutrient)
    {
        var query =
            """
                UPDATE Nutrients SET NameId = @NameId, TypeId = @TypeId, Timestamp = @Timestamp, UserId = @UserId
                OUTPUT INSERTED.*
                WHERE Id = @Id AND UserId = @UserId
            """;

        nutrient.Timestamp = DateTime.UtcNow;
        nutrient.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<Nutrient>(query, nutrient);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var query =
            """
                DELETE FROM Nutrients 
                WHERE Id = @Id
            """;

        using var connection = context.CreateConnection();
        var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
        return affectedRows > 0;
    }
}