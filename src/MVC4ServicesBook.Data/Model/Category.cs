namespace MVC4ServicesBook.Data.Model
{
    public class Category : IVersionedModelObject
    {
        public virtual long CategoryId { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual byte[] Version { get; set; }
    }
}