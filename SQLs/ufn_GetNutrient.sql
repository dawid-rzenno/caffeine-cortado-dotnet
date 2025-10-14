-- =============================================
-- Object: ufn_GetNutrient  (SQL_INLINE_TABLE_VALUED_FUNCTION)
-- Generated: 2025-10-14 16:14:02
-- =============================================
CREATE FUNCTION dbo.ufn_GetNutrient(
    @Id INT,
    @UserId INT
)
    RETURNS TABLE
        AS
        RETURN(SELECT Nutrient.Id,
                      Nutrient.Amount,
                      Nutrient.NutrientTypeId as NutrientNutrientTypeId,
                      Nutrient.UserId,
                      Nutrient.IsPublic,
                      Nutrient.Timestamp,
                      NutrientType.Id        as NutrientTypeId,
                      NutrientType.Name      as NutrientTypeName,
                      NutrientType.UserId    as NutrientTypeUserId,
                      NutrientType.Timestamp as NutrientTypeTimestamp,
                      NutrientType.MassUnitId as NutrientTypeMassUnitId,
                      MassUnit.Id   as MassUnitId,
                      MassUnit.Name as MassUnitName,
                      MassUnit.UserId as MassUnitUserId,
                      MassUnit.Timestamp as MassUnitTimestamp,
                      MassUnit.IsPublic as MassUnitIsPublic
               FROM Nutrient
                        LEFT JOIN NutrientType ON Nutrient.NutrientTypeId = NutrientType.Id
                        LEFT JOIN MassUnit ON NutrientType.MassUnitId = MassUnit.Id
               WHERE Nutrient.Id = @Id
                 AND (Nutrient.UserId = @UserId OR Nutrient.IsPublic = 1))
