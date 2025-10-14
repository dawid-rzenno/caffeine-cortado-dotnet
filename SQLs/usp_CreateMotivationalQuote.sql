-- =============================================
-- Object: usp_CreateMotivationalQuote  (SQL_STORED_PROCEDURE)
-- Generated: 2025-10-14 16:14:02
-- =============================================
CREATE PROCEDURE usp_CreateMotivationalQuote
(@Quote VARCHAR(255), @Author VARCHAR(64), @Timestamp DATETIME, @UserId int)
AS
BEGIN
    INSERT INTO MotivationalQuote (Quote, Author, Timestamp, UserId)
    OUTPUT INSERTED.*
    VALUES (@Quote, @Author, @Timestamp, @UserId)
END
