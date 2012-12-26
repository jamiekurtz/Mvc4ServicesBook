using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using MVC4ServicesBook.Web.Api.HttpFetchers;
using MVC4ServicesBook.Web.Api.Models;
using MVC4ServicesBook.Web.Api.TypeMappers;
using MVC4ServicesBook.Web.Common;
using MVC4ServicesBook.Web.Common.Security;
using NHibernate;
using NHibernate.Linq;

namespace MVC4ServicesBook.Web.Api.Controllers
{
    [LoggingNHibernateSession]
    public class TasksController : ApiController
    {
        private readonly ISession _session;
        private readonly IHttpTaskFetcher _taskFetcher;
        private readonly IUserSession _userSession;
        private readonly ICategoryMapper _categoryMapper;
        private readonly IStatusMapper _statusMapper;
        private readonly IPriorityMapper _priorityMapper;
        private readonly IUserMapper _userMapper;

        public TasksController(
            IHttpTaskFetcher taskFetcher, 
            IUserSession userSession, 
            ISession session, 
            ICategoryMapper categoryMapper,
            IStatusMapper statusMapper,
            IPriorityMapper priorityMapper,
            IUserMapper userMapper)
        {
            _taskFetcher = taskFetcher;
            _userSession = userSession;
            _session = session;
            _categoryMapper = categoryMapper;
            _statusMapper = statusMapper;
            _priorityMapper = priorityMapper;
            _userMapper = userMapper;
        }

        public Task Get(long id)
        {
            var modelTask = _taskFetcher.GetTask(id);
            var task = CreateTaskFromModel(modelTask);

            return task;
        }

        public IEnumerable<Task> Get()
        {
            var tasks = _session
                .Query<Data.Model.Task>()
                .Where(
                    x =>
                    x.CreatedBy.UserId == _userSession.UserId || x.Users.Any(user => user.UserId == _userSession.UserId))
                .Select(CreateTaskFromModel)
                .ToList();

            return tasks;
        }

        private Task CreateTaskFromModel(Data.Model.Task modelTask)
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