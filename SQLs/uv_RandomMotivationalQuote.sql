-- =============================================
-- Object: uv_RandomMotivationalQuote  (VIEW)
-- Generated: 2025-10-14 16:14:02
-- =============================================
CREATE VIEW dbo.uv_RandomMotivationalQuote
AS
SELECT TOP 1 *
FROM MotivationalQuote
ORDER BY NEWID()
