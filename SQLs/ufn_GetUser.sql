-- =============================================
-- Object: ufn_GetUser  (SQL_INLINE_TABLE_VALUED_FUNCTION)
-- Generated: 2025-10-14 16:14:02
-- =============================================
CREATE FUNCTION dbo.ufn_GetUser(
    @Id INT,
    @Username VARCHAR(64),
    @UserId INT
)
    RETURNS TABLE
        AS
        RETURN(SELECT [User].Id,
                      [User].Password,
                      [User].UserId AS UserUserId,
                      [User].UserRoleId AS UserUserRoleId,
                      [User].Timestamp,
                      [User].UserId,
                      UserRole.Id   AS UserRoleId,
                      UserRole.Name AS UserRoleName,
                      UserRole.AccessLevel AS UserRoleAccessLevel
               FROM [User]
                        LEFT JOIN UserRole ON UserRole.Id = [User].UserRoleId
               WHERE ([User].Id = @Id OR [User].Username = @Username)
                 AND ([User].Id = @UserId))
