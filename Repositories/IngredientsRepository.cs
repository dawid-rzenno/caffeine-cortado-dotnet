using cortado.DTOs;
using cortado.Models;
using cortado.Services;
using Dapper;

namespace cortado.Repositories;

public interface IIngredientsRepository : ICrudRepository<Ingredient, IngredientDetails>
{
}

public class IngredientsRepository(DapperContext context, ICurrentUserService currentUserService) : IIngredientsRepository
{
    public async Task<IEnumerable<Ingredient>> GetAllAsync()
    {
        var query = "SELECT * FROM Ingredients WHERE UserId = @UserId";

        using var connection = context.CreateConnection();
        return await connection.QueryAsync<Ingredient>(query, new { UserId = currentUserService.GetUserId() });
    }

    public async Task<IngredientDetails?> GetByIdAsync(int id)
    {
        var ingredientQuery = """
                        SELECT * FROM Ingredients 
                        WHERE Id = @Id AND UserId = @UserId
                    """;
        
        using var connection = context.CreateConnection();
        
        Ingredient? ingredient = await connection.QueryFirstOrDefaultAsync<Ingredient>(ingredientQuery, new { Id = id, UserId = currentUserService.GetUserId() });

        if (ingredient == null) return null;
		
        return new IngredientDetails(ingredient, new List<Nutrient>());
    }

    public async Task<Ingredient> CreateAsync(Ingredient ingredient)
    {
        var createIngredientQuery = """
                                  INSERT INTO Ingredients (Name, Timestamp, UserId) 
                                  OUTPUT INSERTED.*
                                  VALUES (@Name, @Timestamp, @UserId)
                              """;

        ingredient.Timestamp = DateTime.UtcNow;
        ingredient.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<Ingredient>(createIngredientQuery, ingredient);
    }

    public async Task<Ingredient> UpdateAsync(Ingredient ingredient)
    {
        var query = """
                        UPDATE Ingredients SET Name = @Name, Timestamp = @Timestamp, UserId = @UserId
                        OUTPUT INSERTED.*
                        WHERE Id = @Id AND UserId = @UserId
                    """;

        ingredient.Timestamp = DateTime.UtcNow;
        ingredient.UserId = currentUserService.GetUserId();

        using var connection = context.CreateConnection();

        return await connection.QuerySingleAsync<Ingredient>(query, ingredient);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var query = """
                        DELETE FROM Ingredients 
                        WHERE Id = @Id
                    """;

        using var connection = context.CreateConnection();
        var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
        return affectedRows > 0;
    }
}