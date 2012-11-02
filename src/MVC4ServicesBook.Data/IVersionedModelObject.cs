namespace MVC4ServicesBook.Data
{
    public interface IVersionedModelObject
    {
        byte[] Version { get; set; }
    }
}