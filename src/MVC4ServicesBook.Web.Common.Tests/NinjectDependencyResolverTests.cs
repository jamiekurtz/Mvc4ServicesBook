using System.Linq;
using MVC4ServicesBook.Common;
using Moq;
using NUnit.Framework;
using Ninject;

namespace MVC4ServicesBook.Web.Common.Tests
{
    [TestFixture]
    public class NinjectDependencyResolverTests
    {
        [Test]
        public void GetService_returns_null_when_type_not_registered()
        {
            var kernel = new StandardKernel();
            var resolver = new NinjectDependencyResolver(kernel);

            var result = resolver.GetService(typeof (IDateTime));
            Assert.IsNull(result);
        }

        [Test]
        public void GetService_returns_instance_when_registered()
        {
            var kernel = new StandardKernel();
            var resolver = new NinjectDependencyResolver(kernel);
            kernel.Bind<IDateTime>().To<DateTimeAdapter>();

            var result = resolver.GetService(typeof(IDateTime));
            Assert.That(result, Is.TypeOf<DateTimeAdapter>());            
        }

        [Test]
        public void GetServices_returns_empty_collection_when_none_registered()
        {
            var kernel = new StandardKernel();
            var resolver = new NinjectDependencyResolver(kernel);

            var result = resolver.GetServices(typeof(IDateTime));
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count()); 
        }    
        
        [Test]
        public void GetServices_returns_multiple_instances_when_some_registered()
        {
            var kernel = new StandardKernel();
            var resolver = new NinjectDependencyResolver(kernel);

            kernel.Bind<IDateTime>().To<DateTimeAdapter>();
            kernel.Bind<IDateTime>().ToConstant(new Mock<IDateTime>().Object);

            var result = resolver.GetServices(typeof(IDateTime));
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count()); 
        }

        [Test]
        public void BeginScope_just_returns_this()
        {
            var kernel = new StandardKernel();
            var resolver = new NinjectDependencyResolver(kernel);

            Assert.AreSame(resolver, resolver.BeginScope());
        }
    }
}
