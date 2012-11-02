namespace MVC4ServicesBook.Data.Model
{
    public class Priority : IVersionedModelObject
    {
        public virtual long PriorityId { get; set; }
        public virtual string Name { get; set; }
        public virtual int Ordinal { get; set; }
        public virtual byte[] Version { get; set; }
    }
}