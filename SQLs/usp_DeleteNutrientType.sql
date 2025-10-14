-- =============================================
-- Object: usp_DeleteNutrientType  (SQL_STORED_PROCEDURE)
-- Generated: 2025-10-14 16:14:02
-- =============================================
CREATE PROCEDURE usp_DeleteNutrientType
(@Id INT, @UserId INT)
AS
BEGIN
    DELETE FROM NutrientType
    WHERE Id = @Id AND UserId = @UserId
END
