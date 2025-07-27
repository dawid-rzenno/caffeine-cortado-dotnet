using cortado.Models;
using cortado.Services;
using Dapper;

namespace cortado.Repositories;

public interface IDietMealsRepository : ICrudRepository<DietMeal, DietMeal>
{
}

public class DietMealsRepository(DapperContext context, ICurrentUserService currentUserService) : IDietMealsRepository
{
    public async Task<IEnumerable<DietMeal>> GetAllAsync()
    {
        var query = "SELECT * FROM DietMeals";

        using var connection = context.CreateConnection();
        return await connection.QueryAsync<DietMeal>(query);
    }

    public async Task<DietMeal?> GetByIdAsync(int id)
    {
        var query = """
                        SELECT * FROM DietMeals 
                        WHERE Id = @Id
                    """;

        using var connection = context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<DietMeal>(query, new { Id = id });
    }

    public async Task<DietMeal> CreateAsync(DietMeal dietMeal)
    {
        var createDietMealQuery = """
                                  INSERT INTO DietMeals (DietId, MealId, MealDayIndex, MealIndex, UserId, Timestamp) 
                                  OUTPUT INSERTED.*
                                  VALUES (@DietId, @MealId, @MealDayIndex, @MealIndex, @UserId, @Timestamp)
                              """;
        
        dietMeal.Timestamp = DateTime.UtcNow;
        dietMeal.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<DietMeal>(createDietMealQuery, dietMeal);
    }

    public async Task<DietMeal> UpdateAsync(DietMeal dietMeal)
    {
        var query = """
                        UPDATE DietMeals SET MealDayIndex = @MealDayIndex, MealIndex = @MealIndex, UserId = @UserId, Timestamp = @Timestamp
                        OUTPUT INSERTED.*
                        WHERE Id = @Id
                    """;
        
        dietMeal.Timestamp = DateTime.UtcNow;
        dietMeal.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<DietMeal>(query, dietMeal);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var query = """
                        DELETE FROM DietMeals 
                        WHERE Id = @Id
                    """;

        using var connection = context.CreateConnection();
        var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
        return affectedRows > 0;
    }
}