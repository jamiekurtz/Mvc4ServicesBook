using System.Collections.Generic;
using System.Linq;
using MVC4ServicesBook.Web.Api.Models;

namespace MVC4ServicesBook.Web.Api.TypeMappers
{
    public class TaskMapper : ITaskMapper
    {
        private readonly ICategoryMapper _categoryMapper;
        private readonly IStatusMapper _statusMapper;
        private readonly IPriorityMapper _priorityMapper;
        private readonly IUserMapper _userMapper;

        public TaskMapper(
            IUserMapper userMapper, 
            IPriorityMapper priorityMapper,
            IStatusMapper statusMapper, 
            ICategoryMapper categoryMapper)
        {
            _userMapper = userMapper;
            _priorityMapper = priorityMapper;
            _statusMapper = statusMapper;
            _categoryMapper = categoryMapper;
        }

        public Task CreateTask(Data.Model.Task modelTask)
        {
            var task = new Task
                           {
                               TaskId = modelTask.TaskId,
                               Subject = modelTask.Subject,
                               StartDate = modelTask.StartDate,
                               DateCompleted = modelTask.DateCompleted,
                               DueDate = modelTask.DueDate,
                               CreatedDate = modelTask.CreatedDate,
                               Status = _statusMapper.CreateStatus(modelTask.Status),
                               Priority = _priorityMapper.CreatePriority(modelTask.Priority),
                               Categories = modelTask
                                   .Categories
                                   .Select(_categoryMapper.CreateCategory)
                                   .ToList(),
                               Assignees = modelTask
                                   .Users
                                   .Select(_userMapper.CreateUser)
                                   .ToList()
                           };

            task.Links = new List<Link>
                             {
                                 new Link
                                     {
                                         Title = "self",
                                         Rel = "self",
                                         Href = "/api/tasks/" + task.TaskId
                                     },
                                 new Link
                                     {
                                         Title = "Categories",
                                         Rel = "categories",
                                         Href = "/api/tasks/" + task.TaskId + "/categories"
                                     },
                                 new Link
                                     {
                                         Title = "Assignees",
                                         Rel = "users",
                                         Href = "/api/tasks/" + task.TaskId + "/users"
                                     }
                             };

            return task;
        }
    }
}