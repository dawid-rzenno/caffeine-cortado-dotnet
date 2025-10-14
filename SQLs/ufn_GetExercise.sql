-- =============================================
-- Object: ufn_GetExercise  (SQL_INLINE_TABLE_VALUED_FUNCTION)
-- Generated: 2025-10-14 16:14:02
-- =============================================
CREATE FUNCTION dbo.ufn_GetExercise(
    @Id INT,
    @UserId INT
)
    RETURNS TABLE
        AS
        RETURN(SELECT Exercise.*
               FROM Exercise
               WHERE Exercise.Id = @Id
                 AND (Exercise.UserId = @UserId OR Exercise.IsPublic = 1))
