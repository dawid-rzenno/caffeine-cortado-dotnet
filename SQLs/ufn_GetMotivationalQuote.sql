-- =============================================
-- Object: ufn_GetMotivationalQuote  (SQL_INLINE_TABLE_VALUED_FUNCTION)
-- Generated: 2025-10-14 16:14:02
-- =============================================
CREATE FUNCTION dbo.ufn_GetMotivationalQuote(
    @Id INT,
    @UserId INT
)
    RETURNS TABLE
        AS
        RETURN(SELECT MotivationalQuote.*
               FROM MotivationalQuote
               WHERE MotivationalQuote.Id = @Id
                 AND (MotivationalQuote.UserId = @UserId OR MotivationalQuote.IsPublic = 1))
