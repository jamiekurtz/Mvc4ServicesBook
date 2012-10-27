using FluentNHibernate.Mapping;
using MVC4ServicesBook.Data.Model;

namespace MVC4ServicesBook.Data.SqlServer.Mapping
{
    public class StatusMap : ClassMap<Status>
    {
        public StatusMap()
        {
            Id(x => x.StatusId);
            Map(x => x.Name).Not.Nullable();
            Map(x => x.Ordinal).Not.Nullable();
            Version(x => x.Version).Column("ts").CustomSqlType("Rowversion").Generated.Always().UnsavedValue("null");
        }
    }
}