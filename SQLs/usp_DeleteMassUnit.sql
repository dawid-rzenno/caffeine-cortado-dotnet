-- =============================================
-- Object: usp_DeleteMassUnit  (SQL_STORED_PROCEDURE)
-- Generated: 2025-10-14 16:14:02
-- =============================================
CREATE PROCEDURE usp_DeleteMassUnit
(@Id INT, @UserId INT)
AS
BEGIN
    DELETE FROM MassUnit
    WHERE Id = @Id AND UserId = @UserId
END
