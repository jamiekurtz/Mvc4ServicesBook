using MVC4ServicesBook.Data.Model;

namespace MVC4ServicesBook.Web.Api.HttpFetchers
{
    public interface IHttpCategoryFetcher
    {
        Category GetCategory(long categoryId);
    }
}