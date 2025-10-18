using System.Data;
using cortado.DTOs;
using cortado.Models;
using cortado.Repositories.Interfaces;
using cortado.Services;
using Dapper;

namespace cortado.Repositories;

public interface IIngredientsRepository : ICrudRepository<Ingredient, IngredientDetails>
{
}

public class IngredientsRepository(DapperContext context, ICurrentUserService currentUserService)
    : IIngredientsRepository
{
    public async Task<IEnumerable<Ingredient>> GetAllAsync(
        string sort,
        string sortBy,
        int size,
        int page,
        string term,
        bool globalSearch
    )
    {
        using var connection = context.CreateConnection();
        return await connection.QueryAsync<Ingredient>(
            "SELECT * FROM ufn_GetIngredients(@UserId, @GlobalSearch, @Term, @Page, @Size, @SortBy, @Sort)",
            new
            {
                Size = size,
                Page = page,
                Sort = sort,
                SortBy = sortBy,
                Term = term,
                GlobalSearch = globalSearch,
                UserId = currentUserService.GetUserId()
            }
        );
    }

    public async Task<IngredientDetails?> GetByIdAsync(int id)
    {
        using var connection = context.CreateConnection();

        var ingredientDetailsDict = new Dictionary<int, IngredientDetails>();

        IEnumerable<IngredientDetails> ingredientDetails = await connection.QueryAsync<
            IngredientDetails,
            IngredientNutrient,
            NutrientDetails,
            NutrientTypeDetails,
            MassUnit,
            IngredientDetails
        >(
            "SELECT * FROM ufn_GetIngredient(@Id, @UserId)",
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
            splitOn: "IngredientNutrientId, NutrientId, NutrientTypeId, MassUnitId"
        );

        return ingredientDetails.FirstOrDefault();
    }

    public async Task<Ingredient> CreateAsync(Ingredient ingredient)
    {
        ingredient.Timestamp = DateTime.UtcNow;
        ingredient.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<Ingredient>("usp_CreateIngredient", ingredient,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Ingredient> UpdateAsync(Ingredient ingredient)
    {
        ingredient.Timestamp = DateTime.UtcNow;
        ingredient.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<Ingredient>("usp_UpdateIngredient", ingredient,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = context.CreateConnection();
        var affectedRows = await connection.ExecuteAsync("usp_DeleteIngredient", new { Id = id },
            commandType: CommandType.StoredProcedure);
        return affectedRows > 0;
    }
}