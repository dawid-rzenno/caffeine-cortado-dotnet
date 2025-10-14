-- =============================================
-- Object: usp_UpdateIngredient  (SQL_STORED_PROCEDURE)
-- Generated: 2025-10-14 16:14:02
-- =============================================
CREATE PROCEDURE usp_UpdateIngredient
(@Id INT, @Name VARCHAR(255), @Timestamp DATETIME, @UserId INT)
AS
BEGIN
    UPDATE Ingredient SET Name = @Name, Timestamp = @Timestamp, UserId = @UserId
    OUTPUT INSERTED.*
    WHERE Id = @Id AND UserId = @UserId
END
