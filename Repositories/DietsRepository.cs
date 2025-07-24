using cortado.DTOs;
using cortado.Models;
using cortado.Services;
using Dapper;

namespace cortado.Repositories;

public interface IDietsRepository : ICrudRepository<Diet, DietDetails>
{
}

public class DietsRepository(DapperContext context, ICurrentUserService currentUserService) : IDietsRepository
{
    public async Task<IEnumerable<Diet>> GetAllAsync()
    {
        var query = "SELECT * FROM Diets WHERE UserId = @UserId";

        using var connection = context.CreateConnection();
        return await connection.QueryAsync<Diet>(query, new { UserId = currentUserService.GetUserId() });
    }

    public async Task<DietDetails?> GetByIdAsync(int id)
    {
        var dietQuery = """
                        SELECT * FROM Diets 
                        WHERE Id = @Id AND UserId = @UserId
                    """;
        
        using var connection = context.CreateConnection();
        
        Diet? diet = await connection.QueryFirstOrDefaultAsync<Diet>(dietQuery, new { Id = id, UserId = currentUserService.GetUserId() });

        if (diet == null) return null;
		
        return new DietDetails(diet, new List<Meal>());
    }

    public async Task<Diet> CreateAsync(Diet diet)
    {
        var createDietQuery = """
                                  INSERT INTO Diets (Name, Timestamp, UserId) 
                                  OUTPUT INSERTED.*
                                  VALUES (@Name, @Timestamp, @UserId)
                              """;

        diet.Timestamp = DateTime.UtcNow;
        diet.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<Diet>(createDietQuery, diet);
    }

    public async Task<Diet> UpdateAsync(Diet diet)
    {
        var query = """
                        UPDATE Diets SET Name = @Name, Timestamp = @Timestamp, UserId = @UserId
                        OUTPUT INSERTED.*
                        WHERE Id = @Id AND UserId = @UserId
                    """;

        diet.Timestamp = DateTime.UtcNow;
        diet.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<Diet>(query, diet);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var query = """
                        DELETE FROM Diets 
                        WHERE Id = @Id
                    """;

        using var connection = context.CreateConnection();
        var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
        return affectedRows > 0;
    }
}