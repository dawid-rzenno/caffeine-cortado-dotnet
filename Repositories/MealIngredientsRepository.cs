using cortado.Models;
using cortado.Services;
using Dapper;

namespace cortado.Repositories;

public interface IMealIngredientsRepository : ICrudRepository<MealIngredient, MealIngredient>
{
}

public class MealIngredientsRepository(DapperContext context, ICurrentUserService currentUserService) : IMealIngredientsRepository
{
    public async Task<IEnumerable<MealIngredient>> GetAllAsync()
    {
        var query = "SELECT * FROM MealIngredients";

        using var connection = context.CreateConnection();
        return await connection.QueryAsync<MealIngredient>(query);
    }

    public async Task<MealIngredient?> GetByIdAsync(int id)
    {
        var query = """
                        SELECT * FROM MealIngredients 
                        WHERE Id = @Id
                    """;

        using var connection = context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<MealIngredient>(query, new { Id = id });
    }

    public async Task<MealIngredient> CreateAsync(MealIngredient mealIngredient)
    {
        var createMealIngredientQuery = """
                                  INSERT INTO MealIngredients (MealId, IngredientId, UserId, Timestamp) 
                                  OUTPUT INSERTED.*
                                  VALUES (@MealId, @IngredientId, @UserId, @Timestamp)
                              """;
        
        mealIngredient.Timestamp = DateTime.UtcNow;
        mealIngredient.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<MealIngredient>(createMealIngredientQuery, mealIngredient);
    }

    public Task<MealIngredient> UpdateAsync(MealIngredient mealIngredient)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var query = """
                        DELETE FROM MealIngredients 
                        WHERE Id = @Id
                    """;

        using var connection = context.CreateConnection();
        var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
        return affectedRows > 0;
    }
}