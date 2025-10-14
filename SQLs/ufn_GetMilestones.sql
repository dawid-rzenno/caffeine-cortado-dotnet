-- =============================================
-- Object: ufn_GetMilestones  (SQL_INLINE_TABLE_VALUED_FUNCTION)
-- Generated: 2025-10-14 16:14:02
-- =============================================
CREATE FUNCTION dbo.ufn_GetMilestones(
    @Id INT,
    @UserId INT
)
    RETURNS TABLE
        AS
        RETURN(SELECT Milestone.*
               FROM Milestone
               WHERE Milestone.Id = @Id
                 AND (Milestone.UserId = @UserId OR Milestone.IsPublic = 1))
