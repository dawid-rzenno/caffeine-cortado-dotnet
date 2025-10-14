-- =============================================
-- Object: usp_CreateMeal  (SQL_STORED_PROCEDURE)
-- Generated: 2025-10-14 16:14:02
-- =============================================
CREATE PROCEDURE usp_CreateMeal
(@Name VARCHAR(255), @Timestamp DATETIME, @UserId int)
AS
BEGIN
    INSERT INTO Meal (Name, Timestamp, UserId)
    OUTPUT INSERTED.*
    VALUES (@Name, @Timestamp, @UserId)
END
