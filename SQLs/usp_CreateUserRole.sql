-- =============================================
-- Object: usp_CreateUserRole  (SQL_STORED_PROCEDURE)
-- Generated: 2025-10-14 16:14:02
-- =============================================
CREATE PROCEDURE usp_CreateUserRole
(@Name VARCHAR(64), @AccessLevel INT)
AS
BEGIN
    INSERT INTO UserRole (Name, AccessLevel)
    OUTPUT INSERTED.*
    VALUES (@Name, @AccessLevel)
END
