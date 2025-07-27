using cortado.DTOs;
using cortado.Models;
using cortado.Services;
using Dapper;

namespace cortado.Repositories;

public interface IDietsRepository : ICrudRepository<Diet, DietDetails>
{
    public Task<IEnumerable<Diet>> GetAllByTermAsync(string term, bool globalSearch);
}

public class DietsRepository(DapperContext context, ICurrentUserService currentUserService) : IDietsRepository
{
    public async Task<IEnumerable<Diet>> GetAllAsync()
    {
        var query = "SELECT * FROM Diets WHERE UserId = @UserId";

        using var connection = context.CreateConnection();
        return await connection.QueryAsync<Diet>(query, new { UserId = currentUserService.GetUserId() });
    }

    public async Task<IEnumerable<Diet>> GetAllByTermAsync(string term, bool globalSearch)
    {
        var query = globalSearch
            ? "SELECT TOP 10 * FROM Diets WHERE Name LIKE @Term"
            : "SELECT TOP 10 * FROM Diets WHERE Name LIKE @Term AND UserId = @UserId";

        using var connection = context.CreateConnection();

        return await connection.QueryAsync<Diet>(query,
            new { Term = $"%{term}%", UserId = currentUserService.GetUserId() });
    }

    public async Task<DietDetails?> GetByIdAsync(int id)
    {
        using var connection = context.CreateConnection();


        var dietDetailsQuery =
            """
                SELECT d.*, NULL AS Meal, m.*, NULL AS DietMeal, dm.*
                FROM Diets AS d
                LEFT JOIN DietMeals AS dm ON d.Id = dm.DietId
                LEFT JOIN Meals AS m ON dm.MealId = m.Id
                WHERE d.Id = @Id AND d.UserId = @UserId
            """;

        var dietMealsExistsQuery =
            "SELECT CAST(CASE WHEN EXISTS (SELECT 1 FROM DietMeals WHERE DietId = @Id) THEN 1 ELSE 0 END AS BIT)";

        if (await connection.ExecuteScalarAsync<bool>(dietMealsExistsQuery, new { Id = id }) == false)
        {
            dietDetailsQuery =
                """
                    SELECT * 
                    FROM Diets 
                    WHERE Id = @Id AND UserId = @UserId
                """;

            return await connection.QuerySingleOrDefaultAsync<DietDetails>(
                dietDetailsQuery,
                new { Id = id, UserId = currentUserService.GetUserId() }
            );
        }

        var dietDetailsDict = new Dictionary<int, DietDetails>();

        IEnumerable<DietDetails> dietDetails = await connection.QueryAsync<DietDetails, Meal, DietMeal, DietDetails>(
            dietDetailsQuery,
            (dietDetails, meal, dietMeal) =>
            {
                if (!dietDetailsDict.TryGetValue(dietDetails.Id, out var currentDiet))
                {
                    currentDiet = dietDetails;
                    currentDiet.Meals = new List<DietMealDetails>();
                    dietDetailsDict.Add(currentDiet.Id, currentDiet);
                }

                currentDiet.Meals.Add(new DietMealDetails(dietMeal, meal));

                return currentDiet;
            },
            new { Id = id, UserId = currentUserService.GetUserId() },
            splitOn: "Meal, DietMeal"
        );

        return dietDetails.FirstOrDefault();
    }

    public async Task<Diet> CreateAsync(Diet diet)
    {
        var createDietQuery =
            """
                INSERT INTO Diets (Name, Timestamp, UserId) 
                OUTPUT INSERTED.*
                VALUES (@Name, @Timestamp, @UserId)
            """;

        diet.Timestamp = DateTime.UtcNow;
        diet.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<Diet>(createDietQuery, diet);
    }

    public async Task<Diet> UpdateAsync(Diet diet)
    {
        var query =
            """
                UPDATE Diets SET Name = @Name, Timestamp = @Timestamp, UserId = @UserId
                OUTPUT INSERTED.*
                WHERE Id = @Id AND UserId = @UserId
            """;

        diet.Timestamp = DateTime.UtcNow;
        diet.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<Diet>(query, diet);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var query =
            """
                DELETE FROM Diets 
                WHERE Id = @Id
            """;

        using var connection = context.CreateConnection();
        var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
        return affectedRows > 0;
    }
}