using System.Net;
using System.Web.Http;
using MVC4ServicesBook.Data;
using MVC4ServicesBook.Web.Api.Models;

namespace MVC4ServicesBook.Web.Api.Controllers
{
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
                                              }
                           };
            return task;
        }
    }
}