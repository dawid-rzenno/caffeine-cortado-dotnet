-- =============================================
-- Object: usp_DeleteNutrient  (SQL_STORED_PROCEDURE)
-- Generated: 2025-10-14 16:14:02
-- =============================================
CREATE PROCEDURE usp_DeleteNutrient
(@Id INT, @UserId INT)
AS
BEGIN
    DELETE FROM Nutrient
    WHERE Id = @Id AND UserId = @UserId
END
