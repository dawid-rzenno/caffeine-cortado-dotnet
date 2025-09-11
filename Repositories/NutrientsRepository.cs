using System.Data;
using cortado.DTOs;
using cortado.Models;
using cortado.Repositories.Interfaces;
using cortado.Services;
using Dapper;

namespace cortado.Repositories;

public interface INutrientsRepository : ICrudRepository<Nutrient, NutrientDetails>
{
}

public class NutrientsRepository(DapperContext context, ICurrentUserService currentUserService) : INutrientsRepository
{
    public async Task<IEnumerable<Nutrient>> GetAllAsync(
        string sort,
        string sortBy,
        int size,
        int page,
        string term,
        bool globalSearch
    )
    {
        using var connection = context.CreateConnection();
        return await connection.QueryAsync<Nutrient>("ufn_GetNutrients",
            new
            {
                Size = size,
                Page = page,
                Sort = sort,
                SortBy = sortBy,
                Term = term,
                GlobalSearch = globalSearch,
                UserId = currentUserService.GetUserId()
            }, commandType: CommandType.Text);
    }

    public async Task<NutrientDetails?> GetByIdAsync(int id)
    {
        using var connection = context.CreateConnection();

        IEnumerable<NutrientDetails> nutrientDetails = await connection.QueryAsync<
            NutrientDetails,
            NutrientTypeDetails,
            MassUnit,
            NutrientDetails
        >(
            "ufn_GetNutrient",
            (nutrientDetails, nutrientType, massUnit) =>
            {
                nutrientDetails.Type = nutrientType;
                nutrientDetails.Type.MassUnit = massUnit;

                return nutrientDetails;
            },
            new { Id = id, UserId = currentUserService.GetUserId() },
            splitOn: "NutrientTypeId, MassUnitId",
            commandType: CommandType.Text
        );

        return nutrientDetails.SingleOrDefault();
    }

    public async Task<Nutrient> CreateAsync(Nutrient nutrient)
    {
        nutrient.Timestamp = DateTime.UtcNow;
        nutrient.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<Nutrient>("usp_CreateNutrient", nutrient,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Nutrient> UpdateAsync(Nutrient nutrient)
    {
        nutrient.Timestamp = DateTime.UtcNow;
        nutrient.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<Nutrient>("usp_UpdateNutrient", nutrient,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = context.CreateConnection();
        var affectedRows = await connection.ExecuteAsync("usp_DeleteNutrient", new { Id = id },
            commandType: CommandType.StoredProcedure);
        return affectedRows > 0;
    }
}