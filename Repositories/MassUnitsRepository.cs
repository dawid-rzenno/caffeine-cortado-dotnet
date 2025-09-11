using System.Data;
using cortado.Models;
using cortado.Repositories.Interfaces;
using cortado.Services;
using Dapper;

namespace cortado.Repositories;

public interface IMassUnitsRepository : ICrudRepository<MassUnit, MassUnit>
{
}

public class MassUnitsRepository(DapperContext context, ICurrentUserService currentUserService) : IMassUnitsRepository
{
    public async Task<IEnumerable<MassUnit>> GetAllAsync(
        string sort,
        string sortBy,
        int size,
        int page,
        string term,
        bool globalSearch
    )
    {
        using var connection = context.CreateConnection();
        return await connection.QueryAsync<MassUnit>("ufn_GetMassUnits", new
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

    public async Task<MassUnit?> GetByIdAsync(int id)
    {
        using var connection = context.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<MassUnit>("ufn_GetMassUnit",
            new { Id = id, UserId = currentUserService.GetUserId() });
    }

    public async Task<MassUnit> CreateAsync(MassUnit massUnit)
    {
        massUnit.Timestamp = DateTime.UtcNow;
        massUnit.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<MassUnit>("usp_CreateMassUnit", massUnit, commandType: CommandType.StoredProcedure);
    }

    public async Task<MassUnit> UpdateAsync(MassUnit massUnit)
    {

        massUnit.Timestamp = DateTime.UtcNow;
        massUnit.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<MassUnit>("usp_UpdateMassUnit", massUnit, commandType: CommandType.StoredProcedure);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = context.CreateConnection();
        var affectedRows = await connection.ExecuteAsync("usp_DeleteMassUnit", new { Id = id }, commandType: CommandType.StoredProcedure);
        return affectedRows > 0;
    }
}