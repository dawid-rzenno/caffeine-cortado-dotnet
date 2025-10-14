-- =============================================
-- Object: usp_UnassignIngredientFromMeal  (SQL_STORED_PROCEDURE)
-- Generated: 2025-10-14 16:14:02
-- =============================================
create procedure usp_UnassignIngredientFromMeal (
    @Id INT, @UserId INT
) as
begin
    DELETE FROM MealIngredient
    WHERE Id = @Id and UserId = @UserId
end
