using FluentNHibernate.Mapping;

namespace MVC4ServicesBook.Data.SqlServer.Mapping
{
    public abstract class VersionedClassMap<T> : ClassMap<T> where T : IVersionedModelObject
    {
        protected VersionedClassMap()
        {
            Version(x => x.Version).Column("ts").CustomSqlType("Rowversion").Generated.Always().UnsavedValue("null");
        }
    }
}