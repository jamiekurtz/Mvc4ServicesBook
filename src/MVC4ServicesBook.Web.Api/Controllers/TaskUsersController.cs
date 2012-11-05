using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MVC4ServicesBook.Web.Api.HttpFetchers;
using MVC4ServicesBook.Web.Api.Models;
using MVC4ServicesBook.Web.Api.TypeMappers;
using MVC4ServicesBook.Web.Common;
using NHibernate;

namespace MVC4ServicesBook.Web.Api.Controllers
{
    [LoggingNHibernateSessions]
    public class TaskUsersController : ApiController
    {
        private readonly ISession _session;
        private readonly IUserMapper _userMapper;
        private readonly IHttpTaskFetcher _taskFetcher;

        public TaskUsersController(IHttpTaskFetcher taskFetcher, ISession session, IUserMapper userMapper)
        {
            _taskFetcher = taskFetcher;
            _session = session;
            _userMapper = userMapper;
        }

        public IEnumerable<User> Get(long taskId)
        {
            var task = _taskFetcher.GetTask(taskId);

            return task
                .Users
                .Select(_userMapper.CreateUser)
                .ToList();
        }

        public void Delete(long taskId)
        {
            var task = _taskFetcher.GetTask(taskId);

            task.Users
                .ToList()
                .ForEach(x => task.Users.Remove(x));

            _session.Save(task);
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

            _session.Save(task);
        }       
        
        public void Put(long taskId, Guid userId)
        {
            var task = _taskFetcher.GetTask(taskId);

            var user = task.Users.FirstOrDefault(x => x.UserId == userId);
            if (user != null)
            {
                return;
            }

            user = _session.Get<Data.Model.User>(userId);
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

            _session.Save(task);
        }
    }
}