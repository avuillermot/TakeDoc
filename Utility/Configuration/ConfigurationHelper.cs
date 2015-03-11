using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility.Configuration
{
    public static class ConfigurationHelper
    {
        public static string GetAppSettings(string key)
        {
            if (System.Configuration.ConfigurationManager.AppSettings[key] != null)
                return System.Configuration.ConfigurationManager.AppSettings[key].ToString();
            return string.Empty;
        }
    }
}
