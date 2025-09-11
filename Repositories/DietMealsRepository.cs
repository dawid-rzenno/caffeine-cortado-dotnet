using System.Data;
using cortado.Models;
using cortado.Repositories.Interfaces;
using cortado.Services;
using Dapper;

namespace cortado.Repositories;

public interface IDietMealsRepository : ICrudRepository<DietMeal, DietMeal>
{
}

public class DietMealsRepository(DapperContext context, ICurrentUserService currentUserService) : IDietMealsRepository
{
    public async Task<DietMeal> CreateAsync(DietMeal dietMeal)
    {
        dietMeal.Timestamp = DateTime.UtcNow;
        dietMeal.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<DietMeal>("usp_AssignMealToDiet", dietMeal,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = context.CreateConnection();
        var affectedRows = await connection.ExecuteAsync("usp_UnassignMealFromDiet", new { Id = id },
            commandType: CommandType.StoredProcedure);
        return affectedRows > 0;
    }
}