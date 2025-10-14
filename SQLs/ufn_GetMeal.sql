-- =============================================
-- Object: ufn_GetMeal  (SQL_INLINE_TABLE_VALUED_FUNCTION)
-- Generated: 2025-10-14 16:14:02
-- =============================================
CREATE FUNCTION dbo.ufn_GetMeal(
    @Id INT,
    @UserId INT
)
    RETURNS TABLE
        AS
        RETURN(SELECT Meal.*,
                      MealIngredient.Id                 as MealIngredientId,
                      MealIngredient.IngredientId       as MealIngredientIngredientId,
                      MealIngredient.MealId             as MealIngredientMealId,
                      MealIngredient.UserId             as MealIngredientUserId,
                      MealIngredient.Timestamp          as MealIngredientTimestamp,
                      Ingredient.Id                     as IngredientId,
                      Ingredient.Name                   as IngredientName,
                      Ingredient.UserId                 as IngredientUserId,
                      Ingredient.Timestamp              as IngredientTimestamp
               FROM Meal
                        LEFT JOIN MealIngredient ON Meal.Id = MealIngredient.MealId
                        LEFT JOIN Ingredient ON MealIngredient.IngredientId = Ingredient.Id
               WHERE Meal.Id = @Id AND Meal.UserId = @UserId
                 AND (Meal.UserId = @UserId OR Meal.IsPublic = 1))
