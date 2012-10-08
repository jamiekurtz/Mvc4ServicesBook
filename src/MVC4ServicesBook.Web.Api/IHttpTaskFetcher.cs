using MVC4ServicesBook.Data.Model;

namespace MVC4ServicesBook.Web.Api
{
    public interface IHttpTaskFetcher
    {
        Task GetTask(long taskId);
    }
}