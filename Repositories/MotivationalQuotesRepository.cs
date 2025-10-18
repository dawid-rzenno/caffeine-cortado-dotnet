using System.Data;
using cortado.Models;
using cortado.Repositories.Interfaces;
using cortado.Services;
using Dapper;

namespace cortado.Repositories;

public interface IMotivationalQuotesRepository : ICrudRepository<MotivationalQuote, MotivationalQuote>
{
    public Task<MotivationalQuote?> GetRandomAsync();
}

// Todo: option to select user's favourite athletes and quotes based on that choice
public class MotivationalQuotesRepository(DapperContext context, ICurrentUserService currentUserService)
    : IMotivationalQuotesRepository
{
    public async Task<IEnumerable<MotivationalQuote>> GetAllAsync(
        string sort,
        string sortBy,
        int size,
        int page,
        string term,
        bool globalSearch
    )
    {
        using var connection = context.CreateConnection();
        return await connection.QueryAsync<MotivationalQuote>(
            "SELECT * FROM ufn_GetMotivationalQuotes(@UserId, @GlobalSearch, @Term, @Page, @Size, @SortBy, @Sort)",
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

    public async Task<MotivationalQuote?> GetRandomAsync()
    {
        using var connection = context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<MotivationalQuote>(
            "SELECT * FROM ufn_GetRandomMotivationalQuote()"
        );
    }

    public async Task<MotivationalQuote?> GetByIdAsync(int id)
    {
        using var connection = context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<MotivationalQuote>(
            "SELECT * FROM ufn_GetMotivationalQuote(@Id, @UserId)",
            new { Id = id, UserId = currentUserService.GetUserId() }
        );
    }

    public async Task<MotivationalQuote> CreateAsync(MotivationalQuote motivationalQuote)
    {
        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<MotivationalQuote>("usp_CreateMotivationalQuote", motivationalQuote,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<MotivationalQuote> UpdateAsync(MotivationalQuote motivationalQuote)
    {
        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<MotivationalQuote>("usp_UpdateMotivationalQuote", motivationalQuote,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = context.CreateConnection();
        var affectedRows = await connection.ExecuteAsync("usp_DeleteMotivationalQuote", new { Id = id },
            commandType: CommandType.StoredProcedure);
        return affectedRows > 0;
    }
}