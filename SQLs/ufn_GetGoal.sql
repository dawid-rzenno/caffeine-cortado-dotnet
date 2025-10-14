-- =============================================
-- Object: ufn_GetGoal  (SQL_INLINE_TABLE_VALUED_FUNCTION)
-- Generated: 2025-10-14 16:14:02
-- =============================================
CREATE FUNCTION dbo.ufn_GetGoal(
    @Id INT,
    @UserId INT
)
    RETURNS TABLE
        AS
        RETURN(SELECT Goal.*,
                      Milestone.Id                     as MilestoneId,
                      Milestone.Name                   as MilestoneName,
                      Milestone.UserId                 as MilestoneUserId,
                      Milestone.Timestamp              as MilestoneTimestamp
               FROM Goal
                        LEFT JOIN Milestone ON Goal.Id = Milestone.GoalId
               WHERE Goal.Id = @Id
                 AND (Goal.UserId = @UserId OR Goal.IsPublic = 1))
