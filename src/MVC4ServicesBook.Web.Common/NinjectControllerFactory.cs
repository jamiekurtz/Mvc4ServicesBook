using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using Ninject;

namespace MVC4ServicesBook.Web.Common
{
    public class NinjectControllerFactory : IDependencyResolver
    {
        private readonly IKernel _container;

        public IKernel Container
        {
            get { return _container; }
        }

        public NinjectControllerFactory(IKernel container)
        {
            _container = container;
        }

        public object GetService(Type serviceType)
        {
            return serviceType == null ? null : _container.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return
                serviceType == null
                    ? null
                    : _container.GetAll(serviceType);
        }

        public IDependencyScope BeginScope()
        {
            return this;
        }

        public void Dispose()
        {
            // noop
        }
    }
}