-- =============================================
-- Object: usp_DeleteMeal  (SQL_STORED_PROCEDURE)
-- Generated: 2025-10-14 16:14:02
-- =============================================
CREATE PROCEDURE usp_DeleteMeal
(@Id INT, @UserId INT)
AS
BEGIN
    DELETE FROM Meal
    WHERE Id = @Id AND UserId = @UserId
END
