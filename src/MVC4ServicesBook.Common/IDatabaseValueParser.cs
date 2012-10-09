using System;

namespace MVC4ServicesBook.Common
{
    public interface IDatabaseValueParser
    {
        Guid ParseGuid(object obj);
        long ParseLong(object obj);
        DateTime ParseDateTime(object obj);
        Guid? ParseGuidNullable(object obj);
        long? ParseLongNullable(object obj);
        DateTime? ParseDateTimeNullable(object obj);
        string ParseString(object obj);
        byte[] ParseByteArray(object obj);
    }
}
