-- =============================================
-- Object: ufn_GetDiet  (SQL_INLINE_TABLE_VALUED_FUNCTION)
-- Generated: 2025-10-14 16:14:02
-- =============================================
CREATE FUNCTION dbo.ufn_GetDiet(
    @Id INT,
    @UserId INT
)
    RETURNS TABLE
        AS
        RETURN(SELECT Diet.*,
                      DietMeal.Id                 as DietMealId,
                      DietMeal.DietId             as DietMealDietId,
                      DietMeal.MealId             as DietMealMealId,
                      DietMeal.UserId             as DietMealUserId,
                      DietMeal.Timestamp          as DietMealTimestamp,
                      Meal.Id                     as MealId,
                      Meal.Name                   as MealName,
                      Meal.UserId                 as MealUserId,
                      Meal.Timestamp              as MealTimestamp
               FROM Diet
                        LEFT JOIN DietMeal ON Diet.Id = DietMeal.DietId
                        LEFT JOIN Meal ON DietMeal.MealId = Meal.Id
               WHERE Diet.Id = @Id
                 AND (Diet.UserId = @UserId OR Diet.IsPublic = 1))
