using cortado.Models;
using cortado.Services;
using Dapper;

namespace cortado.Repositories;

public interface IIngredientNutrientsRepository : ICrudRepository<IngredientNutrient, IngredientNutrient>
{
}

public class IngredientNutrientsRepository(DapperContext context, ICurrentUserService currentUserService)
    : IIngredientNutrientsRepository
{
    public async Task<IEnumerable<IngredientNutrient>> GetAllAsync()
    {
        var query = "SELECT * FROM IngredientNutrients";

        using var connection = context.CreateConnection();
        return await connection.QueryAsync<IngredientNutrient>(query);
    }

    public async Task<IngredientNutrient?> GetByIdAsync(int id)
    {
        var query =
            """
                SELECT * FROM IngredientNutrients 
                WHERE Id = @Id
            """;

        using var connection = context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<IngredientNutrient>(query, new { Id = id });
    }

    public async Task<IngredientNutrient> CreateAsync(IngredientNutrient ingredientNutrient)
    {
        var query =
            """
                INSERT INTO IngredientNutrients (IngredientId, NutrientId, UserId, Timestamp) 
                OUTPUT INSERTED.*
                VALUES (@IngredientId, @NutrientId, @UserId, @Timestamp)
            """;

        ingredientNutrient.Timestamp = DateTime.UtcNow;
        ingredientNutrient.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<IngredientNutrient>(query, ingredientNutrient);
    }

    public Task<IngredientNutrient> UpdateAsync(IngredientNutrient ingredientNutrient)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var query =
            """
                DELETE FROM IngredientNutrients 
                WHERE Id = @Id
            """;

        using var connection = context.CreateConnection();
        var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
        return affectedRows > 0;
    }
}