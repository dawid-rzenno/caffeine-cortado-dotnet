-- =============================================
-- Object: usp_CreateMilestone  (SQL_STORED_PROCEDURE)
-- Generated: 2025-10-14 16:14:02
-- =============================================
CREATE PROCEDURE usp_CreateMilestone
(@Name VARCHAR(255), @GoalId INT, @Timestamp DATETIME, @UserId int)
AS
BEGIN
    INSERT INTO Milestone (Name, GoalId, Timestamp, UserId)
    OUTPUT INSERTED.*
    VALUES (@Name, @GoalId, @Timestamp, @UserId)
END
