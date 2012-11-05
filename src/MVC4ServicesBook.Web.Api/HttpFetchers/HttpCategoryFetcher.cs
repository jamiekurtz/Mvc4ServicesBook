using System.Net;
using System.Net.Http;
using System.Web.Http;
using MVC4ServicesBook.Data.Model;
using NHibernate;

namespace MVC4ServicesBook.Web.Api.HttpFetchers
{
    public class HttpCategoryFetcher : IHttpCategoryFetcher
    {
        private readonly ISession _session;

        public HttpCategoryFetcher(ISession session)
        {
            _session = session;
        }

        public Category GetCategory(long categoryId)
        {
            var category = _session.Get<Category>(categoryId);
            if (category == null)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.NotFound,
                            ReasonPhrase = string.Format("Category {0} not found", categoryId)
                        });
            }
            return category;
        }
    }
}