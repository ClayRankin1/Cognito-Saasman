--IF NOT EXISTS (SELECT * FROM [User].[AspNetRoles] WHERE [NormalizedName] = N'SYSADMIN')
--BEGIN
--    INSERT INTO [User].[AspNetRoles] ([Name], [NormalizedName])
--    VALUES (N'SysAdmin', N'SYSADMIN')
--END

--IF NOT EXISTS (SELECT * FROM [User].[AspNetRoles] WHERE [NormalizedName] = N'TENANTADMIN')
--BEGIN
--    INSERT INTO [User].[AspNetRoles] ([Name], [NormalizedName])
--    VALUES (N'TenantAdmin', N'TENANTADMIN')
--END

--IF NOT EXISTS (SELECT * FROM [User].[AspNetRoles] WHERE [NormalizedName] = N'REPORTSUSER')
--BEGIN
--    INSERT INTO [User].[AspNetRoles] ([Name], [NormalizedName])
--    VALUES (N'ReportsUser', N'REPORTSUSER')
--END

--IF NOT EXISTS (SELECT * FROM [User].[AspNetRoles] WHERE [NormalizedName] = N'PROJECTOWNER')
--BEGIN
--    INSERT INTO [User].[AspNetRoles] ([Name], [NormalizedName])
--    VALUES (N'ProjectOwner', N'PROJECTOWNER')
--END

--IF NOT EXISTS (SELECT * FROM [User].[AspNetRoles] WHERE [NormalizedName] = N'PROJECTPROXY')
--BEGIN
--    INSERT INTO [User].[AspNetRoles] ([Name], [NormalizedName])
--    VALUES (N'ProjectProxy', N'PROJECTPROXY')
--END

--IF NOT EXISTS (SELECT * FROM [User].[AspNetRoles] WHERE [NormalizedName] = N'PROJECTTEAMMEMBER')
--BEGIN
--    INSERT INTO [User].[AspNetRoles] ([Name], [NormalizedName])
--    VALUES (N'ProjectTeamMember', N'PROJECTTEAMMEMBER')
--END

--IF NOT EXISTS (SELECT * FROM [User].[AspNetRoles] WHERE [NormalizedName] = N'TENANTREPORTSUSER')
--BEGIN
--    INSERT INTO [User].[AspNetRoles] ([Name], [NormalizedName])
--    VALUES (N'TenantReportsUser', N'TENANTREPORTSUSER')
--END

--IF NOT EXISTS (SELECT * FROM [User].[AspNetRoles] WHERE [NormalizedName] = N'TIMEKEEPER')
--BEGIN
--    INSERT INTO [User].[AspNetRoles] ([Name], [NormalizedName])
--    VALUES (N'TimeKeeper', N'TIMEKEEPER')
--END

--IF NOT EXISTS (SELECT * FROM [User].[AspNetRoles] WHERE [NormalizedName] = N'TASKOWNER')
--BEGIN
--    INSERT INTO [User].[AspNetRoles] ([Name], [NormalizedName])
--    VALUES (N'TaskOwner', N'TASKOWNER')
--END

--IF NOT EXISTS (SELECT * FROM [User].[AspNetRoles] WHERE [NormalizedName] = N'SUBTASKOWNER')
--BEGIN
--    INSERT INTO [User].[AspNetRoles] ([Name], [NormalizedName])
--    VALUES (N'SubtaskOwner', N'SUBTASKOWNER')
--END