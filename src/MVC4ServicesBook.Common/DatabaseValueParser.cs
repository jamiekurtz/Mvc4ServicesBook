using System;

namespace MVC4ServicesBook.Common
{
    public class DatabaseValueParser : IDatabaseValueParser
    {
        public Guid ParseGuid(object obj)
        {
            return Guid.Parse(obj.ToString());
        }

        public long ParseLong(object obj)
        {
            return long.Parse(obj.ToString());
        }

        public DateTime ParseDateTime(object obj)
        {
            return DateTime.Parse(obj.ToString());
        }

        public Guid? ParseGuidNullable(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return null;
            }

            return ParseGuid(obj);
        }

        public long? ParseLongNullable(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return null;
            }

            return ParseLong(obj);
        }

        public DateTime? ParseDateTimeNullable(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return null;
            }

            return ParseDateTime(obj);
        }

        public string ParseString(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return null;
            }

            return obj.ToString();
        }

        public byte[] ParseByteArray(object obj)
        {
            if(obj == null || obj == DBNull.Value)
            {
                return null;
            }

            return (byte[]) obj;
        }
    }
}