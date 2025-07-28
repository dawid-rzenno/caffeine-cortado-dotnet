using cortado.DTOs;
using cortado.Models;
using cortado.Services;
using Dapper;

namespace cortado.Repositories;

public interface IIngredientsRepository : ICrudRepository<Ingredient, IngredientDetails>
{
    public Task<IEnumerable<Ingredient>> GetAllByTermAsync(string term, bool globalSearch);
}

public class IngredientsRepository(DapperContext context, ICurrentUserService currentUserService)
    : IIngredientsRepository
{
    public async Task<IEnumerable<Ingredient>> GetAllAsync()
    {
        var query = "SELECT * FROM Ingredients WHERE UserId = @UserId";

        using var connection = context.CreateConnection();
        return await connection.QueryAsync<Ingredient>(query, new { UserId = currentUserService.GetUserId() });
    }

    public async Task<IEnumerable<Ingredient>> GetAllByTermAsync(string term, bool globalSearch)
    {
        var query = globalSearch
            ? "SELECT TOP 10 * FROM Ingredients WHERE Name LIKE @Term"
            : "SELECT TOP 10 * FROM Ingredients WHERE Name LIKE @Term AND UserId = @UserId";

        using var connection = context.CreateConnection();

        return await connection.QueryAsync<Ingredient>(query,
            new { Term = $"%{term}%", UserId = currentUserService.GetUserId() });
    }

    public async Task<IngredientDetails?> GetByIdAsync(int id)
    {
        var ingredientQuery =
            """
                SELECT i.*, NULL AS IngredientNutrient, inn.*, NULL AS Nutrient, n.*, NULL AS NutrientType, nt.*, NULL AS MassUnit, mu.*
                FROM Ingredients as i
                    LEFT JOIN IngredientNutrients AS inn ON inn.IngredientId = i.Id
                        LEFT JOIN Nutrients AS n ON inn.NutrientId = n.Id
                            LEFT JOIN NutrientTypes AS nt ON n.TypeId = nt.Id
                                LEFT JOIN MassUnits AS mu ON nt.MassUnitId = mu.Id
                WHERE i.Id = @Id AND i.UserId = @UserId
            """;

        using var connection = context.CreateConnection();

        var dietMealsExistsQuery =
            "SELECT CAST(CASE WHEN EXISTS (SELECT 1 FROM IngredientNutrients WHERE IngredientId = @Id) THEN 1 ELSE 0 END AS BIT)";

        if (await connection.ExecuteScalarAsync<bool>(dietMealsExistsQuery, new { Id = id }))
        {
            var ingredientDetailsDict = new Dictionary<int, IngredientDetails>();

            IEnumerable<IngredientDetails> ingredientDetails = await connection.QueryAsync<
                IngredientDetails,
                IngredientNutrient,
                NutrientDetails,
                NutrientTypeDetails,
                MassUnit,
                IngredientDetails
            >(
                ingredientQuery,
                (nutrientDetails, _, nutrient, nutrientType, massUnit) =>
                {
                    if (!ingredientDetailsDict.TryGetValue(nutrientDetails.Id, out var currentIngredient))
                    {
                        currentIngredient = nutrientDetails;
                        currentIngredient.Nutrients = new List<NutrientDetails>();
                        ingredientDetailsDict.Add(currentIngredient.Id, currentIngredient);
                    }

                    nutrientType.MassUnit = massUnit;

                    currentIngredient.Nutrients.Add(new NutrientDetails(nutrient, nutrientType));

                    return currentIngredient;
                },
                new { Id = id, UserId = currentUserService.GetUserId() },
                splitOn: "IngredientNutrient, Nutrient, NutrientType, MassUnit"
            );

            return ingredientDetails.FirstOrDefault();
        }

        ingredientQuery =
            """
                SELECT * 
                FROM Ingredients 
                WHERE Id = @Id AND UserId = @UserId
            """;

        return await connection.QuerySingleOrDefaultAsync<IngredientDetails>(
            ingredientQuery,
            new { Id = id, UserId = currentUserService.GetUserId() }
        );
    }

    public async Task<Ingredient> CreateAsync(Ingredient ingredient)
    {
        var createIngredientQuery =
            """
                INSERT INTO Ingredients (Name, Timestamp, UserId) 
                OUTPUT INSERTED.*
                VALUES (@Name, @Timestamp, @UserId)
            """;

        ingredient.Timestamp = DateTime.UtcNow;
        ingredient.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<Ingredient>(createIngredientQuery, ingredient);
    }

    public async Task<Ingredient> UpdateAsync(Ingredient ingredient)
    {
        var query =
            """
                UPDATE Ingredients SET Name = @Name, Timestamp = @Timestamp, UserId = @UserId
                OUTPUT INSERTED.*
                WHERE Id = @Id AND UserId = @UserId
            """;

        ingredient.Timestamp = DateTime.UtcNow;
        ingredient.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<Ingredient>(query, ingredient);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var query =
            """
                DELETE FROM Ingredients 
                WHERE Id = @Id
            """;

        using var connection = context.CreateConnection();
        var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
        return affectedRows > 0;
    }
}