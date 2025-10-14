-- =============================================
-- Object: ufn_GetNutrientTypes  (SQL_INLINE_TABLE_VALUED_FUNCTION)
-- Generated: 2025-10-14 16:14:02
-- =============================================
CREATE FUNCTION ufn_GetNutrientTypes(
    @UserId INT,
    @GlobalSearch BIT,
    @Term NVARCHAR(100),
    @Page INT,
    @Size INT,
    @SortBy NVARCHAR(64),
    @Sort NVARCHAR(4)
)
    RETURNS TABLE
        AS
        RETURN(SELECT NutrientType.Id         AS NutrientTypeId,
                      NutrientType.Name       AS NutrientTypeName,
                      NutrientType.UserId     AS NutrientTypeUserId,
                      NutrientType.Timestamp  AS NutrientTypeTimestamp,
                      NutrientType.MassUnitId AS NutrientTypeMassUnitId,
                      MassUnit.Id             AS MassUnitId,
                      MassUnit.Name           AS MassUnitName,
                      MassUnit.UserId         AS MassUnitUserId,
                      MassUnit.Timestamp      AS MassUnitTimestamp,
                      MassUnit.IsPublic       AS MassUnitIsPublic,
                      COUNT(*) OVER ()        AS TotalCount
               FROM NutrientType
                        LEFT JOIN MassUnit ON NutrientType.MassUnitId = MassUnit.Id
               WHERE NutrientType.Name LIKE '%' + @Term + '%'
                 AND (
                   (@GlobalSearch = 0 AND NutrientType.UserId = @UserId)
                       OR (@GlobalSearch = 1 AND (NutrientType.UserId = @UserId OR NutrientType.IsPublic = 1))
                   )

               ORDER BY CASE WHEN @SortBy = 'Id' AND @Sort = 'ASC' THEN NutrientType.Id END ASC,
                        CASE WHEN @SortBy = 'Id' AND @Sort = 'DESC' THEN NutrientType.Id END DESC,
                        CASE WHEN @SortBy = 'Name' AND @Sort = 'ASC' THEN NutrientType.Name END ASC,
                        CASE WHEN @SortBy = 'Name' AND @Sort = 'DESC' THEN NutrientType.Name END DESC,
                        CASE WHEN @SortBy = 'Timestamp' AND @Sort = 'ASC' THEN NutrientType.Timestamp END ASC,
                        CASE WHEN @SortBy = 'Timestamp' AND @Sort = 'DESC' THEN NutrientType.Timestamp END DESC
               OFFSET (@Page - 1) * @Size ROWS FETCH NEXT @Size ROWS ONLY)
