CREATE OR ALTER PROCEDURE [dbo].[UpdateUserRefreshToken]
(
	@UserId INT,
	@Now DATETIME2(7),
	@RefreshToken NVARCHAR(64),
	@RefreshTokenExpiration DATETIME,
	@ExpiredRefreshToken NVARCHAR(64) = NULL
)
AS
BEGIN

	-- NEW LOGIN
	IF (@ExpiredRefreshToken IS NOT NULL)
	BEGIN
		UPDATE [User].[RefreshTokens]
		SET
			[Token] = @RefreshToken,
			[Expiration] = @RefreshTokenExpiration
		WHERE [UserId] = @UserId AND [Token] = @ExpiredRefreshToken
	END
	-- REFRESHING TOKEN
	ELSE
	BEGIN
		INSERT INTO [User].[RefreshTokens] ([UserId], [Token], [Expiration])
		VALUES (@UserId, @RefreshToken, @RefreshTokenExpiration)
	END

	-- DELETE EXPIRED USER TOKENS
	DELETE [User].[RefreshTokens]
	WHERE [UserId] = @UserId AND [Expiration] < @Now
    
END