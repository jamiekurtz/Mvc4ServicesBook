using System.Security.Principal;
using System.Threading;
using System.Web.Http;
using FluentNHibernate.Cfg.Db;
using MVC4ServicesBook.Common;
using MVC4ServicesBook.Data;
using MVC4ServicesBook.Data.SqlServer;
using MVC4ServicesBook.Web.Api.HttpFetchers;
using MVC4ServicesBook.Web.Api.TypeMappers;
using MVC4ServicesBook.Web.Common;
using MVC4ServicesBook.Web.Common.Security;
using NHibernate;
using NHibernate.Context;
using Ninject;
using Ninject.Activation;
using log4net;
using Ninject.Web.Common;

namespace MVC4ServicesBook.Web.Api.App_Start
{
    /// <summary>
    /// Class used to set up the Ninject DI container.
    /// </summary>
    public class NinjectConfigurator
    {
        /// <summary>
        /// Entry method used by caller to configure the given 
        /// container with all of this application's 
        /// dependencies. Also configures the container as this
        /// application's dependency resolver.
        /// </summary>
        public void Configure(IKernel container)
        {
            // Add all bindings/dependencies
            AddBindings(container);

            // Use the container and our NinjectDependencyResolver as
            // application's resolver
            var resolver = new NinjectDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = resolver;
        }

        /// <summary>
        /// Add all bindings/dependencies to the container
        /// </summary>
        private void AddBindings(IKernel container)
        {
            ConfigureNHibernate(container);

            ConfigureLog4net(container);

            container.Bind<IDateTime>().To<DateTimeAdapter>();
            container.Bind<IDatabaseValueParser>().To<DatabaseValueParser>();

            container.Bind<IHttpCategoryFetcher>().To<HttpCategoryFetcher>();
            container.Bind<IHttpPriorityFetcher>().To<HttpPriorityFetcher>();
            container.Bind<IHttpStatusFetcher>().To<HttpStatusFetcher>();
            container.Bind<IHttpUserFetcher>().To<HttpUserFetcher>();
            container.Bind<IHttpTaskFetcher>().To<HttpTaskFetcher>();
            container.Bind<IActionLogHelper>().To<ActionLogHelper>();
            container.Bind<IExceptionMessageFormatter>().To<ExceptionMessageFormatter>();
            container.Bind<IActionExceptionHandler>().To<ActionExceptionHandler>();
            container.Bind<IActionTransactionHelper>().To<ActionTransactionHelper>();

            container.Bind<IUserManager>().To<UserManager>();
            container.Bind<IMembershipAdapter>().To<MembershipAdapter>();
            container.Bind<ICategoryMapper>().To<CategoryMapper>();
            container.Bind<IPriorityMapper>().To<PriorityMapper>();
            container.Bind<IStatusMapper>().To<StatusMapper>();
            container.Bind<IUserMapper>().To<UserMapper>();
            container.Bind<ITaskMapper>().To<TaskMapper>();

            container.Bind<ISqlCommandFactory>().To<SqlCommandFactory>();
            container.Bind<IUserRepository>().To<UserRepository>();

            container.Bind<IUserSession>().ToMethod(CreateUserSession).InRequestScope();
        }

        /// <summary>
        /// Set up log4net for this application, including putting it in the 
        /// given container.
        /// </summary>
        private void ConfigureLog4net(IKernel container)
        {
            log4net.Config.XmlConfigurator.Configure();
            var loggerForWebSite = LogManager.GetLogger("Mvc4ServicesBookWebsite");
            container.Bind<ILog>().ToConstant(loggerForWebSite);
        }

        /// <summary>
        /// Used to fetch the current thread's principal as 
        /// an <see cref="IUserSession"/> object.
        /// </summary>
        private IUserSession CreateUserSession(IContext arg)
        {
            return new UserSession(Thread.CurrentPrincipal as GenericPrincipal);
        }

        /// <summary>
        /// Sets up NHibernate, and adds an ISessionFactory to the given
        /// container.
        /// </summary>
        private void ConfigureNHibernate(IKernel container)
        {
            // Build the NHibernate ISessionFactory object
            var sessionFactory = FluentNHibernate
                .Cfg.Fluently.Configure()
                .Database(
                    MsSqlConfiguration.MsSql2008.ConnectionString(
                        c => c.FromConnectionStringWithKey("Mvc4ServicesDb")))
                .CurrentSessionContext("web")
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<SqlCommandFactory>())
                .BuildSessionFactory();

            // Add the ISessionFactory instance to the container
            container.Bind<ISessionFactory>().ToConstant(sessionFactory);

            // Configure a resolver method to be used for creating ISession objects
            container.Bind<ISession>().ToMethod(CreateSession);

            container.Bind<ICurrentSessionContextAdapter>().To<CurrentSessionContextAdapter>();
        }

        /// <summary>
        /// Method used to create instances of ISession objects
        /// and bind them to the HTTP context.
        /// </summary>
        private ISession CreateSession(IContext context)
        {
            var sessionFactory = context.Kernel.Get<ISessionFactory>();
            if (!CurrentSessionContext.HasBind(sessionFactory))
            {
                // Open new ISession and bind it to the current session context
                var session = sessionFactory.OpenSession();
                CurrentSessionContext.Bind(session);
            }

            return sessionFactory.GetCurrentSession();
        }
    }
}