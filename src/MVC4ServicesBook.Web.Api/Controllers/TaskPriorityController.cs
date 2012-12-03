using System.Web.Http;
using MVC4ServicesBook.Web.Api.HttpFetchers;
using MVC4ServicesBook.Web.Api.Models;
using MVC4ServicesBook.Web.Api.TypeMappers;
using MVC4ServicesBook.Web.Common;
using NHibernate;

namespace MVC4ServicesBook.Web.Api.Controllers
{
    [LoggingNHibernateSession]
    public class TaskPriorityController : ApiController
    {
        private readonly ISession _session;
        private readonly IPriorityMapper _priorityMapper;
        private readonly IHttpPriorityFetcher _priorityFetcher;
        private readonly IHttpTaskFetcher _taskFetcher;

        public TaskPriorityController(
            IHttpTaskFetcher taskFetcher, 
            ISession session,
            IPriorityMapper priorityMapper,
            IHttpPriorityFetcher priorityFetcher)
        {
            _taskFetcher = taskFetcher;
            _session = session;
            _priorityMapper = priorityMapper;
            _priorityFetcher = priorityFetcher;
        }

        public Priority Get(long taskId)
        {
            var task = _taskFetcher.GetTask(taskId);
            return _priorityMapper.CreatePriority(task.Priority);
        }

        public void Put(long taskId, long priorityId)
        {
            var task = _taskFetcher.GetTask(taskId);

            var priority = _priorityFetcher.GetPriority(priorityId);

            task.Priority = priority;

            _session.Save(task);
        }
    }
}