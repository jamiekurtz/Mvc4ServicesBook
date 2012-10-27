using FluentNHibernate.Mapping;
using MVC4ServicesBook.Data.Model;

namespace MVC4ServicesBook.Data.SqlServer.Mapping
{
    public class CategoryMap : ClassMap<Category>
    {
        public CategoryMap()
        {
            Id(x => x.CategoryId);
            Map(x => x.Name).Not.Nullable();
            Map(x => x.Description).Nullable();
            Version(x => x.Version).Column("ts").CustomSqlType("Rowversion").Generated.Always().UnsavedValue("null");
        }
    }
}
