using cortado.DTOs;
using cortado.Models;
using cortado.Services;
using Dapper;

namespace cortado.Repositories;

public interface IDietDaysRepository : ICrudRepository<DietDay, DietDayDetails>
{
}

public class DietDaysRepository(DapperContext context, ICurrentUserService currentUserService) : IDietDaysRepository
{
    public async Task<IEnumerable<DietDay>> GetAllAsync()
    {
        var query = "SELECT * FROM DietDays WHERE UserId = @UserId";

        using var connection = context.CreateConnection();
        return await connection.QueryAsync<DietDay>(query, new { UserId = currentUserService.GetUserId() });
    }

    public async Task<DietDayDetails?> GetByIdAsync(int id)
    {
        var dietDayQuery = """
                        SELECT * FROM DietDays 
                        WHERE Id = @Id AND UserId = @UserId
                    """;
        
        using var connection = context.CreateConnection();
        
        DietDay? dietDay = await connection.QueryFirstOrDefaultAsync<DietDay>(dietDayQuery, new { Id = id, UserId = currentUserService.GetUserId() });

        if (dietDay == null) return null;
		
        return new DietDayDetails(dietDay, new List<Meal>());
    }

    public async Task<DietDay> CreateAsync(DietDay dietDay)
    {
        var createDietDayQuery = """
                                  INSERT INTO DietDays (Name, Timestamp, UserId) 
                                  OUTPUT INSERTED.*
                                  VALUES (@Name, @Timestamp, @UserId)
                              """;

        dietDay.Timestamp = DateTime.UtcNow;
        dietDay.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<DietDay>(createDietDayQuery, dietDay);
    }

    public async Task<DietDay> UpdateAsync(DietDay dietDay)
    {
        var query = """
                        UPDATE DietDays SET Name = @Name, Timestamp = @Timestamp, UserId = @UserId
                        OUTPUT INSERTED.*
                        WHERE Id = @Id AND UserId = @UserId
                    """;

        dietDay.Timestamp = DateTime.UtcNow;
        dietDay.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<DietDay>(query, dietDay);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var query = """
                        DELETE FROM DietDays 
                        WHERE Id = @Id
                    """;

        using var connection = context.CreateConnection();
        var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
        return affectedRows > 0;
    }
}