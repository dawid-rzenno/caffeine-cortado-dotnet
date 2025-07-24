using cortado.DTOs;
using cortado.Models;
using cortado.Services;
using Dapper;

namespace cortado.Repositories;

public interface ITrainingsRepository : ICrudRepository<Training, TrainingDetails>
{
}

public class TrainingsRepository(DapperContext context, ICurrentUserService currentUserService) : ITrainingsRepository
{
    public async Task<IEnumerable<Training>> GetAllAsync()
    {
        var query = "SELECT * FROM Trainings WHERE UserId = @UserId";

        using var connection = context.CreateConnection();
        return await connection.QueryAsync<Training>(query, new { UserId = currentUserService.GetUserId() });
    }

    public async Task<TrainingDetails?> GetByIdAsync(int id)
    {
        var trainingQuery = """
                        SELECT * FROM Trainings 
                        WHERE Id = @Id AND UserId = @UserId
                    """;
        
        using var connection = context.CreateConnection();
        
        Training? training = await connection.QueryFirstOrDefaultAsync<Training>(trainingQuery, new { Id = id, UserId = currentUserService.GetUserId() });

        if (training == null) return null;
        
        return new TrainingDetails(training, new List<Exercise>());
    }

    public async Task<Training> CreateAsync(Training training)
    {
        var createTrainingQuery = """
                                  INSERT INTO Trainings (Name, Timestamp, UserId) 
                                  OUTPUT INSERTED.*
                                  VALUES (@Name, @Timestamp, @UserId)
                              """;

        training.Timestamp = DateTime.UtcNow;
        training.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<Training>(createTrainingQuery, training);
    }

    public async Task<Training> UpdateAsync(Training training)
    {
        var query = """
                        UPDATE Trainings SET Name = @Name, Timestamp = @Timestamp, UserId = @UserId
                        OUTPUT INSERTED.*
                        WHERE Id = @Id AND UserId = @UserId
                    """;

        training.Timestamp = DateTime.UtcNow;
        training.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<Training>(query, training);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var query = """
                        DELETE FROM Trainings 
                        WHERE Id = @Id
                    """;

        using var connection = context.CreateConnection();
        var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
        return affectedRows > 0;
    }
}