using cortado.DTOs;
using cortado.Models;
using cortado.Services;
using Dapper;

namespace cortado.Repositories;

public interface IGoalsRepository : ICrudRepository<Goal, GoalResponse>
{
}

public class GoalsRepository(DapperContext context, ICurrentUserService currentUserService) : IGoalsRepository
{
    public async Task<IEnumerable<Goal>> GetAllAsync()
    {
        var query = "SELECT * FROM Goals";

        using var connection = context.CreateConnection();
        return await connection.QueryAsync<Goal>(query);
    }

    public async Task<GoalResponse?> GetByIdAsync(int id)
    {
        var goalQuery = """
                        SELECT * FROM Goals 
                        WHERE Id = @Id
                    """;
        
        var milestonesQuery = """
                                  SELECT * FROM Milestones 
                                  WHERE GoalId = @GoalId
                              """;
        
        using var connection = context.CreateConnection();

        Goal? goal = await connection.QueryFirstOrDefaultAsync<Goal>(goalQuery, new { Id = id });

        if (goal == null) return null;
        
        IEnumerable<Milestone> milestones = await connection.QueryAsync<Milestone>(milestonesQuery, new { GoalId = id });
		
        return new GoalResponse(goal, milestones);
    }

    public async Task<Goal> CreateAsync(Goal goal)
    {
        var createGoalQuery = """
                                  INSERT INTO Goals (Name, Timestamp, UserId) 
                                  OUTPUT INSERTED.*
                                  VALUES (@Name, @Timestamp, @UserId)
                              """;

        goal.Timestamp = DateTime.UtcNow;
        goal.UserId = (int)currentUserService.GetUserId()!;

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<Goal>(createGoalQuery, goal);
    }

    public async Task<Goal> UpdateAsync(Goal goal)
    {
        var query = """
                        UPDATE Goals SET Name = @Name, Timestamp = @Timestamp, UserId = @UserId
                        OUTPUT INSERTED.*
                        WHERE Id = @Id
                    """;

        goal.Timestamp = DateTime.UtcNow;
        goal.UserId = (int)currentUserService.GetUserId()!;

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<Goal>(query, goal);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var query = """
                        DELETE FROM Goals 
                        WHERE Id = @Id
                    """;

        using var connection = context.CreateConnection();
        var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
        return affectedRows > 0;
    }
}