using MVC4ServicesBook.Data.Model;

namespace MVC4ServicesBook.Web.Api.HttpFetchers
{
    public interface IHttpStatusFetcher
    {
        Status GetStatus(long statusId);
    }
}