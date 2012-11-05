using System;
using MVC4ServicesBook.Data.Model;

namespace MVC4ServicesBook.Web.Api.HttpFetchers
{
    public interface IHttpUserFetcher
    {
        User GetUser(Guid userId);
    }
}