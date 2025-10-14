-- =============================================
-- Object: ufn_GetUserRoles  (SQL_INLINE_TABLE_VALUED_FUNCTION)
-- Generated: 2025-10-14 16:14:02
-- =============================================
CREATE FUNCTION ufn_GetUserRoles(
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
                      AccessLevel,
                      COUNT(*) OVER () AS TotalCount
               FROM UserRole
               WHERE Name LIKE '%' + @Term + '%'
               ORDER BY CASE WHEN @SortBy = 'Id' AND @Sort = 'ASC' THEN Id END ASC,
                        CASE WHEN @SortBy = 'Id' AND @Sort = 'DESC' THEN Id END DESC,
                        CASE WHEN @SortBy = 'Name' AND @Sort = 'ASC' THEN Name END ASC,
                        CASE WHEN @SortBy = 'Name' AND @Sort = 'DESC' THEN Name END DESC,
                        CASE WHEN @SortBy = 'AccessLevel' AND @Sort = 'ASC' THEN AccessLevel END ASC,
                        CASE WHEN @SortBy = 'AccessLevel' AND @Sort = 'DESC' THEN AccessLevel END DESC
               OFFSET (@Page - 1) * @Size ROWS FETCH NEXT @Size ROWS ONLY)
