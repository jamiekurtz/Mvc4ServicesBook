using MVC4ServicesBook.Web.Api.Models;

namespace MVC4ServicesBook.Web.Api.TypeMappers
{
    public interface IPriorityMapper
    {
        Priority CreatePriority(Data.Model.Priority priority);
    }
}