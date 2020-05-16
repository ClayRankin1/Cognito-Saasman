CREATE OR ALTER PROCEDURE [dbo].[MergeSubtasks]
    @TaskId INT,
    @UserIds NVARCHAR(1000)
AS
BEGIN

    ;WITH [Target] AS (
		SELECT
			[Subtasks].[TaskId],
			[Subtasks].[UserId],
			[Subtasks].[IsDeleted]
		FROM [Task].[Subtasks]
		WHERE [TaskId] = @TaskId
	),
	[Source] AS (
		SELECT
			@TaskId AS [TaskId],
			value AS [UserId]
		FROM STRING_SPLIT(@UserIds, ',')
		WHERE ISNULL(TRIM([value]), '') <> ''
	)
	MERGE INTO [Target] WITH (HOLDLOCK)
	USING [Source]
		ON [Target].[TaskId] = [Source].[TaskId] AND [Target].[UserId] = [Source].[UserId]
	WHEN MATCHED THEN 
		UPDATE SET [Target].[IsDeleted] = 0
	WHEN NOT MATCHED BY TARGET THEN
		INSERT ([TaskId], [UserId], [IsDeleted])
		VALUES ([Source].[TaskId], [Source].[UserId], 0)
	WHEN NOT MATCHED BY SOURCE THEN
		UPDATE SET [Target].[IsDeleted] = 1;

END
