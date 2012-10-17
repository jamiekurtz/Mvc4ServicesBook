using System;

namespace MVC4ServicesBook.Common
{
    public interface IClock
    {
        DateTime UtcNow { get; }
    }
}