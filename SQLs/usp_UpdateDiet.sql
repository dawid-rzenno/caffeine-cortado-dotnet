-- =============================================
-- Object: usp_UpdateDiet  (SQL_STORED_PROCEDURE)
-- Generated: 2025-10-14 16:14:02
-- =============================================
CREATE PROCEDURE usp_UpdateDiet
(@Id INT, @Name VARCHAR(255), @Timestamp DATETIME, @UserId INT)
AS
BEGIN
    UPDATE Diet SET Name = @Name, Timestamp = @Timestamp, UserId = @UserId
    OUTPUT INSERTED.*
    WHERE Id = @Id AND UserId = @UserId
END
