using FluentNHibernate.Mapping;
using MVC4ServicesBook.Data.Model;

namespace MVC4ServicesBook.Data.SqlServer.Mapping
{
    public class TaskMap : ClassMap<Task>
    {
        public TaskMap()
        {
            Id(x => x.TaskId);
            Map(x => x.Subject).Not.Nullable();
            Map(x => x.StartDate).Nullable();
            Map(x => x.DueDate).Nullable();
            Map(x => x.DateCompleted).Nullable();
            Version(x => x.Version).Column("ts").CustomSqlType("Rowversion").Generated.Always().UnsavedValue("null");

            References(x => x.Status, "StatusId");
            References(x => x.Priority, "PriorityId");
            References(x => x.CreatedBy, "CreatedUserId");

            HasManyToMany(x => x.Users)
                .Access.ReadOnlyPropertyThroughCamelCaseField(Prefix.Underscore)
                .Table("TaskUser")
                .ParentKeyColumn("TaskId")
                .ChildKeyColumn("UserId");

            HasManyToMany(x => x.Categories)
                .Access.ReadOnlyPropertyThroughCamelCaseField(Prefix.Underscore)
                .Table("TaskCategory")
                .ParentKeyColumn("TaskId")
                .ChildKeyColumn("CategoryId");
        }
    }
}