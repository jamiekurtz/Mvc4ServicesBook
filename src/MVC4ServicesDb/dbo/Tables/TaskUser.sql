CREATE TABLE [dbo].[TaskUser]
(
	[TaskId] bigint NOT NULL,
	[UserId] uniqueidentifier not null,
	[ts] rowversion not null,
	constraint pk_TaskUser primary key (TaskId, UserId)
)
go

create index ix_TaskUser_UserId on TaskUser(UserId)
go

alter table dbo.TaskUser 
	add constraint fk_TaskUser_User foreign key (UserId) 
	references dbo.[User] (UserId)
go

alter table dbo.TaskUser 
	add constraint fk_taskUser_Task foreign key (TaskId)
	references dbo.Task	(TaskId) 
go
