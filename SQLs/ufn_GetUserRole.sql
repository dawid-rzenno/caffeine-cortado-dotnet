-- =============================================
-- Object: ufn_GetUserRole  (SQL_INLINE_TABLE_VALUED_FUNCTION)
-- Generated: 2025-10-14 16:14:02
-- =============================================
CREATE FUNCTION dbo.ufn_GetUserRole(
    @Id INT
)
    RETURNS TABLE
        AS
        RETURN(SELECT UserRole.*
               FROM UserRole
               WHERE UserRole.Id = @Id)
