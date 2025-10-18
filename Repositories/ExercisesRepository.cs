using System.Data;
using cortado.DTOs;
using cortado.Models;
using cortado.Repositories.Interfaces;
using cortado.Services;
using Dapper;

namespace cortado.Repositories;

public interface IExercisesRepository : ICrudRepository<Exercise, ExerciseDetails>
{
}

public class ExercisesRepository(DapperContext context, ICurrentUserService currentUserService) : IExercisesRepository
{
    public async Task<IEnumerable<Exercise>> GetAllAsync(
        string sort,
        string sortBy,
        int size,
        int page,
        string term,
        bool globalSearch
    )
    {
        using var connection = context.CreateConnection();
        return await connection.QueryAsync<Exercise>(
            "SELECT * FROM ufn_GetExercises(@UserId, @GlobalSearch, @Term, @Page, @Size, @SortBy, @Sort)",
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

    public async Task<ExerciseDetails?> GetByIdAsync(int id)
    {
        using var connection = context.CreateConnection();

        Exercise? exercise = await connection.QueryFirstOrDefaultAsync<Exercise>(
            "SELECT * FROM ufn_GetExercise(@Id, @UserId)",
            new { Id = id, UserId = currentUserService.GetUserId() }
        );

        if (exercise == null) return null;

        return new ExerciseDetails(exercise);
    }

    public async Task<Exercise> CreateAsync(Exercise exercise)
    {
        exercise.Timestamp = DateTime.UtcNow;
        exercise.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<Exercise>("usp_CreateExercise", exercise,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Exercise> UpdateAsync(Exercise exercise)
    {
        exercise.Timestamp = DateTime.UtcNow;
        exercise.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<Exercise>("usp_UpdateExercise", exercise,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = context.CreateConnection();
        var affectedRows = await connection.ExecuteAsync("usp_DeleteExercise", new { Id = id },
            commandType: CommandType.StoredProcedure);
        return affectedRows > 0;
    }
}