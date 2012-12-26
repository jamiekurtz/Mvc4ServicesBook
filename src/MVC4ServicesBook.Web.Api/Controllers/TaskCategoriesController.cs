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
    public class TaskCategoriesController : ApiController
    {
        private readonly ISession _session;
        private readonly ICategoryMapper _categoryMapper;
        private readonly IHttpTaskFetcher _taskFetcher;
        private readonly IHttpCategoryFetcher _categoryFetcher;

        public TaskCategoriesController(
            IHttpTaskFetcher taskFetcher,
            ISession session,
            ICategoryMapper categoryMapper,
            IHttpCategoryFetcher categoryFetcher)
        {
            _taskFetcher = taskFetcher;
            _session = session;
            _categoryMapper = categoryMapper;
            _categoryFetcher = categoryFetcher;
        }

        public IEnumerable<Category> Get(long taskId)
        {
            var task = _taskFetcher.GetTask(taskId);

            return task
                .Categories
                .Select(_categoryMapper.CreateCategory)
                .ToList();
        }

        public void Put(long taskId, long categoryId)
        {
            var task = _taskFetcher.GetTask(taskId);

            var category = task.Categories.FirstOrDefault(x => x.CategoryId == categoryId);
            if (category != null)
            {
                return;
            }

            category = _categoryFetcher.GetCategory(categoryId);

            task.Categories.Add(category);

            _session.SaveOrUpdate(task);
        }

        public void Delete(long taskId)
        {
            var task = _taskFetcher.GetTask(taskId);

            task.Categories
                .ToList()
                .ForEach(x => task.Categories.Remove(x));

            _session.SaveOrUpdate(task);
        }

        public void Delete(long taskId, long categoryId)
        {
            var task = _taskFetcher.GetTask(taskId);

            var category = task.Categories.FirstOrDefault(x => x.CategoryId == categoryId);
            if (category == null)
            {
                return;
            }

            task.Categories.Remove(category);

            _session.SaveOrUpdate(task);
        }
    }
}