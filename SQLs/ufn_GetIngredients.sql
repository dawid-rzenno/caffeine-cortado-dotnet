-- =============================================
-- Object: ufn_GetIngredients  (SQL_INLINE_TABLE_VALUED_FUNCTION)
-- Generated: 2025-10-14 16:14:02
-- =============================================
CREATE FUNCTION ufn_GetIngredients(
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
        RETURN(SELECT Id,
                      Name,
                      IsPublic,
                      Timestamp,
                      UserId,
                      COUNT(*) OVER () AS TotalCount
               FROM Ingredient
               WHERE Name LIKE '%' + @Term + '%'
                 AND (
                   (@GlobalSearch = 0 AND UserId = @UserId)
                       OR (@GlobalSearch = 1 AND (UserId = @UserId OR IsPublic = 1))
                   )

               ORDER BY CASE WHEN @SortBy = 'Id' AND @Sort = 'ASC' THEN Id END ASC,
                        CASE WHEN @SortBy = 'Id' AND @Sort = 'DESC' THEN Id END DESC,
                        CASE WHEN @SortBy = 'Name' AND @Sort = 'ASC' THEN Name END ASC,
                        CASE WHEN @SortBy = 'Name' AND @Sort = 'DESC' THEN Name END DESC,
                        CASE WHEN @SortBy = 'Timestamp' AND @Sort = 'ASC' THEN Timestamp END ASC,
                        CASE WHEN @SortBy = 'Timestamp' AND @Sort = 'DESC' THEN Timestamp END DESC
               OFFSET (@Page - 1) * @Size ROWS FETCH NEXT @Size ROWS ONLY)
