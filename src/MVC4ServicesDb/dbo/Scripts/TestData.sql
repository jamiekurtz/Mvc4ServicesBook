
declare @statusId int,
	@priorityId int,
	@taskId int,
	@categoryId int,
	@userId uniqueidentifier

if not exists(select * from dbo.aspnet_Applications where ApplicationId = '8b2e549a-c283-46f2-b481-25136daa9059')
	insert into dbo.aspnet_Applications(ApplicationName, LoweredApplicationName, ApplicationId)
		values('/', '/', '8b2e549a-c283-46f2-b481-25136daa9059')

if not exists (select * from aspnet_Roles where RoleId = N'6c82524a-b1e0-4b20-97b1-dbdf0dadad8e')
	INSERT [dbo].[aspnet_Roles] ([ApplicationId], [RoleId], [RoleName], [LoweredRoleName], [Description]) 
		VALUES (N'8b2e549a-c283-46f2-b481-25136daa9059', N'6c82524a-b1e0-4b20-97b1-dbdf0dadad8e', N'Administrators', N'adminstrators', NULL)

if not exists (select * from [aspnet_Users] where UserId = N'6c82524a-b1e0-4b20-97b1-dbdf0dadad8e')
	INSERT [dbo].[aspnet_Users] ([ApplicationId], [UserId], [UserName], [LoweredUserName], [MobileAlias], [IsAnonymous], [LastActivityDate]) 
		VALUES (N'8b2e549a-c283-46f2-b481-25136daa9059', N'6c82524a-b1e0-4b20-97b1-dbdf0dadad8e', N'jbob', N'jbob', NULL, 0, CAST(0x0000A100003CF03D AS DateTime))

if not exists(select * from aspnet_UsersInRoles where userId = N'6c82524a-b1e0-4b20-97b1-dbdf0dadad8e' and roleId = N'6c82524a-b1e0-4b20-97b1-dbdf0dadad8e')
	INSERT [dbo].[aspnet_UsersInRoles] ([UserId], [RoleId]) 
		VALUES (N'6c82524a-b1e0-4b20-97b1-dbdf0dadad8e', N'6c82524a-b1e0-4b20-97b1-dbdf0dadad8e')

if not exists (select * from [aspnet_Membership] where UserId = N'6c82524a-b1e0-4b20-97b1-dbdf0dadad8e')
	INSERT [dbo].[aspnet_Membership] ([ApplicationId], [UserId], [Password], [PasswordFormat], [PasswordSalt], [MobilePIN], [Email], [LoweredEmail], [PasswordQuestion], [PasswordAnswer], [IsApproved], [IsLockedOut], [CreateDate], [LastLoginDate], [LastPasswordChangedDate], [LastLockoutDate], [FailedPasswordAttemptCount], [FailedPasswordAttemptWindowStart], [FailedPasswordAnswerAttemptCount], [FailedPasswordAnswerAttemptWindowStart], [Comment])
		VALUES (N'8b2e549a-c283-46f2-b481-25136daa9059', N'6c82524a-b1e0-4b20-97b1-dbdf0dadad8e', N'RahmIwZNZNs7icla4wK9U6oGnr4=', 1, N'SO/L9Bthj5NwZUtWHB1vSg==', NULL, N'jbob@gmail.com', N'jbob@gmail.com', NULL, NULL, 1, 0, CAST(0x0000A0E600088AB8 AS DateTime), CAST(0x0000A100003CF03D AS DateTime), CAST(0x0000A0E600088AB8 AS DateTime), CAST(0xFFFF2FB300000000 AS DateTime), 0, CAST(0xFFFF2FB300000000 AS DateTime), 0, CAST(0xFFFF2FB300000000 AS DateTime), NULL)

if not exists (select * from [User] where UserId = N'6c82524a-b1e0-4b20-97b1-dbdf0dadad8e')
	INSERT [dbo].[User] ([UserId], [Firstname], [Lastname]) 
		VALUES (N'6c82524a-b1e0-4b20-97b1-dbdf0dadad8e', N'Jim', N'Bob')


if not exists(select * from dbo.Task where Subject = 'Test Task')
begin
	select top 1 @statusId = StatusId from Status;
	select top 1 @priorityId = PriorityId from Priority;
	select top 1 @categoryId = CategoryId from Category;
	select top 1 @userId = UserId from [User];

	insert into dbo.Task(Subject, StartDate, PriorityId, StatusId, CreatedDate, CreatedUserId)
		values('Test Task', getdate(), @priorityId, @statusId, getdate(), @userId);

	set @taskId = SCOPE_IDENTITY();
	
	insert into dbo.TaskCategory(TaskId, CategoryId)
		values(@taskId, @categoryId)

	INSERT [dbo].[TaskUser] ([TaskId], [UserId]) 
		VALUES (@taskId, N'6c82524a-b1e0-4b20-97b1-dbdf0dadad8e')
end





