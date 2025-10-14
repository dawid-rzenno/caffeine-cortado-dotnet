-- =============================================
-- Object: usp_CreateNutrientType  (SQL_STORED_PROCEDURE)
-- Generated: 2025-10-14 16:14:02
-- =============================================
CREATE PROCEDURE usp_CreateNutrientType
(@Name VARCHAR(255), @MassUnitId INT, @Timestamp DATETIME, @UserId int)
AS
BEGIN
    INSERT INTO NutrientType (Name, MassUnitId, Timestamp, UserId)
    OUTPUT INSERTED.*
    VALUES (@Name, @MassUnitId, @Timestamp, @UserId)
END
