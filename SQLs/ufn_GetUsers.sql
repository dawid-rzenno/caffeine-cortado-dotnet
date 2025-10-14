-- =============================================
-- Object: ufn_GetUsers  (SQL_INLINE_TABLE_VALUED_FUNCTION)
-- Generated: 2025-10-14 16:14:02
-- =============================================
CREATE FUNCTION ufn_GetUsers(
    @Term NVARCHAR(100),
    @Page INT,
    @Size INT,
    @SortBy NVARCHAR(64),
    @Sort NVARCHAR(4)
)
    RETURNS TABLE
        AS
        RETURN(SELECT Id,
                      Username,
                      Timestamp,
                      COUNT(*) OVER () AS TotalCount
               FROM [User]
               WHERE Username LIKE '%' + @Term + '%'
               ORDER BY CASE WHEN @SortBy = 'Id' AND @Sort = 'ASC' THEN Id END ASC,
                        CASE WHEN @SortBy = 'Id' AND @Sort = 'DESC' THEN Id END DESC,
                        CASE WHEN @SortBy = 'Username' AND @Sort = 'ASC' THEN Username END ASC,
                        CASE WHEN @SortBy = 'Username' AND @Sort = 'DESC' THEN Username END DESC,
                        CASE WHEN @SortBy = 'Timestamp' AND @Sort = 'ASC' THEN Timestamp END ASC,
                        CASE WHEN @SortBy = 'Timestamp' AND @Sort = 'DESC' THEN Timestamp END DESC
               OFFSET (@Page - 1) * @Size ROWS FETCH NEXT @Size ROWS ONLY)
