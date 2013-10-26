using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using MVC4ServicesBook.Common;
using Moq;
using NUnit.Framework;
using log4net;

namespace MVC4ServicesBook.Web.Common.Tests
{
    [TestFixture]
    public class ActionExceptionHandlerTests
    {
        private Mock<ILog> _loggerMock;
        private Mock<IExceptionMessageFormatter> _exceptionFormatterMock;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILog>(MockBehavior.Strict);
            _exceptionFormatterMock = new Mock<IExceptionMessageFormatter>(MockBehavior.Strict);
        }

        [Test]
        public void HandleException_does_nothing_if_no_exception()
        {
            var handler = new ActionExceptionHandler(_loggerMock.Object, _exceptionFormatterMock.Object);

            var context = new HttpActionExecutedContext {Exception = null};

            handler.HandleException(context);

            Assert.IsFalse(handler.ExceptionHandled);
        }

        [Test]
        public void HandleException_exception_logged()
        {
            var handler = new ActionExceptionHandler(_loggerMock.Object, _exceptionFormatterMock.Object);

            
            const string reasonPhrase = "dumb thing don't work";
            var exception = new Exception(reasonPhrase);

            var actionContext = new HttpActionContext { Response = new HttpResponseMessage() };
            var context = new HttpActionExecutedContext(actionContext, exception);

            _loggerMock
                .Setup(x => x.Error(It.IsAny<string>(), exception));
            _exceptionFormatterMock
                .Setup(x => x.GetEntireExceptionStack(exception))
                .Returns(reasonPhrase);

            handler.HandleException(context);

            _loggerMock.VerifyAll();
        }

        [Test]
        public void HandleException_trims_message_if_too_long()
        {
            var handler = new ActionExceptionHandler(_loggerMock.Object, _exceptionFormatterMock.Object);


            var reasonPhrase = "".PadRight(ActionExceptionHandler.MaxStatusDescriptionLength, 'z');
            reasonPhrase += "a";

            var exception = new Exception(reasonPhrase);

            var actionContext = new HttpActionContext { Response = new HttpResponseMessage() };
            var context = new HttpActionExecutedContext(actionContext, exception);

            _loggerMock
                .Setup(x => x.Error(It.IsAny<string>(), exception));
            _exceptionFormatterMock
                .Setup(x => x.GetEntireExceptionStack(exception))
                .Returns(reasonPhrase);

            handler.HandleException(context);

            var actualReasonPhrase = actionContext.Response.ReasonPhrase;
            Assert.AreEqual(ActionExceptionHandler.MaxStatusDescriptionLength, actualReasonPhrase.Length);
            Assert.That(actualReasonPhrase, !Contains.Item('a'));
        }

        [Test]
        public void HandleException_removes_newline()
        {
            var handler = new ActionExceptionHandler(_loggerMock.Object, _exceptionFormatterMock.Object);


            var reasonPhrase = "one" + Environment.NewLine + "two";

            var exception = new Exception(reasonPhrase);

            var actionContext = new HttpActionContext { Response = new HttpResponseMessage() };
            var context = new HttpActionExecutedContext(actionContext, exception);

            _loggerMock
                .Setup(x => x.Error(It.IsAny<string>(), exception));
            _exceptionFormatterMock
                .Setup(x => x.GetEntireExceptionStack(exception))
                .Returns(reasonPhrase);

            handler.HandleException(context);

            var actualReasonPhrase = actionContext.Response.ReasonPhrase;
            Assert.AreEqual(actualReasonPhrase, "one two");
        }
    }
}
