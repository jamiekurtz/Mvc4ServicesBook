using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using MVC4ServicesBook.Web.Api.HttpFetchers;
using MVC4ServicesBook.Web.Api.Models;
using MVC4ServicesBook.Web.Api.TypeMappers;
using MVC4ServicesBook.Web.Common;
using NHibernate;

namespace MVC4ServicesBook.Web.Api.Controllers
{
    [LoggingNHibernateSession]
    public class TaskUsersController : ApiController
    {
        private readonly ISession _session;
        private readonly IUserMapper _userMapper;
        private readonly IHttpTaskFetcher _taskFetcher;
        private readonly IHttpUserFetcher _userFetcher;

        public TaskUsersController(
            IHttpTaskFetcher taskFetcher, 
            IHttpUserFetcher userFetcher,
            ISession session, 
            IUserMapper userMapper)
        {
            _taskFetcher = taskFetcher;
            _userFetcher = userFetcher;
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

            _session.SaveOrUpdate(task);
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

            _session.SaveOrUpdate(task);
        }       
        
        public void Put(long taskId, Guid userId)
        {
            var task = _taskFetcher.GetTask(taskId);

            var user = task.Users.FirstOrDefault(x => x.UserId == userId);
            if (user != null)
            {
                return;
            }

            user = _userFetcher.GetUser(userId);

            task.Users.Add(user);

            _session.SaveOrUpdate(task);
        }
    }
}