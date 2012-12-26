using MVC4ServicesBook.Web.Api.Models;

namespace MVC4ServicesBook.Web.Api.TypeMappers
{
    public interface ITaskMapper
    {
        Task CreateTask(Data.Model.Task modelTask);
    }
}