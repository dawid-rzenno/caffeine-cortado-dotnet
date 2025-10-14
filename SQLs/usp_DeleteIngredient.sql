-- =============================================
-- Object: usp_DeleteIngredient  (SQL_STORED_PROCEDURE)
-- Generated: 2025-10-14 16:14:02
-- =============================================
CREATE PROCEDURE usp_DeleteIngredient
(@Id INT, @UserId INT)
AS
BEGIN
    DELETE FROM Ingredient
    WHERE Id = @Id AND UserId = @UserId
END
