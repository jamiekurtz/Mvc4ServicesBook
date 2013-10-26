using System.Configuration;

namespace MVC4ServicesBook.Common
{
    public class ConfigurationSettingsAdapter : IConfiguration
    {
        public string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}