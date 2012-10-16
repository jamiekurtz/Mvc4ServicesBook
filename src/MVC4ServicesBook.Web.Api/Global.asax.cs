using System.Web.Http;
using System.Web.Mvc;
//using System.Web.Optimization;
using System.Web.Routing;
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

namespace MVC4ServicesBook.Web.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);

            RegisterContainer();
        }

        private void RegisterContainer()
        {
            var container = new StandardKernel();

            AddBindings(container);

            var ninjectControllerFactory = new NinjectDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = ninjectControllerFactory;
        }

        private void AddBindings(StandardKernel container)
        {
            ConfigureNHibernate(container);

            log4net.Config.XmlConfigurator.Configure();
            var loggerForWebSite = LogManager.GetLogger("Mvc4ServicesBookWebsite");
            container.Bind<ILog>().ToConstant(loggerForWebSite);

            //container.Bind<IObjectLogger>().To<ObjectLogger>();
            //container.Bind<IFileAdapter>().To<FileAdapter>();
            //container.Bind<IDirectoryAdapter>().To<DirectoryAdapter>();
            //container.Bind<ICurrentDateTime>().To<CurrentDateTime>();
            //container.Bind<IDateTimeHelper>().To<DateTimeHelper>();
            //container.Bind<IHtmlSanitizer>().To<HtmlSanitizer>();

            container.Bind<IDatabaseValueParser>().To<DatabaseValueParser>();

            container.Bind<IHttpTaskFetcher>().To<HttpTaskFetcher>();
            container.Bind<IUserManager>().To<UserManager>();
            container.Bind<IMembershipAdapter>().To<MembershipAdapter>();

            container.Bind<ISqlCommandFactory>().To<SqlCommandFactory>();
            container.Bind<ICommonRepository>().To<CommonRepository>();
            container.Bind<IUserRepository>().To<UserRepository>();
            
//            container.Bind<IUserSession>().ToMethod(GetCurrentUser);
        }

        private void ConfigureNHibernate(IKernel container)
        {
            var sessionFactory = new Configuration().Configure().BuildSessionFactory();
            container.Bind<ISessionFactory>().ToConstant(sessionFactory);

            container.Bind<ISession>().ToMethod(CreateSession);
        }

        //private IUserSession GetCurrentUser(IContext context)
        //{
        //    var principal = HttpContext.Current.User as StoreFlixPrincipal;
        //    if (principal != null)
        //    {
        //        var identity = principal.Identity as StoreFlixIdentity;
        //        if (identity != null)
        //        {
        //            var userSession = new UserSession(identity.UserId, identity.Username, principal.Roles);
        //            return userSession;
        //        }
        //    }

        //    return null;
        //}

        private ISession CreateSession(IContext context)
        {
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