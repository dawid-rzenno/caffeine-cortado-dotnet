-- =============================================
-- Object: usp_DeleteMotivationalQuote  (SQL_STORED_PROCEDURE)
-- Generated: 2025-10-14 16:14:02
-- =============================================
CREATE PROCEDURE usp_DeleteMotivationalQuote
(@Id INT, @UserId INT)
AS
BEGIN
    DELETE FROM MotivationalQuote
    WHERE Id = @Id AND UserId = @UserId
END
