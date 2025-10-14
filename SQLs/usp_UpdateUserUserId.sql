-- =============================================
-- Object: usp_UpdateUserUserId  (SQL_STORED_PROCEDURE)
-- Generated: 2025-10-14 16:14:02
-- =============================================
CREATE PROCEDURE usp_UpdateUserUserId
(@Id INT, @UserId INT)
AS
BEGIN
    UPDATE [User] SET UserId = @UserId
    OUTPUT INSERTED.*
    WHERE Id = @Id
END
