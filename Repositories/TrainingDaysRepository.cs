using cortado.DTOs;
using cortado.Models;
using cortado.Services;
using Dapper;

namespace cortado.Repositories;

public interface ITrainingDaysRepository : ICrudRepository<TrainingDay, TrainingDayDetails>
{
}

public class TrainingDaysRepository(DapperContext context, ICurrentUserService currentUserService) : ITrainingDaysRepository
{
    public async Task<IEnumerable<TrainingDay>> GetAllAsync()
    {
        var query = "SELECT * FROM TrainingDays WHERE UserId = @UserId";

        using var connection = context.CreateConnection();
        return await connection.QueryAsync<TrainingDay>(query, new { UserId = currentUserService.GetUserId() });
    }

    public async Task<TrainingDayDetails?> GetByIdAsync(int id)
    {
        var trainingQuery = """
                        SELECT * FROM TrainingDays 
                        WHERE Id = @Id AND UserId = @UserId
                    """;
        
        using var connection = context.CreateConnection();
        
        TrainingDay? training = await connection.QueryFirstOrDefaultAsync<TrainingDay>(trainingQuery, new { Id = id, UserId = currentUserService.GetUserId() });

        if (training == null) return null;
        
        return new TrainingDayDetails(training, new List<Exercise>());
    }

    public async Task<TrainingDay> CreateAsync(TrainingDay training)
    {
        var createTrainingDayQuery = """
                                  INSERT INTO TrainingDays (Name, Timestamp, UserId) 
                                  OUTPUT INSERTED.*
                                  VALUES (@Name, @Timestamp, @UserId)
                              """;

        training.Timestamp = DateTime.UtcNow;
        training.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<TrainingDay>(createTrainingDayQuery, training);
    }

    public async Task<TrainingDay> UpdateAsync(TrainingDay training)
    {
        var query = """
                        UPDATE TrainingDays SET Name = @Name, Timestamp = @Timestamp, UserId = @UserId
                        OUTPUT INSERTED.*
                        WHERE Id = @Id AND UserId = @UserId
                    """;

        training.Timestamp = DateTime.UtcNow;
        training.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<TrainingDay>(query, training);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var query = """
                        DELETE FROM TrainingDays 
                        WHERE Id = @Id
                    """;

        using var connection = context.CreateConnection();
        var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
        return affectedRows > 0;
    }
}