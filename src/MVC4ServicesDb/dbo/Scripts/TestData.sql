
declare @statusId int,
	@priorityId int

select top 1 @statusId = StatusId from Status;
select top 1 @priorityId = PriorityId from Status;

insert into dbo.Task(Subject, StartDate, PriorityId, StatusId, CreatedDate)
	values('Test Task', getdate(), @priorityId, @statusId, getdate())
