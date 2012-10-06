using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using MVC4ServicesBook.Data;
using MVC4ServicesBook.Web.Api.Models;
using MVC4ServicesBook.Web.Common;

namespace MVC4ServicesBook.Web.Api.Controllers
{
    [LoggingNHibernateSessions]
    public class TasksController : ApiController
    {
        private readonly ICommonRepository _commonRepository;

        public TasksController(ICommonRepository commonRepository)
        {
            _commonRepository = commonRepository;
        }

        public Task Get(long id)
        {
            var modelTask = _commonRepository.Get<Data.Model.Task>(id);
            if(modelTask == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var task = CreateTaskFromModel(modelTask);

            return task;
        }

        public IEnumerable<Task> Get()
        {
            var modelTasks = _commonRepository.GetAll<Data.Model.Task>();

            var tasks = modelTasks.Select(CreateTaskFromModel).ToList();

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
                               Status = new Status
                                            {
                                                StatusId = modelTask.Status.StatusId,
                                                Name = modelTask.Status.Name,
                                                Ordinal = modelTask.Status.Ordinal
                                            },
                               Priority = new Priority
                                              {
                                                  PriorityId = modelTask.Priority.PriorityId,
                                                  Name = modelTask.Priority.Name,
                                                  Ordinal = modelTask.Priority.Ordinal
                                              },
                               Categories = modelTask
                                   .Categories
                                   .Select(x => new Category
                                                    {
                                                        CategoryId = x.CategoryId,
                                                        Description = x.Description,
                                                        Name = x.Name
                                                    })
                                   .ToList(),
                               Assignees = modelTask
                                   .Users
                                   .Select(x => new User
                                                    {
                                                        UserId = x.UserId,
                                                        Email = x.Email,
                                                        Firstname = x.Firstname,
                                                        Lastname = x.Lastname,
                                                        Username = x.Username
                                                    })
                                   .ToList()
                           };

            return task;
        }
    }
}