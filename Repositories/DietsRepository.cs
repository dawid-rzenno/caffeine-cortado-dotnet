using System.Data;
using cortado.DTOs;
using cortado.Models;
using cortado.Repositories.Interfaces;
using cortado.Services;
using Dapper;

namespace cortado.Repositories;

public interface IDietsRepository : ICrudRepository<Diet, DietDetails>
{
}

public class DietsRepository(DapperContext context, ICurrentUserService currentUserService) : IDietsRepository
{
    public async Task<IEnumerable<Diet>> GetAllAsync(
        string sort,
        string sortBy,
        int size,
        int page,
        string term,
        bool globalSearch
    )
    {
        using var connection = context.CreateConnection();
        return await connection.QueryAsync<Diet>(
            "SELECT * FROM ufn_GetDiets(@UserId, @GlobalSearch, @Term, @Page, @Size, @SortBy, @Sort)",
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

    public async Task<DietDetails?> GetByIdAsync(int id)
    {
        using var connection = context.CreateConnection();

        var dietDetailsDict = new Dictionary<int, DietDetails>();

        IEnumerable<DietDetails> dietDetails =
            await connection.QueryAsync<DietDetails, DietMeal, Meal, DietDetails>(
                "SELECT * FROM ufn_GetDiet(@Id, @UserId)",
                (dietDetails, dietMeal, meal) =>
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
                splitOn: "DietMealId, MealId"
            );

        return dietDetails.FirstOrDefault();
    }

    public async Task<Diet> CreateAsync(Diet diet)
    {
        diet.Timestamp = DateTime.UtcNow;
        diet.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<Diet>("usp_CreateDiet", diet,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Diet> UpdateAsync(Diet diet)
    {
        diet.Timestamp = DateTime.UtcNow;
        diet.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<Diet>("usp_UpdateDiet", diet,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = context.CreateConnection();
        var affectedRows = await connection.ExecuteAsync("usp_DeleteDiet", new { Id = id },
            commandType: CommandType.StoredProcedure);
        return affectedRows > 0;
    }
}