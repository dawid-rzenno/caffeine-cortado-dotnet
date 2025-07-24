using cortado.DTOs;
using cortado.Models;
using cortado.Services;
using Dapper;

namespace cortado.Repositories;

public interface IMealsRepository : ICrudRepository<Meal, MealDetails>
{
    public Task<IEnumerable<Meal>> GetAllByTermAsync(string term, bool globalSearch);
}

public class MealsRepository(DapperContext context, ICurrentUserService currentUserService) : IMealsRepository
{
    public async Task<IEnumerable<Meal>> GetAllAsync()
    {
        var query = "SELECT * FROM Meals WHERE UserId = @UserId";

        using var connection = context.CreateConnection();
        return await connection.QueryAsync<Meal>(query, new { UserId = currentUserService.GetUserId() });
    }
    
    public async Task<IEnumerable<Meal>> GetAllByTermAsync(string term, bool globalSearch)
    {
        var query = globalSearch
            ? "SELECT TOP 10 * FROM Meals WHERE Name LIKE @Term"
            : "SELECT TOP 10 * FROM Meals WHERE Name LIKE @Term AND UserId = @UserId";
        
        using var connection = context.CreateConnection();

        return await connection.QueryAsync<Meal>(query, new { Term = $"%{term}%", UserId = currentUserService.GetUserId() });
    }

    public async Task<MealDetails?> GetByIdAsync(int id)
    {
        var mealQuery = """
                        SELECT * FROM Meals 
                        WHERE Id = @Id AND UserId = @UserId
                    """;
        
        using var connection = context.CreateConnection();
        
        Meal? meal = await connection.QueryFirstOrDefaultAsync<Meal>(mealQuery, new { Id = id, UserId = currentUserService.GetUserId() });

        if (meal == null) return null;
		
        return new MealDetails(meal, new List<Ingredient>());
    }

    public async Task<Meal> CreateAsync(Meal meal)
    {
        var createMealQuery = """
                                  INSERT INTO Meals (Name, Timestamp, UserId) 
                                  OUTPUT INSERTED.*
                                  VALUES (@Name, @Timestamp, @UserId)
                              """;

        meal.Timestamp = DateTime.UtcNow;
        meal.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<Meal>(createMealQuery, meal);
    }

    public async Task<Meal> UpdateAsync(Meal meal)
    {
        var query = """
                        UPDATE Meals SET Name = @Name, Timestamp = @Timestamp, UserId = @UserId
                        OUTPUT INSERTED.*
                        WHERE Id = @Id AND UserId = @UserId
                    """;

        meal.Timestamp = DateTime.UtcNow;
        meal.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<Meal>(query, meal);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var query = """
                        DELETE FROM Meals 
                        WHERE Id = @Id
                    """;

        using var connection = context.CreateConnection();
        var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
        return affectedRows > 0;
    }
}