using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MVC4ServicesBook.Data;
using MVC4ServicesBook.Web.Api.Models;
using MVC4ServicesBook.Web.Common;
using MVC4ServicesBook.Web.Common.Security;

namespace MVC4ServicesBook.Web.Api.Controllers
{
    [LoggingNHibernateSessions]
    public class TaskCategoriesController : ApiController
    {
        private readonly ICommonRepository _commonRepository;
        private readonly IHttpTaskFetcher _taskFetcher;

        public TaskCategoriesController(ICommonRepository commonRepository, IHttpTaskFetcher taskFetcher)
        {
            _commonRepository = commonRepository;
            _taskFetcher = taskFetcher;
        }

        public IEnumerable<Category> Get(long taskId)
        {
            var task = _taskFetcher.GetTask(taskId);

            return task
                .Categories
                .Select(x => new Category
                                 {
                                     CategoryId = x.CategoryId,
                                     Description = x.Description,
                                     Name = x.Name
                                 })
                .ToList();
        }

        public void Put(long taskId, long categoryId)
        {
            var task = _taskFetcher.GetTask(taskId);

            var category = task.Categories.FirstOrDefault(x => x.CategoryId == categoryId);
            if(category != null)
            {
                return;
            }

            category = _commonRepository.Get<Data.Model.Category>(categoryId);
            if (category == null)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        ReasonPhrase = string.Format("Category {0} not found", categoryId)
                    });
            }

            task.Categories.Add(category);

            _commonRepository.Save(task);
        }

        public void Delete(long taskId)
        {
            var task = _taskFetcher.GetTask(taskId);

            task.Categories
                .ToList()
                .ForEach(x => task.Categories.Remove(x));

            _commonRepository.Save(task);
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

            _commonRepository.Save(task);
        }
    }
}