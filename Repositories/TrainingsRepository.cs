using System.Data;
using cortado.DTOs;
using cortado.Models;
using cortado.Repositories.Interfaces;
using cortado.Services;
using Dapper;

namespace cortado.Repositories;

public interface ITrainingsRepository : ICrudRepository<Training, TrainingDetails>
{
}

public class TrainingsRepository(DapperContext context, ICurrentUserService currentUserService) : ITrainingsRepository
{
    public async Task<IEnumerable<Training>> GetAllAsync(
        string sort,
        string sortBy,
        int size,
        int page,
        string term,
        bool globalSearch
    )
    {
        using var connection = context.CreateConnection();
        return await connection.QueryAsync<Training>(
            "ufn_GetTrainings",
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
            commandType: CommandType.Text
        );
    }

    public async Task<TrainingDetails?> GetByIdAsync(int id)
    {
        using var connection = context.CreateConnection();

        var trainingDetailsDict = new Dictionary<int, TrainingDetails>();

        IEnumerable<TrainingDetails> dietDetails =
            await connection.QueryAsync<TrainingDetails, TrainingExercise, Exercise, TrainingDetails>(
                "ufn_GetTraining",
                (trainingDetails, _, exercise) =>
                {
                    if (!trainingDetailsDict.TryGetValue(trainingDetails.Id, out var currentTrainingDetails))
                    {
                        currentTrainingDetails = trainingDetails;
                        currentTrainingDetails.Exercises = new List<Exercise>();
                        trainingDetailsDict.Add(currentTrainingDetails.Id, currentTrainingDetails);
                    }

                    currentTrainingDetails.Exercises.Add(exercise);

                    return currentTrainingDetails;
                },
                new { Id = id, UserId = currentUserService.GetUserId() },
                splitOn: "TrainingExerciseId, ExerciseId",
                commandType: CommandType.Text
            );

        return dietDetails.FirstOrDefault();
    }

    public async Task<Training> CreateAsync(Training training)
    {
        training.Timestamp = DateTime.UtcNow;
        training.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<Training>("usp_CreateTraining", training,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Training> UpdateAsync(Training training)
    {
        training.Timestamp = DateTime.UtcNow;
        training.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<Training>("usp_UpdateTraining", training,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = context.CreateConnection();
        var affectedRows = await connection.ExecuteAsync("usp_DeleteTraining", new { Id = id },
            commandType: CommandType.StoredProcedure);
        return affectedRows > 0;
    }
}