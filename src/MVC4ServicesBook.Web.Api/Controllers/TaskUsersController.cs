using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MVC4ServicesBook.Data;
using MVC4ServicesBook.Web.Api.Models;
using MVC4ServicesBook.Web.Common;

namespace MVC4ServicesBook.Web.Api.Controllers
{
    [LoggingNHibernateSessions]
    public class TaskUsersController : ApiController
    {
        private readonly ICommonRepository _commonRepository;
        private readonly IHttpTaskFetcher _taskFetcher;

        public TaskUsersController(ICommonRepository commonRepository, IHttpTaskFetcher taskFetcher)
        {
            _commonRepository = commonRepository;
            _taskFetcher = taskFetcher;
        }

        public IEnumerable<User> Get(long taskId)
        {
            var task = _taskFetcher.GetTask(taskId);

            return task
                .Users
                .Select(x => new User
                                 {
                                     UserId = x.UserId,
                                     Email = x.Email,
                                     Firstname = x.Firstname,
                                     Lastname = x.Lastname,
                                     Username = x.Username
                                 })
                .ToList();
        }

        public void Delete(long taskId)
        {
            var task = _taskFetcher.GetTask(taskId);

            task.Users
                .ToList()
                .ForEach(x => task.Users.Remove(x));

            _commonRepository.Save(task);
        }

        public void Delete(long taskId, Guid userId)
        {
            var task = _taskFetcher.GetTask(taskId);

            var user = task.Users.FirstOrDefault(x => x.UserId == userId);
            if(user == null)
            {
                return;
            }

            task.Users.Remove(user);

            _commonRepository.Save(task);
        }       
        
        public void Put(long taskId, Guid userId)
        {
            var task = _taskFetcher.GetTask(taskId);

            var user = task.Users.FirstOrDefault(x => x.UserId == userId);
            if (user != null)
            {
                return;
            }

            user = _commonRepository.Get<Data.Model.User>(userId);
            if(user == null)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.NotFound,
                            ReasonPhrase = string.Format("User {0} not found", userId)
                        });
            }

            task.Users.Add(user);
            
            _commonRepository.Save(task);
        }
    }
}