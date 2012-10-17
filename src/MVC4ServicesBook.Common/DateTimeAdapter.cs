using System;

namespace MVC4ServicesBook.Common
{
    public class DateTimeAdapter : IClock
    {
        public DateTime UtcNow
        {
            get { return DateTime.UtcNow; }
        }
    }
}
