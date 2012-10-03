namespace MVC4ServicesBook.Data.Model
{
    public class User
    {
        public virtual long UserId { get; set; }
        public virtual string Username { get; set; }
        public virtual string Firstname { get; set; }
        public virtual string Lastname { get; set; }
        public virtual string Email { get; set; }
        public virtual byte[] Timestamp { get; set; }
    }
}
