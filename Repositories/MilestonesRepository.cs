using cortado.Models;
using cortado.Services;
using Dapper;

namespace cortado.Repositories;

public interface IMilestonesRepository : ICrudRepository<Milestone, Milestone>
{
}

public class MilestonesRepository(DapperContext context, ICurrentUserService currentUserService) : IMilestonesRepository
{
    public async Task<IEnumerable<Milestone>> GetAllAsync()
    {
        var query = "SELECT * FROM Milestones";

        using var connection = context.CreateConnection();
        return await connection.QueryAsync<Milestone>(query);
    }

    public async Task<Milestone?> GetByIdAsync(int id)
    {
        var query = """
                        SELECT * FROM Milestones 
                        WHERE Id = @Id
                    """;

        using var connection = context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Milestone>(query, new { Id = id });
    }

    public async Task<Milestone> CreateAsync(Milestone milestone)
    {
        var createMilestoneQuery = """
                                  INSERT INTO Milestones (Name, GoalId, Timestamp, UserId) 
                                  OUTPUT INSERTED.*
                                  VALUES (@Name, @GoalId, @Timestamp, @UserId)
                              """;

        milestone.Timestamp = DateTime.UtcNow;
        milestone.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<Milestone>(createMilestoneQuery, milestone);
    }

    public async Task<Milestone> UpdateAsync(Milestone milestone)
    {
        var query = """
                        UPDATE Milestones SET Name = @Name, GoalId = @GoalId, Timestamp = @Timestamp, UserId = @UserId
                        OUTPUT INSERTED.*
                        WHERE Id = @Id
                    """;

        milestone.Timestamp = DateTime.UtcNow;
        milestone.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<Milestone>(query, milestone);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var query = """
                        DELETE FROM Milestones 
                        WHERE Id = @Id
                    """;

        using var connection = context.CreateConnection();
        var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
        return affectedRows > 0;
    }
}