using System.Net;
using System.Net.Http;
using System.Web.Http;
using MVC4ServicesBook.Data;
using MVC4ServicesBook.Data.Model;

namespace MVC4ServicesBook.Web.Api
{
    public class HttpTaskFetcher : IHttpTaskFetcher
    {
        private readonly ICommonRepository _commonRepository;

        public HttpTaskFetcher(ICommonRepository commonRepository)
        {
            _commonRepository = commonRepository;
        }

        public Task GetTask(long taskId)
        {
            var task = _commonRepository.Get<Task>(taskId);
            if (task == null)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.NotFound,
                            ReasonPhrase = string.Format("Task {0} not found", taskId)
                        });
            }
            return task;
        }
    }
}