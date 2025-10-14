-- =============================================
-- Object: usp_UpdateUserPassword  (SQL_STORED_PROCEDURE)
-- Generated: 2025-10-14 16:14:02
-- =============================================
CREATE PROCEDURE usp_UpdateUserPassword
(@Id INT, @Password VARCHAR(64), @Timestamp DATETIME)
AS
BEGIN
    UPDATE [User] SET Password = @Password, Timestamp = @Timestamp
    OUTPUT INSERTED.*
    WHERE Id = @Id
END
