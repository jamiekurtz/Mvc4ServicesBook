using System.Collections.Generic;
using MVC4ServicesBook.Web.Api.Models;

namespace MVC4ServicesBook.Web.Api.TypeMappers
{
    public class CategoryMapper : ICategoryMapper
    {
        public Category CreateCategory(Data.Model.Category modelCategory)
        {
            return new Category
                       {
                           CategoryId = modelCategory.CategoryId,
                           Description = modelCategory.Description,
                           Name = modelCategory.Name,
                           Links = new List<Link>
                                       {
                                           new Link
                                               {
                                                   Title = "self",
                                                   Rel = "self",
                                                   Href = "/api/categories/" + modelCategory.CategoryId
                                               }
                                       }
                       };
        }
    }
}