using System.Data;
using cortado.Models;
using cortado.Repositories.Interfaces;
using cortado.Services;
using Dapper;

namespace cortado.Repositories;

public interface IMealIngredientsRepository : ICrudRepository<MealIngredient, MealIngredient>
{
}

public class MealIngredientsRepository(DapperContext context, ICurrentUserService currentUserService)
    : IMealIngredientsRepository
{
    public async Task<MealIngredient> CreateAsync(MealIngredient mealIngredient)
    {
        mealIngredient.Timestamp = DateTime.UtcNow;
        mealIngredient.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<MealIngredient>("usp_AssignIngredientToMeal", mealIngredient,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = context.CreateConnection();
        var affectedRows = await connection.ExecuteAsync("usp_UnassignIngredientFromMeal", new { Id = id },
            commandType: CommandType.StoredProcedure);
        return affectedRows > 0;
    }
}