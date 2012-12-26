using System;
using System.Web.Http.Filters;
using Moq;
using NHibernate;
using NUnit.Framework;

namespace MVC4ServicesBook.Web.Common.Tests
{
    [TestFixture]
    public class ActionTransactionHelperTests
    {
        private Mock<ISessionFactory> _sessionFactoryMock;
        private Mock<ISession> _sessionMock;
        private Mock<ITransaction> _transactionMock;
        private Mock<ICurrentSessionContextAdapter> _contextAdapterMock;

        [SetUp]
        public void Setup()
        {
            _sessionFactoryMock = new Mock<ISessionFactory>(MockBehavior.Strict);
            _sessionMock = new Mock<ISession>(MockBehavior.Strict);
            _transactionMock = new Mock<ITransaction>(MockBehavior.Strict);
            _contextAdapterMock = new Mock<ICurrentSessionContextAdapter>(MockBehavior.Strict);
        }

        [Test]
        public void BeginTransaction_starts_transaction_on_session()
        {
            var helper = CreateHelper();

            _sessionFactoryMock
                .Setup(x => x.GetCurrentSession())
                .Returns(_sessionMock.Object);

            _sessionMock
                .Setup(x => x.BeginTransaction())
                .Returns(_transactionMock.Object);

            helper.BeginTransaction();

            _sessionMock.VerifyAll();
        }

        private ActionTransactionHelper CreateHelper()
        {
            var helper = new ActionTransactionHelper(_sessionFactoryMock.Object, _contextAdapterMock.Object);
            return helper;
        }

        [Test]
        public void BeginTransaction_does_not_blow_it_no_active_session()
        {
            var helper = CreateHelper();

            _sessionFactoryMock
                .Setup(x => x.GetCurrentSession())
                .Returns(value: null);

            Assert.DoesNotThrow(helper.BeginTransaction);
        }

        [Test]
        public void EndTransaction_transaction_not_handled_when_no_active_session()
        {
            var helper = CreateHelper();

            var context = new HttpActionExecutedContext();

            _sessionFactoryMock
                .Setup(x => x.GetCurrentSession())
                .Returns(value: null);

            helper.EndTransaction(context);

            Assert.IsFalse(helper.TransactionHandled);
        }

        [Test]
        public void EndTransaction_transaction_not_handled_when_no_active_transaction()
        {
            var helper = CreateHelper();

            var context = new HttpActionExecutedContext();

            var transactionMock = new Mock<ITransaction>();
            transactionMock
                .Setup(x => x.IsActive)
                .Returns(false);

            _sessionMock
                .Setup(x => x.Transaction)
                .Returns(transactionMock.Object);

            _sessionFactoryMock
                .Setup(x => x.GetCurrentSession())
                .Returns(_sessionMock.Object);

            helper.EndTransaction(context);

            Assert.IsFalse(helper.TransactionHandled);
        }

        [Test]
        public void EndTransaction_flush_and_commit_when_no_exception()
        {
            var helper = CreateHelper();

            var context = new HttpActionExecutedContext();

            _transactionMock
                .Setup(x => x.IsActive)
                .Returns(true);

            _sessionMock
                .Setup(x => x.Transaction)
                .Returns(_transactionMock.Object);

            _sessionFactoryMock
                .Setup(x => x.GetCurrentSession())
                .Returns(_sessionMock.Object);

            _sessionMock.Setup(x => x.Flush());

            _transactionMock.Setup(x => x.Commit());

            helper.EndTransaction(context);

            _sessionMock.VerifyAll();
            _transactionMock.VerifyAll();
        }

        [Test]
        public void EndTransaction_rollback_when_exception_exists()
        {
            var helper = CreateHelper();

            var context = new HttpActionExecutedContext {Exception = new Exception()};

            var transactionMock = new Mock<ITransaction>();
            transactionMock
                .Setup(x => x.IsActive)
                .Returns(true);

            _sessionMock
                .Setup(x => x.Transaction)
                .Returns(transactionMock.Object);

            _sessionFactoryMock
                .Setup(x => x.GetCurrentSession())
                .Returns(_sessionMock.Object);

            helper.EndTransaction(context);

            transactionMock.Verify(x => x.Rollback());
        }

        [Test]
        public void CloseSession_should_do_nothing_when_no_binding_exists()
        {
            var helper = CreateHelper();

            _contextAdapterMock
                .Setup(x => x.HasBind(_sessionFactoryMock.Object))
                .Returns(false);

            helper.CloseSession();

            Assert.IsFalse(helper.SessionClosed);
        }  
        
        [Test]
        public void CloseSession_should_close_dispose_and_unbind_when_binding_exists()
        {
            var helper = CreateHelper();

            _contextAdapterMock
                .Setup(x => x.HasBind(_sessionFactoryMock.Object))
                .Returns(true);

            _sessionFactoryMock
                .Setup(x => x.GetCurrentSession())
                .Returns(_sessionMock.Object);

            _sessionMock.Setup(x => x.Close()).Returns(value: null);
            _sessionMock.Setup(x => x.Dispose());

            _contextAdapterMock
                .Setup(x => x.Unbind(_sessionFactoryMock.Object))
                .Returns(value: null);

            helper.CloseSession();

            _contextAdapterMock.VerifyAll();
            _sessionMock.VerifyAll();
        }
    }
}
