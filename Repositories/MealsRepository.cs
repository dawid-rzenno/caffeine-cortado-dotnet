using System.Data;
using cortado.DTOs;
using cortado.Models;
using cortado.Repositories.Interfaces;
using cortado.Services;
using Dapper;

namespace cortado.Repositories;

public interface IMealsRepository : ICrudRepository<Meal, MealDetails>
{
}

public class MealsRepository(DapperContext context, ICurrentUserService currentUserService) : IMealsRepository
{
    public async Task<IEnumerable<Meal>> GetAllAsync(
        string sort,
        string sortBy,
        int size,
        int page,
        string term,
        bool globalSearch)
    {
        using var connection = context.CreateConnection();
        return await connection.QueryAsync<Meal>("ufn_GetMeals", new
            {
                Size = size,
                Page = page,
                Sort = sort,
                SortBy = sortBy,
                Term = term,
                GlobalSearch = globalSearch,
                UserId = currentUserService.GetUserId()
            },
            commandType: CommandType.Text);
    }

    public async Task<MealDetails?> GetByIdAsync(int id)
    {
        using var connection = context.CreateConnection();

        Meal? meal =
            await connection.QueryFirstOrDefaultAsync<Meal>("ufn_GetMeal",
                new { Id = id, UserId = currentUserService.GetUserId() }, commandType: CommandType.Text);

        if (meal == null) return null;

        return new MealDetails(meal, new List<Ingredient>());
    }

    public async Task<Meal> CreateAsync(Meal meal)
    {
        meal.Timestamp = DateTime.UtcNow;
        meal.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<Meal>("usp_CreateMeal", meal,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Meal> UpdateAsync(Meal meal)
    {
        meal.Timestamp = DateTime.UtcNow;
        meal.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<Meal>("usp_UpdateMeal", meal,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = context.CreateConnection();
        var affectedRows = await connection.ExecuteAsync("usp_DeleteMeal", new { Id = id },
            commandType: CommandType.StoredProcedure);
        return affectedRows > 0;
    }
}