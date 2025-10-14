-- =============================================
-- Object: usp_AssignIngredientToMeal  (SQL_STORED_PROCEDURE)
-- Generated: 2025-10-14 16:14:02
-- =============================================
CREATE procedure usp_AssignIngredientToMeal (
    @MealId INT, @IngredientId INT, @UserId INT, @Timestamp DATETIME
) as
begin
    INSERT INTO MealIngredient (MealId, IngredientId, UserId, Timestamp)
    OUTPUT INSERTED.*
    VALUES (@MealId, @IngredientId, @UserId, @Timestamp)
end
