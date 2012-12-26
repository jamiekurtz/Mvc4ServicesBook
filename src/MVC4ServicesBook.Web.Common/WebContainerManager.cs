using System;
using System.Web.Http;
using Ninject;

namespace MVC4ServicesBook.Web.Common
{
    public static class WebContainerManager
    {
        public static IKernel GetContainer()
        {
            var resolver = GlobalConfiguration.Configuration.DependencyResolver as NinjectDependencyResolver;
            if (resolver != null)
            {
                return resolver.Container;
            }

            throw new InvalidOperationException("NinjectDependencyResolver not being used as the MVC dependency resolver");
        }

        public static T Get<T>()
        {
            return GetContainer().Get<T>();
        }
    }
}