using System.Web.Http.Controllers;
using Moq;
using NUnit.Framework;
using log4net;

namespace MVC4ServicesBook.Web.Common.Tests
{
    [TestFixture]
    public class ActionLogHelperTests
    {
        private Mock<ILog> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILog>();
        }

        [Test]
        public void LogAction_should_log_with_format_string()
        {
            var helper = new ActionLogHelper(_loggerMock.Object);

            const string prefix = "pre";

            var controllerDescriptor = new HttpControllerDescriptor {ControllerType = typeof (ActionLogHelper)};
            var actionDescriptor = new ReflectedHttpActionDescriptor {ControllerDescriptor = controllerDescriptor};

            helper.LogAction(actionDescriptor, prefix);

            _loggerMock.Verify(x => x.DebugFormat(ActionLogHelper.LogTextFormatString, prefix, typeof(ActionLogHelper).FullName, null));
        }

        [Test]
        public void LogEntry_should_log_with_entry_text()
        {
            var helper = new ActionLogHelperTestDouble(_loggerMock.Object);

            var controllerDescriptor = new HttpControllerDescriptor { ControllerType = typeof(ActionLogHelper) };
            var actionDescriptor = new ReflectedHttpActionDescriptor { ControllerDescriptor = controllerDescriptor };

            helper.LogEntry(actionDescriptor);

            Assert.AreEqual(actionDescriptor, helper.ActionDescriptorGiven);
            Assert.AreEqual(ActionLogHelper.EnteringText, helper.PrefixGiven);
        }

        [Test]
        public void LogExit_should_log_with_exit_text()
        {
            var helper = new ActionLogHelperTestDouble(_loggerMock.Object);

            var controllerDescriptor = new HttpControllerDescriptor { ControllerType = typeof(ActionLogHelper) };
            var actionDescriptor = new ReflectedHttpActionDescriptor { ControllerDescriptor = controllerDescriptor };

            helper.LogExit(actionDescriptor);

            Assert.AreEqual(actionDescriptor, helper.ActionDescriptorGiven);
            Assert.AreEqual(ActionLogHelper.ExitingText, helper.PrefixGiven);
        }

        public class ActionLogHelperTestDouble : ActionLogHelper
        {
            public ActionLogHelperTestDouble(ILog logger)
                : base(logger)
            {
            }

            public HttpActionDescriptor ActionDescriptorGiven { get; set; }
            public string PrefixGiven { get; set; }

            public override void LogAction(HttpActionDescriptor actionDescriptor, string prefix)
            {
                ActionDescriptorGiven = actionDescriptor;
                PrefixGiven = prefix;
            }
        }
    }
}
