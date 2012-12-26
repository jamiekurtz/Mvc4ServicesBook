using MVC4ServicesBook.Data.Model;

namespace MVC4ServicesBook.Data.SqlServer.Mapping
{
    public class PriorityMap : VersionedClassMap<Priority>
    {
        public PriorityMap()
        {
            Id(x => x.PriorityId);
            Map(x => x.Name).Not.Nullable();
            Map(x => x.Ordinal).Not.Nullable();
        }
    }
}