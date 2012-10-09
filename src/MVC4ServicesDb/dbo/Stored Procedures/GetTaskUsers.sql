create proc [dbo].[GetTaskUsers]
(
	@taskId bigint
)
as
begin

	set nocount on

	select	u.*
		from dbo.AllUsers u
		inner join dbo.TaskUser tu on tu.UserId = u.UserId
		where tu.TaskId = @taskId

end

go