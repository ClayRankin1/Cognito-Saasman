CREATE OR ALTER PROCEDURE [dbo].[MergeProjectUsers]
    @ProjectId INT,
    @UserIds NVARCHAR(1000)
AS
BEGIN

    WITH [Target] AS (
		SELECT
			[ProjectUsers].[ProjectId],
			[ProjectUsers].[UserId],
			[ProjectUsers].[IsDeleted]
		FROM [dbo].[ProjectUsers]
		WHERE [ProjectId] = @ProjectId
	),
	[Source] AS (
		SELECT
			@ProjectId AS [ProjectId],
			value AS [UserId]
		FROM STRING_SPLIT(@UserIds, ',')
		WHERE DATALENGTH([value]) > 0
	)
	MERGE INTO [Target] WITH (HOLDLOCK)
	USING [Source]
		ON [Target].[ProjectId] = [Source].[ProjectId] AND [Target].[UserId] = [Source].[UserId]
	WHEN MATCHED THEN 
		UPDATE SET [Target].[IsDeleted] = 0
	WHEN NOT MATCHED BY TARGET THEN
		INSERT ([ProjectId], [UserId], [IsDeleted])
		VALUES ([Source].[ProjectId], [Source].[UserId], 0)
	WHEN NOT MATCHED BY SOURCE THEN
		UPDATE SET [Target].[IsDeleted] = 1;

END
