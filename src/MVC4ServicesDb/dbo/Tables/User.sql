CREATE TABLE [dbo].[User](
	[UserId] [uniqueidentifier] NOT NULL PRIMARY KEY CLUSTERED,
	[Firstname] [nvarchar](50) NOT NULL,
	[Lastname] [nvarchar](50) NOT NULL,
	[ts] [rowversion] NOT NULL
)
go

alter table dbo.[User]
	add constraint fk_User_aspnet_Users foreign key (UserId)
	references dbo.aspnet_Users (UserId)
go
