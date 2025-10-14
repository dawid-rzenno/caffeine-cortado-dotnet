-- =============================================
-- Object: usp_DeleteMilestone  (SQL_STORED_PROCEDURE)
-- Generated: 2025-10-14 16:14:02
-- =============================================
CREATE PROCEDURE usp_DeleteMilestone
(@Id INT, @UserId INT)
AS
BEGIN
    DELETE FROM Milestone
    WHERE Id = @Id AND UserId = @UserId
END
