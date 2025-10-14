-- =============================================
-- Object: usp_CreateNutrient  (SQL_STORED_PROCEDURE)
-- Generated: 2025-10-14 16:14:02
-- =============================================
CREATE PROCEDURE usp_CreateNutrient
(@NutrientTypeId INT, @Amount DECIMAL, @Timestamp DATETIME, @UserId int)
AS
BEGIN
    INSERT INTO Nutrient (NutrientTypeId, Amount, Timestamp, UserId)
    OUTPUT INSERTED.*
    VALUES (@NutrientTypeId, @Amount, @Timestamp, @UserId)
END
