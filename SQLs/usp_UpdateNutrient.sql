-- =============================================
-- Object: usp_UpdateNutrient  (SQL_STORED_PROCEDURE)
-- Generated: 2025-10-14 16:14:02
-- =============================================
CREATE PROCEDURE usp_UpdateNutrient
(@Id INT, @NutrientTypeId INT, @Amount DECIMAL, @Timestamp DATETIME, @UserId int)
AS
BEGIN
    UPDATE Nutrient SET NutrientTypeId = @NutrientTypeId, Amount = @Amount, Timestamp = @Timestamp, UserId = @UserId
    OUTPUT INSERTED.*
    WHERE Id = @Id AND UserId = @UserId
END
