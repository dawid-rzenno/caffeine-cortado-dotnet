using System.Data;
using cortado.DTOs;
using cortado.Models;
using cortado.Repositories.Interfaces;
using cortado.Services;
using Dapper;

namespace cortado.Repositories;

public interface INutrientTypesRepository : ICrudRepository<NutrientType, NutrientTypeDetails>
{
}

public class NutrientTypesRepository(DapperContext context, ICurrentUserService currentUserService)
    : INutrientTypesRepository
{
    public async Task<IEnumerable<NutrientType>> GetAllAsync(
        string sort,
        string sortBy,
        int size,
        int page,
        string term,
        bool globalSearch
    )
    {
        using var connection = context.CreateConnection();
        return await connection.QueryAsync<NutrientType>("ufn_GetNutrientTypes",
            new
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

    public async Task<NutrientTypeDetails?> GetByIdAsync(int id)
    {
        using var connection = context.CreateConnection();

        IEnumerable<NutrientTypeDetails> nutrientTypeDetails = await connection.QueryAsync<
            NutrientTypeDetails,
            MassUnit,
            NutrientTypeDetails
        >(
            "ufn_GetNutrientType",
            (nutrientTypeDetails, massUnit) =>
            {
                nutrientTypeDetails.MassUnit = massUnit;

                return nutrientTypeDetails;
            },
            new { Id = id, UserId = currentUserService.GetUserId() },
            splitOn: "MassUnitId",
            commandType: CommandType.Text
        );

        return nutrientTypeDetails.FirstOrDefault();
    }

    public async Task<NutrientType> CreateAsync(NutrientType nutrientType)
    {
        nutrientType.Timestamp = DateTime.UtcNow;
        nutrientType.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<NutrientType>("usp_CreateNutrientType", nutrientType,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<NutrientType> UpdateAsync(NutrientType nutrientType)
    {
        nutrientType.Timestamp = DateTime.UtcNow;
        nutrientType.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<NutrientType>("usp_UpdateNutrientType", nutrientType,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = context.CreateConnection();
        var affectedRows = await connection.ExecuteAsync("usp_DeleteNutrientType", new { Id = id },
            commandType: CommandType.StoredProcedure);
        return affectedRows > 0;
    }
}