using System.Data;
using cortado.Models;
using cortado.Repositories.Interfaces;
using cortado.Services;
using Dapper;

namespace cortado.Repositories;

public interface IIngredientNutrientsRepository : ICrudRepository<IngredientNutrient, IngredientNutrient>
{
}

public class IngredientNutrientsRepository(DapperContext context, ICurrentUserService currentUserService)
    : IIngredientNutrientsRepository
{
    public async Task<IngredientNutrient> CreateAsync(IngredientNutrient ingredientNutrient)
    {
        ingredientNutrient.Timestamp = DateTime.UtcNow;
        ingredientNutrient.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<IngredientNutrient>("usp_AssignNutrientToIngredient",
            ingredientNutrient, commandType: CommandType.StoredProcedure);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = context.CreateConnection();
        var affectedRows = await connection.ExecuteAsync("usp_UnassignNutrientFromIngredient",
            new { Id = id, UserId = currentUserService.GetUserId() },
            commandType: CommandType.StoredProcedure);
        return affectedRows > 0;
    }
}