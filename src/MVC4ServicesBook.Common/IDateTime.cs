using System;

namespace MVC4ServicesBook.Common
{
    public interface IDateTime
    {
        DateTime UtcNow { get; }
    }
}