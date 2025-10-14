-- =============================================
-- Object: usp_AssignNutrientToIngredient  (SQL_STORED_PROCEDURE)
-- Generated: 2025-10-14 16:14:02
-- =============================================
CREATE procedure usp_AssignNutrientToIngredient (
    @IngredientId INT, @NutrientId INT, @UserId INT, @Timestamp DATETIME
) as 
    begin
        INSERT INTO IngredientNutrient (IngredientId, NutrientId, UserId, Timestamp)
        OUTPUT INSERTED.*
        VALUES (@IngredientId, @NutrientId, @UserId, @Timestamp)
    end
