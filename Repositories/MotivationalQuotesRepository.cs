using cortado.Models;
using Dapper;

namespace cortado.Repositories;

public interface IMotivationalQuotesRepository : ICrudRepository<MotivationalQuote, MotivationalQuote>
{
    public Task<MotivationalQuote?> GetRandomAsync();
}

public class MotivationalQuotesRepository(DapperContext context) : IMotivationalQuotesRepository
{
    public async Task<IEnumerable<MotivationalQuote>> GetAllAsync()
    {
        var query = "SELECT * FROM MotivationalQuotes";

        using var connection = context.CreateConnection();
        return await connection.QueryAsync<MotivationalQuote>(query);
    }
    
    public async Task<MotivationalQuote?> GetRandomAsync()
    {
        var query = """
                        SELECT TOP 1 * FROM MotivationalQuotes ORDER BY NEWID();
                    """;

        using var connection = context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<MotivationalQuote>(query);
    }

    public async Task<MotivationalQuote?> GetByIdAsync(int id)
    {
        var query = """
                        SELECT * FROM MotivationalQuotes 
                        WHERE Id = @Id
                    """;

        using var connection = context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<MotivationalQuote>(query, new { Id = id });
    }

    public async Task<MotivationalQuote> CreateAsync(MotivationalQuote motivationalQuote)
    {
        var createMotivationalQuoteQuery = """
                                  INSERT INTO MotivationalQuotes (Quote, Author) 
                                  OUTPUT INSERTED.*
                                  VALUES (@Quote, @Author)
                              """;

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<MotivationalQuote>(createMotivationalQuoteQuery, motivationalQuote);
    }

    public async Task<MotivationalQuote> UpdateAsync(MotivationalQuote motivationalQuote)
    {
        var query = """
                        UPDATE MotivationalQuotes SET Quote = @Quote, Author = @Author
                        OUTPUT INSERTED.*
                        WHERE Id = @Id
                    """;

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<MotivationalQuote>(query, motivationalQuote);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var query = """
                        DELETE FROM MotivationalQuotes 
                        WHERE Id = @Id
                    """;

        using var connection = context.CreateConnection();
        var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
        return affectedRows > 0;
    }
}