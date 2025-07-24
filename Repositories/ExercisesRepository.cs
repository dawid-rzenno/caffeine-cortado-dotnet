using cortado.DTOs;
using cortado.Models;
using cortado.Services;
using Dapper;

namespace cortado.Repositories;

public interface IExercisesRepository : ICrudRepository<Exercise, ExerciseDetails>
{
}

public class ExercisesRepository(DapperContext context, ICurrentUserService currentUserService) : IExercisesRepository
{
    public async Task<IEnumerable<Exercise>> GetAllAsync()
    {
        var query = "SELECT * FROM Exercises WHERE UserId = @UserId";

        using var connection = context.CreateConnection();
        return await connection.QueryAsync<Exercise>(query, new { UserId = currentUserService.GetUserId() });
    }

    public async Task<ExerciseDetails?> GetByIdAsync(int id)
    {
        var exerciseQuery = """
                        SELECT * FROM Exercises 
                        WHERE Id = @Id AND UserId = @UserId
                    """;
        
        using var connection = context.CreateConnection();
        
        Exercise? exercise = await connection.QueryFirstOrDefaultAsync<Exercise>(exerciseQuery, new { Id = id, UserId = currentUserService.GetUserId() });

        if (exercise == null) return null;
        
        return new ExerciseDetails(exercise);
    }

    public async Task<Exercise> CreateAsync(Exercise exercise)
    {
        var createExerciseQuery = """
                                  INSERT INTO Exercises (Name, Timestamp, UserId) 
                                  OUTPUT INSERTED.*
                                  VALUES (@Name, @Timestamp, @UserId)
                              """;

        exercise.Timestamp = DateTime.UtcNow;
        exercise.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<Exercise>(createExerciseQuery, exercise);
    }

    public async Task<Exercise> UpdateAsync(Exercise exercise)
    {
        var query = """
                        UPDATE Exercises SET Name = @Name, Timestamp = @Timestamp, UserId = @UserId
                        OUTPUT INSERTED.*
                        WHERE Id = @Id AND UserId = @UserId
                    """;

        exercise.Timestamp = DateTime.UtcNow;
        exercise.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<Exercise>(query, exercise);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var query = """
                        DELETE FROM Exercises 
                        WHERE Id = @Id
                    """;

        using var connection = context.CreateConnection();
        var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
        return affectedRows > 0;
    }
}