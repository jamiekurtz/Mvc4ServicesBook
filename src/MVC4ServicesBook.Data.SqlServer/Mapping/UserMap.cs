using System;
using FluentNHibernate.Mapping;
using MVC4ServicesBook.Data.Model;

namespace MVC4ServicesBook.Data.SqlServer.Mapping
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Table("AllUsers");

            Id(x => x.UserId).CustomType<Guid>();
            Map(x => x.Firstname).Not.Nullable();
            Map(x => x.Lastname).Not.Nullable();
            Map(x => x.Email).Nullable();
            Version(x => x.Version).Column("ts").CustomSqlType("Rowversion").Generated.Always().UnsavedValue("null");
        }
    }
}