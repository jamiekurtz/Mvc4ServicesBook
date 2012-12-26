using System;

namespace MVC4ServicesBook.Common
{
    public interface IExceptionMessageFormatter
    {
        string GetEntireExceptionStack(Exception ex);
    }
}