using System.Collections.Generic;
using System.Security.Claims;
using System.Web.Http;
using BasicAuthForWebAPI;
using MVC4ServicesBook.Data.Model;
using NHibernate;

[assembly: WebActivator.PreApplicationStartMethod(typeof(MVC4ServicesBook.Web.Api.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(MVC4ServicesBook.Web.Api.App_Start.NinjectWebCommon), "Stop")]

namespace MVC4ServicesBook.Web.Api.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            var containerConfigurator = new NinjectConfigurator();
            containerConfigurator.Configure(kernel);

            var authHandler = new BasicAuthenticationMessageHandler
                {
                    GetAdditionalClaims = user =>
                        {
                            var sessionFactory = kernel.Get<ISessionFactory>();
                            using (var session = sessionFactory.OpenSession())
                            {
                                var userId = Guid.Parse(user.UserId);
                                var modelUser = session.Get<User>(userId);
                                
                                var claims = new List<Claim>
                                {
                                    new Claim(ClaimTypes.GivenName, modelUser.Firstname),
                                    new Claim(ClaimTypes.Surname, modelUser.Lastname)
                                };

                                return claims;
                            }

                        }
                };

            GlobalConfiguration.Configuration.MessageHandlers.Add(authHandler);
        }
    }
}
