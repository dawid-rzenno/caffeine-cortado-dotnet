-- =============================================
-- Object: usp_UpdateUser  (SQL_STORED_PROCEDURE)
-- Generated: 2025-10-14 16:14:02
-- =============================================
CREATE PROCEDURE usp_UpdateUser
(@Id INT, @Password VARCHAR(64), @UserRoleId INT, @Timestamp DATETIME, @UserId INT)
AS
BEGIN
    UPDATE [User] SET Password = @Password, UserRoleId = @UserRoleId, Timestamp = @Timestamp, UserId = @UserId
    OUTPUT INSERTED.*
    WHERE Id = @Id
END
