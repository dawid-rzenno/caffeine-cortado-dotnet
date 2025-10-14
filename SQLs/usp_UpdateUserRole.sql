-- =============================================
-- Object: usp_UpdateUserRole  (SQL_STORED_PROCEDURE)
-- Generated: 2025-10-14 16:14:02
-- =============================================
CREATE PROCEDURE usp_UpdateUserRole
(@Id INT, @Name VARCHAR(64))
AS
BEGIN
    UPDATE UserRole SET Name = @Name
    OUTPUT INSERTED.*
    WHERE Id = @Id
END
