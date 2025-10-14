-- =============================================
-- Object: ufn_GetNutrientType  (SQL_INLINE_TABLE_VALUED_FUNCTION)
-- Generated: 2025-10-14 16:14:02
-- =============================================
CREATE FUNCTION dbo.ufn_GetNutrientType(
    @Id INT,
    @UserId INT
)
    RETURNS TABLE
        AS
        RETURN(SELECT NutrientType.Id        as NutrientTypeId,
                      NutrientType.Name      as NutrientTypeName,
                      NutrientType.UserId    as NutrientTypeUserId,
                      NutrientType.Timestamp as NutrientTypeTimestamp,
                      NutrientType.MassUnitId as NutrientTypeMassUnitId,
                      MassUnit.Id   as MassUnitId,
                      MassUnit.Name as MassUnitName,
                      MassUnit.UserId as MassUnitUserId,
                      MassUnit.Timestamp as MassUnitTimestamp,
                      MassUnit.IsPublic as MassUnitIsPublic
               FROM NutrientType
                        LEFT JOIN MassUnit ON NutrientType.MassUnitId = MassUnit.Id
               WHERE NutrientType.Id = @Id
                 AND (NutrientType.UserId = @UserId OR NutrientType.IsPublic = 1))
