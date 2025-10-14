-- =============================================
-- Object: usp_UnassignMealFromDiet  (SQL_STORED_PROCEDURE)
-- Generated: 2025-10-14 16:14:02
-- =============================================
create procedure usp_UnassignMealFromDiet (
    @Id INT, @UserId INT
) as
begin
    DELETE FROM DietMeal
    WHERE Id = @Id and UserId = @UserId
end
