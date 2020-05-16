CREATE OR ALTER PROCEDURE [Task].[CopyTask]
    @TaskId INT,
    @UpdatedByUserId INT,
    @DateUpdated DATETIME2(7),
    @IsCopyRelatedData BIT
AS
BEGIN

    -- COPY TASK
    INSERT INTO [Task].[Tasks] (
        [ProjectId],
        [OwnerId],
        [NextDate],
        [EndDate],
        [Description],
        [Accrued],
        [TaskStatusId],
        [IsEvent],
        [DisplayOrder],
        [TimeId],
        [TaskTypeId],
        [IsDeleted],
        [DateAdded],
        [DateUpdated],
        [CreatedByUserId],
        [UpdatedByUserId]
    )
    SELECT
        [ProjectId],
        [OwnerId],
        [NextDate],
        [EndDate],
        [Description],
        [Accrued],
        [TaskStatusId],
        [IsEvent],
        [DisplayOrder],
        [TimeId],
        [TaskTypeId],
        [IsDeleted],
        @DateUpdated,
        @DateUpdated,
        @UpdatedByUserId,
        @UpdatedByUserId
    FROM [Task].[Tasks]
    WHERE [Id] = @TaskId

    IF (@IsCopyRelatedData = 0)
    BEGIN
        RETURN;
    END

    DECLARE @NewTaskId INT = (SELECT CAST(SCOPE_IDENTITY() AS INT));

    -- COPY DETAILS
    INSERT INTO [Detail].[Details] (
        [DateAdded],
        [DateUpdated],
        [CreatedByUserId],
        [UpdatedByUserId],
        [Body],
        [Subject],
        [Source],
        [SourceId],
        [BeginPage],
        [BeginLine],
        [EndPage],
        [EndLine],
        [Chrono],
        [DisplayOrder],
        [DetailTypeId],
        [TaskId],
		[IsDeleted]
    )
    SELECT
        @DateUpdated,
        @DateUpdated,
        @UpdatedByUserId,
        @UpdatedByUserId,
        [Body],
        [Subject],
        [Source],
        [SourceId],
        [BeginPage],
        [BeginLine],
        [EndPage],
        [EndLine],
        [Chrono],
        [DisplayOrder],
        [DetailTypeId],
        @NewTaskId,
		[IsDeleted]
    FROM [Detail].[Details]
    WHERE [TaskId] = @TaskId

    -- WEBSITES
    INSERT INTO [Task].[TaskWebsites] (
        [TaskId],
        [WebsiteId]
    )
    SELECT
        @NewTaskId,
        [WebsiteId]
    FROM [Task].[TaskWebsites]
    WHERE [TaskId] = @TaskId

    -- CONTACTS
    INSERT INTO [Task].[TaskContacts] (
        [TaskId],
        [ContactId]
    )
    SELECT
        @NewTaskId,
        [ContactId]
    FROM [Task].[TaskContacts]
    WHERE [TaskId] = @TaskId

    -- DOCUMENTS
    INSERT INTO [Task].[TaskDocuments] (
        [TaskId],
        [DocumentId]
    )
    SELECT
        @NewTaskId,
        [DocumentId]
    FROM [Task].[TaskDocuments]
    WHERE [TaskId] = @TaskId

END
