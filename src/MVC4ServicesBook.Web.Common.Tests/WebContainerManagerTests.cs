using System.Web.Http;
using System.Web.Http.Dependencies;
using Moq;
using NUnit.Framework;

namespace MVC4ServicesBook.Web.Common.Tests
{
    [TestFixture]
    public class WebContainerManagerTests
    {
        private Mock<IDependencyResolver> mockDependencyResolver;

        [SetUp]
        public void Setup()
        {
            mockDependencyResolver = new Mock<IDependencyResolver>();
            mockDependencyResolver.Setup(x => x.GetService(typeof(ITestableInterface))).Returns(new TestableClass());
            GlobalConfiguration.Configuration.DependencyResolver = mockDependencyResolver.Object;
        }

        [TearDown]
        public void Teardown()
        {
            mockDependencyResolver = null;
        }

        [Test]
        public void GetContainer_Returns_Current_Dependency_Resolver()
        {
            IDependencyResolver dependencyResolver = WebContainerManager.GetContainer();
            Assert.IsInstanceOf<IDependencyResolver>(dependencyResolver);
        }

        [Test]
        public void Get_Returns_Instance_Of_Class()
        {
            ITestableInterface testable = WebContainerManager.Get<ITestableInterface>();
            Assert.IsInstanceOf<TestableClass>(testable);
        }

        public interface ITestableInterface { }

        public class TestableClass : ITestableInterface { }
    }
}
