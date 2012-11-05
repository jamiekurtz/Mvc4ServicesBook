using MVC4ServicesBook.Web.Api.Models;

namespace MVC4ServicesBook.Web.Api.TypeMappers
{
    public interface IStatusMapper
    {
        Status CreateStatus(Data.Model.Status status);
    }
}