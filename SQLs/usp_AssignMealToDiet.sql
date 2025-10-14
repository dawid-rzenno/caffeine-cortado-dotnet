-- =============================================
-- Object: usp_AssignMealToDiet  (SQL_STORED_PROCEDURE)
-- Generated: 2025-10-14 16:14:02
-- =============================================
CREATE procedure usp_AssignMealToDiet (
    @DietId INT, @MealId INT, @MealDayIndex INT, @MealIndex INT, @UserId INT, @Timestamp DATETIME
) as
begin
    INSERT INTO DietMeal (DietId, MealId, MealDayIndex, MealIndex, UserId, Timestamp)
    OUTPUT INSERTED.*
    VALUES (@DietId, @MealId, @MealDayIndex, @MealIndex, @UserId, @Timestamp)
end
