-- =============================================
-- Object: usp_DeleteUser  (SQL_STORED_PROCEDURE)
-- Generated: 2025-10-14 16:14:02
-- =============================================
CREATE PROCEDURE usp_DeleteUser
(@Id INT)
AS
BEGIN
    DELETE FROM [User]
    WHERE Id = @Id
END
