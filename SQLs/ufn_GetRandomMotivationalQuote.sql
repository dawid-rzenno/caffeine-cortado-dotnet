-- =============================================
-- Object: ufn_GetRandomMotivationalQuote  (SQL_INLINE_TABLE_VALUED_FUNCTION)
-- Generated: 2025-10-14 16:14:02
-- =============================================
CREATE FUNCTION ufn_GetRandomMotivationalQuote()
    RETURNS TABLE
        AS
        RETURN(SELECT * FROM uv_RandomMotivationalQuote)
