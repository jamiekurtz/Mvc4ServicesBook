using System.Net;
using System.Net.Http;
using System.Web.Http;
using MVC4ServicesBook.Data;
using MVC4ServicesBook.Web.Api.Models;
using MVC4ServicesBook.Web.Common;

namespace MVC4ServicesBook.Web.Api.Controllers
{
    [LoggingNHibernateSessions]
    public class TaskPriorityController : ApiController
    {
        private readonly ICommonRepository _commonRepository;
        private readonly IHttpTaskFetcher _taskFetcher;

        public TaskPriorityController(ICommonRepository commonRepository, IHttpTaskFetcher taskFetcher)
        {
            _commonRepository = commonRepository;
            _taskFetcher = taskFetcher;
        }

        public Priority Get(long taskId)
        {
            var task = _taskFetcher.GetTask(taskId);

            return new Priority
                       {
                           Name = task.Priority.Name,
                           Ordinal = task.Priority.Ordinal,
                           PriorityId = task.Priority.PriorityId
                       };
        }

        public void Put(long taskId, long priorityId)
        {
            var task = _taskFetcher.GetTask(taskId);

            var priority = _commonRepository.Get<Data.Model.Priority>(priorityId);
            if (priority == null)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.NotFound,
                            ReasonPhrase = string.Format("Priority {0} not found", priorityId)
                        });
            }

            task.Priority = priority;

            _commonRepository.Save(task);
        }
    }
}