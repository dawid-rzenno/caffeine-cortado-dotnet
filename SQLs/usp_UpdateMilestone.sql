-- =============================================
-- Object: usp_UpdateMilestone  (SQL_STORED_PROCEDURE)
-- Generated: 2025-10-14 16:14:02
-- =============================================
CREATE PROCEDURE usp_UpdateMilestone
(@Id INT, @Name VARCHAR(255), @GoalId INT, @Timestamp DATETIME, @UserId INT)
AS
BEGIN
    UPDATE Milestone SET Name = @Name, GoalId = @GoalId, Timestamp = @Timestamp, UserId = @UserId
    OUTPUT INSERTED.*
    WHERE Id = @Id AND UserId = @UserId
END
