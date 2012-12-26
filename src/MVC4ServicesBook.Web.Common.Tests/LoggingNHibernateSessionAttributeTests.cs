using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Moq;
using NUnit.Framework;

namespace MVC4ServicesBook.Web.Common.Tests
{
    [TestFixture]
    public class LoggingNHibernateSessionAttributeTests
    {
        private Mock<IActionLogHelper> _logHelperMock;
        private Mock<IActionTransactionHelper> _transactionHelperMock;
        private Mock<IActionExceptionHandler> _exceptionHandlerMock;

        [SetUp]
        public void Setup()
        {
            _logHelperMock = new Mock<IActionLogHelper>(MockBehavior.Strict);
            _transactionHelperMock = new Mock<IActionTransactionHelper>(MockBehavior.Strict);
            _exceptionHandlerMock = new Mock<IActionExceptionHandler>(MockBehavior.Strict);
        }

        [Test]
        public void OnActionExecuting_should_log_entry_and_start_transaction_in_order()
        {
            var target = CreateTarget();
            var actionContext = new HttpActionContext {ActionDescriptor = new ReflectedHttpActionDescriptor()};

            var ordinal = 0;
            var loggerOrdinal = -1;
            var transactionOrdinal = -1;

            _logHelperMock
                .Setup(x => x.LogEntry(actionContext.ActionDescriptor))
                .Callback(() => loggerOrdinal = ordinal++);
            _transactionHelperMock
                .Setup(x => x.BeginTransaction())
                .Callback(() => transactionOrdinal = ordinal++);

            target.OnActionExecuting(actionContext);

            _logHelperMock.VerifyAll();
            _transactionHelperMock.VerifyAll();

            Assert.AreEqual(0, loggerOrdinal);
            Assert.AreEqual(1, transactionOrdinal);
        }

        [Test]
        public void OnActionExecuted_should_end_transaction_log_exception_log_exit_in_order()
        {
            var target = CreateTarget();
            var actionContext = new HttpActionContext {ActionDescriptor = new ReflectedHttpActionDescriptor()};
            var actionExecutedContext = new HttpActionExecutedContext {ActionContext = actionContext};

            var ordinal = 0;
            var loggerOrdinal = -1;
            var transactionOrdinal = -1;
            var sessionOrdinal = -1;
            var exceptionOrdinal = -1;

            _transactionHelperMock
                .Setup(x => x.EndTransaction(actionExecutedContext))
                .Callback(() => transactionOrdinal = ordinal++);
            _transactionHelperMock
                .Setup(x => x.CloseSession())
                .Callback(() => sessionOrdinal = ordinal++);
            _exceptionHandlerMock
                .Setup(x => x.HandleException(actionExecutedContext))
                .Callback(() => exceptionOrdinal = ordinal++);
            _logHelperMock
                .Setup(x => x.LogExit(actionContext.ActionDescriptor))
                .Callback(() => loggerOrdinal = ordinal++);

            target.OnActionExecuted(actionExecutedContext);

            _logHelperMock.VerifyAll();
            _transactionHelperMock.VerifyAll();
            _exceptionHandlerMock.VerifyAll();

            Assert.AreEqual(0, transactionOrdinal);
            Assert.AreEqual(1, sessionOrdinal);
            Assert.AreEqual(2, exceptionOrdinal);
            Assert.AreEqual(3, loggerOrdinal);
        }

        private LoggingNHibernateSessionAttribute CreateTarget()
        {
            return new LoggingNHibernateSessionAttribute(
                _logHelperMock.Object,
                _exceptionHandlerMock.Object,
                _transactionHelperMock.Object);
        }
    }
}
