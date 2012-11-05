using MVC4ServicesBook.Web.Api.Models;

namespace MVC4ServicesBook.Web.Api.TypeMappers
{
    public interface ICategoryMapper
    {
        Category CreateCategory(Data.Model.Category modelCategory);
    }
}