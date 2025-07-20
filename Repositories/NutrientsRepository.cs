using cortado.DTOs;
using cortado.Models;
using cortado.Services;
using Dapper;

namespace cortado.Repositories;

public interface INutrientsRepository : ICrudRepository<Nutrient, NutrientDetails>
{
}

public class NutrientsRepository(DapperContext context, ICurrentUserService currentUserService) : INutrientsRepository
{
    public async Task<IEnumerable<Nutrient>> GetAllAsync()
    {
        var query = "SELECT * FROM Nutrients WHERE UserId = @UserId";

        using var connection = context.CreateConnection();
        return await connection.QueryAsync<Nutrient>(query, new { UserId = currentUserService.GetUserId() });
    }

    public async Task<NutrientDetails?> GetByIdAsync(int id)
    {
        var nutrientQuery = """
                        SELECT * FROM Nutrients 
                        WHERE Id = @Id AND UserId = @UserId
                    """;
        
        using var connection = context.CreateConnection();
        
        Nutrient? nutrient = await connection.QueryFirstOrDefaultAsync<Nutrient>(nutrientQuery, new { Id = id, UserId = currentUserService.GetUserId() });

        if (nutrient == null) return null;
		
        return new NutrientDetails(nutrient, new MassUnit());
    }

    public async Task<Nutrient> CreateAsync(Nutrient nutrient)
    {
        var createNutrientQuery = """
                                  INSERT INTO Nutrients (Name, Timestamp, UserId) 
                                  OUTPUT INSERTED.*
                                  VALUES (@Name, @Timestamp, @UserId)
                              """;

        nutrient.Timestamp = DateTime.UtcNow;
        nutrient.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<Nutrient>(createNutrientQuery, nutrient);
    }

    public async Task<Nutrient> UpdateAsync(Nutrient nutrient)
    {
        var query = """
                        UPDATE Nutrients SET Name = @Name, Timestamp = @Timestamp, UserId = @UserId
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
        var query = """
                        DELETE FROM Nutrients 
                        WHERE Id = @Id
                    """;

        using var connection = context.CreateConnection();
        var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
        return affectedRows > 0;
    }
}