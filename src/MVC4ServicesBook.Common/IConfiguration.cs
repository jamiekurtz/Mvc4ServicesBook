namespace MVC4ServicesBook.Common
{
    public interface IConfiguration
    {
        string GetAppSetting(string key);
    }
}