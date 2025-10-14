-- =============================================
-- Object: ufn_GetMassUnit  (SQL_INLINE_TABLE_VALUED_FUNCTION)
-- Generated: 2025-10-14 16:14:02
-- =============================================
CREATE FUNCTION dbo.ufn_GetMassUnit(
    @Id INT,
    @UserId INT
)
    RETURNS TABLE
        AS
        RETURN(SELECT MassUnit.*
               FROM MassUnit
               WHERE MassUnit.Id = @Id
                 AND (MassUnit.UserId = @UserId OR MassUnit.IsPublic = 1))
