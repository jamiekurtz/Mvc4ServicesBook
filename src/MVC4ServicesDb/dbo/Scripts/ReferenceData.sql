if not exists(Select * from [dbo].[aspnet_SchemaVersions])
begin
	insert into dbo.aspnet_SchemaVersions (Feature, CompatibleSchemaVersion, IsCurrentVersion)
		values('common', 1, 1)
	insert into dbo.aspnet_SchemaVersions (Feature, CompatibleSchemaVersion, IsCurrentVersion)
		values('membership', 1, 1)
	insert into dbo.aspnet_SchemaVersions (Feature, CompatibleSchemaVersion, IsCurrentVersion)
		values('role manager', 1, 1)
end

if not exists(select * from dbo.Priority where Name = 'Low')
	insert into dbo.Priority(Name, Ordinal) values('Low', 0);
if not exists(select * from dbo.Priority where Name = 'Medium')
	insert into dbo.Priority(Name, Ordinal) values('Medium', 1);
if not exists(select * from dbo.Priority where Name = 'High')
	insert into dbo.Priority(Name, Ordinal) values('High', 2);

if not exists(select * from dbo.Status where Name = 'Not Started')
	insert into dbo.Status(Name, Ordinal) values('Not Started', 0);
if not exists(select * from dbo.Status where Name = 'In Progress')
	insert into dbo.Status(Name, Ordinal) values('In Progress', 1);
if not exists(select * from dbo.Status where Name = 'Completed')
	insert into dbo.Status(Name, Ordinal) values('Completed', 2);

if not exists(select * from dbo.Category where Name = 'Red')
	insert into dbo.Category(Name, Description) values('Red', 'Next action');
if not exists(select * from dbo.Category where Name = 'Purple')
	insert into dbo.Category(Name, Description) values('Purple', 'Waiting on someone');
if not exists(select * from dbo.Category where Name = 'Orange')
	insert into dbo.Category(Name, Description) values('Orange', 'Reference');
