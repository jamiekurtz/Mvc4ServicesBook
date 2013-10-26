using System;
using NUnit.Framework;

namespace MVC4ServicesBook.Common.Tests
{
    [TestFixture]
    public class ExceptionMessageFormatterTests
    {
        [Test]
        public void GetEntireExceptionStack_returns_single_message_when_only_one_exception()
        {
            var formatter = new ExceptionMessageFormatter();

            var exception = new Exception("test");

            var result = formatter.GetEntireExceptionStack(exception);

            Assert.AreEqual(exception.Message, result);
        }

        [Test]
        public void GetEntireExceptionStack_returns_all_messages_when_three_exceptions()
        {
            var formatter = new ExceptionMessageFormatter();

            var exception3 = new Exception("three");
            var exception2 = new Exception("two", exception3);
            var exception1 = new Exception("one", exception2);

            var result = formatter.GetEntireExceptionStack(exception1);

            Assert.AreEqual("one --> two --> three", result);
        }
    }
}
