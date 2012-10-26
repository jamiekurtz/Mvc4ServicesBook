using System.Security.Principal;
using System.Threading;
using System.Web.Http;
using MVC4ServicesBook.Common;
using MVC4ServicesBook.Data;
using MVC4ServicesBook.Data.SqlServer;
using MVC4ServicesBook.Web.Common;
using MVC4ServicesBook.Web.Common.Security;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using Ninject;
using Ninject.Activation;
using log4net;
using Ninject.Web.Common;

namespace MVC4ServicesBook.Web.Api.App_Start
{
    public class NinjectConfigurator
    {
        public void Configure(IKernel container)
        {
            AddBindings(container);

            var ninjectControllerFactory = new NinjectDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = ninjectControllerFactory;
        }

        private void AddBindings(IKernel container)
        {
            ConfigureNHibernate(container);

            log4net.Config.XmlConfigurator.Configure();
            var loggerForWebSite = LogManager.GetLogger("Mvc4ServicesBookWebsite");
            container.Bind<ILog>().ToConstant(loggerForWebSite);

            container.Bind<IDateTime>().To<DateTimeAdapter>();
            container.Bind<IDatabaseValueParser>().To<DatabaseValueParser>();

            container.Bind<IHttpTaskFetcher>().To<HttpTaskFetcher>();
            container.Bind<IUserManager>().To<UserManager>();
            container.Bind<IMembershipAdapter>().To<MembershipAdapter>();

            container.Bind<ISqlCommandFactory>().To<SqlCommandFactory>();
            container.Bind<ICommonRepository>().To<CommonRepository>();
            container.Bind<IUserRepository>().To<UserRepository>();

            container.Bind<IUserSession>().ToMethod(CreateUserSession).InRequestScope();
        }

        private IUserSession CreateUserSession(IContext arg)
        {
            return new UserSession(Thread.CurrentPrincipal as GenericPrincipal);
        }

        private void ConfigureNHibernate(IKernel container)
        {
            var sessionFactory = new Configuration().Configure().BuildSessionFactory();
            container.Bind<ISessionFactory>().ToConstant(sessionFactory);
            container.Bind<ISession>().ToMethod(CreateSession); //.InRequestScope();
        }

        private ISession CreateSession(IContext context)
        {
            //var sessionFactory = context.Kernel.Get<ISessionFactory>();
            //return sessionFactory.OpenSession();

            var sessionFactory = context.Kernel.Get<ISessionFactory>();
            if (!CurrentSessionContext.HasBind(sessionFactory))
            {
                var session = sessionFactory.OpenSession();
                CurrentSessionContext.Bind(session);
            }

            return sessionFactory.GetCurrentSession();
        }
    }
}