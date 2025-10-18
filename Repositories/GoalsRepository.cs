using System.Data;
using cortado.DTOs;
using cortado.Models;
using cortado.Repositories.Interfaces;
using cortado.Services;
using Dapper;

namespace cortado.Repositories;

public interface IGoalsRepository : ICrudRepository<Goal, GoalDetails>
{
}

public class GoalsRepository(DapperContext context, ICurrentUserService currentUserService) : IGoalsRepository
{
    public async Task<IEnumerable<Goal>> GetAllAsync(
        string sort,
        string sortBy,
        int size,
        int page,
        string term,
        bool globalSearch
    )
    {
        using var connection = context.CreateConnection();
        return await connection.QueryAsync<Goal>(
            "SELECT * FROM ufn_GetGoals(@UserId, @GlobalSearch, @Term, @Page, @Size, @SortBy, @Sort)",
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

    public async Task<GoalDetails?> GetByIdAsync(int id)
    {
        using var connection = context.CreateConnection();

        var goalDetailsDict = new Dictionary<int, GoalDetails>();

        IEnumerable<GoalDetails> goalDetails =
            await connection.QueryAsync<GoalDetails, Milestone, GoalDetails>(
                "SELECT * FROM ufn_GetGoal(@Id, @UserId)",
                (goalDetails, milestone) =>
                {
                    if (!goalDetailsDict.TryGetValue(goalDetails.Id, out var currentGoalDetails))
                    {
                        currentGoalDetails = goalDetails;
                        currentGoalDetails.Milestones = new List<Milestone>();
                        goalDetailsDict.Add(currentGoalDetails.Id, currentGoalDetails);
                    }

                    currentGoalDetails.Milestones.Add(milestone);

                    return currentGoalDetails;
                },
                new { Id = id, UserId = currentUserService.GetUserId() },
                splitOn: "MilestoneId",
                commandType: CommandType.Text
            );

        return goalDetails.FirstOrDefault();
    }

    public async Task<Goal> CreateAsync(Goal goal)
    {
        goal.Timestamp = DateTime.UtcNow;
        goal.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<Goal>("usp_CreateGoal", goal,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Goal> UpdateAsync(Goal goal)
    {
        goal.Timestamp = DateTime.UtcNow;
        goal.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<Goal>("usp_UpdateGoal", goal,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = context.CreateConnection();
        var affectedRows = await connection.ExecuteAsync("usp_DeleteGoal", new { Id = id },
            commandType: CommandType.StoredProcedure);
        return affectedRows > 0;
    }
}