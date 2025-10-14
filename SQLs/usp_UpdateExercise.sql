-- =============================================
-- Object: usp_UpdateExercise  (SQL_STORED_PROCEDURE)
-- Generated: 2025-10-14 16:14:02
-- =============================================
CREATE PROCEDURE usp_UpdateExercise
(@Id INT, @Name VARCHAR(255), @Timestamp DATETIME, @UserId INT)
AS
BEGIN
    UPDATE Exercise SET Name = @Name, Timestamp = @Timestamp, UserId = @UserId
    OUTPUT INSERTED.*
    WHERE Id = @Id AND UserId = @UserId
END
