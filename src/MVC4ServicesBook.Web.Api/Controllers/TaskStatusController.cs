using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using MVC4ServicesBook.Data;
using MVC4ServicesBook.Web.Api.Models;
using MVC4ServicesBook.Web.Common;

namespace MVC4ServicesBook.Web.Api.Controllers
{
    [LoggingNHibernateSessions]
    public class TaskStatusController : ApiController
    {
        private readonly ICommonRepository _commonRepository;

        public TaskStatusController(ICommonRepository commonRepository)
        {
            _commonRepository = commonRepository;
        }

        public Status Get(long taskId)
        {
            var task = _commonRepository.Get<Data.Model.Task>(taskId);
            if(task == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return new Status
                       {
                           Name = task.Status.Name,
                           Ordinal = task.Status.Ordinal,
                           StatusId = task.Status.StatusId
                       };
        }

        public void Put(long taskId, long statusId)
        {
            var task = _commonRepository.Get<Data.Model.Task>(taskId);
            if (task == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var status = _commonRepository.Get<Data.Model.Status>(statusId);
            if (status == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            task.Status = status;

            _commonRepository.Save(task);
        }
    }
}