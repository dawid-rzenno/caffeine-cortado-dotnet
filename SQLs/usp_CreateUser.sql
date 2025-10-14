-- =============================================
-- Object: usp_CreateUser  (SQL_STORED_PROCEDURE)
-- Generated: 2025-10-14 16:14:02
-- =============================================
CREATE PROCEDURE usp_CreateUser
(@Username VARCHAR(255), @Password VARCHAR(64), @RoleId INT, @Timestamp DATETIME)
AS
BEGIN
    INSERT INTO [User] (Username, Password, UserRoleId, Timestamp)
    OUTPUT INSERTED.*
    VALUES (@Username, @Password, @RoleId, @Timestamp);
    SELECT CAST(SCOPE_IDENTITY() as int)
END
