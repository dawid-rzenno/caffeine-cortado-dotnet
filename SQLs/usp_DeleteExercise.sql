-- =============================================
-- Object: usp_DeleteExercise  (SQL_STORED_PROCEDURE)
-- Generated: 2025-10-14 16:14:02
-- =============================================
CREATE PROCEDURE usp_DeleteExercise
(@Id INT, @UserId INT)
AS
BEGIN
    DELETE FROM Exercise
    WHERE Id = @Id AND UserId = @UserId
END
