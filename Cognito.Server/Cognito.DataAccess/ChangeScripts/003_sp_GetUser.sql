CREATE OR ALTER PROCEDURE [dbo].[GetUser]
(
	@Email NVARCHAR(128)
)
AS
BEGIN

    DECLARE @UPPER_EMAIL NVARCHAR(128) = UPPER(@email)

    -- USER
	SELECT TOP 1
        [AspNetUsers].*
	FROM [User].[AspNetUsers]
    WHERE [NormalizedEmail] = @UPPER_EMAIL

    -- ROLES
    SELECT
        [AspNetRoles].*
	FROM [User].[AspNetUsers]
    INNER JOIN [User].[AspNetUserRoles]
    ON [AspNetUsers].[Id] = [AspNetUserRoles].[UserId]
    INNER JOIN [User].[AspNetRoles]
    ON [AspNetUserRoles].[RoleId] = [AspNetRoles].[Id]
    WHERE [NormalizedEmail] = @UPPER_EMAIL

    -- DOMAINS
    SELECT [UserDomains].*
	FROM [User].[AspNetUsers]
    INNER JOIN [dbo].[UserDomains]
    ON [AspNetUsers].[Id] = [UserDomains].[UserId]
    INNER JOIN [dbo].[Domains]
    ON [UserDomains].[DomainId] = [Domains].[Id]
    WHERE [NormalizedEmail] = @UPPER_EMAIL

    -- REFRESH TOKENS
    SELECT *
    FROM [User].[RefreshTokens]
    INNER JOIN [User].[AspNetUsers]
    ON [RefreshTokens].[UserId] = [AspNetUsers].[Id] 
    WHERE [AspNetUsers].[NormalizedEmail] = @UPPER_EMAIL
    
END