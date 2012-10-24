
declare @statusId int,
	@priorityId int

if not exists(select * from dbo.Task where Subject = 'Test Task')
begin
	select top 1 @statusId = StatusId from Status;
	select top 1 @priorityId = PriorityId from Priority;

	insert into dbo.Task(Subject, StartDate, PriorityId, StatusId, CreatedDate)
		values('Test Task', getdate(), @priorityId, @statusId, getdate())
end
