using System;
using System.Web.Http;
using System.Web.Http.Dependencies;

namespace MVC4ServicesBook.Web.Common
{
    public static class WebContainerManager
    {
        public static IDependencyResolver GetContainer()
        {
            var dependencyResolver = GlobalConfiguration.Configuration.DependencyResolver;
            if (dependencyResolver != null)
            {
                return dependencyResolver;
            }

            throw new InvalidOperationException("NinjectDependencyResolver not being used as the MVC dependency resolver");
        }

        public static T Get<T>()
        {
            object service = GetContainer().GetService(typeof(T));
            
            if (service == null)
                throw new NullReferenceException(string.Format("Requested service of type {0}, but null was found.", typeof(T).FullName));

            return (T)service;
        }
    }
}