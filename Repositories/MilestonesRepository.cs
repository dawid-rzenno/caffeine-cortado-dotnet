using System.Data;
using cortado.Models;
using cortado.Repositories.Interfaces;
using cortado.Services;
using Dapper;

namespace cortado.Repositories;

public interface IMilestonesRepository : ICrudRepository<Milestone, Milestone>
{
}

public class MilestonesRepository(DapperContext context, ICurrentUserService currentUserService) : IMilestonesRepository
{
    public async Task<IEnumerable<Milestone>> GetAllAsync(
        string sort,
        string sortBy,
        int size,
        int page,
        string term,
        bool globalSearch
    )
    {
        using var connection = context.CreateConnection();
        return await connection.QueryAsync<Milestone>("ufn_GetMilestone", new
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

    public async Task<Milestone?> GetByIdAsync(int id)
    {
        using var connection = context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Milestone>("ufn_GetMilestone", new { Id = id },
            commandType: CommandType.Text);
    }

    public async Task<Milestone> CreateAsync(Milestone milestone)
    {
        milestone.Timestamp = DateTime.UtcNow;
        milestone.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<Milestone>("usp_CreateMilestone", milestone,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Milestone> UpdateAsync(Milestone milestone)
    {
        milestone.Timestamp = DateTime.UtcNow;
        milestone.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<Milestone>("usp_UpdateMilestone", milestone,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = context.CreateConnection();
        var affectedRows = await connection.ExecuteAsync("usp_DeleteMilestone", new { Id = id },
            commandType: CommandType.StoredProcedure);
        return affectedRows > 0;
    }
}