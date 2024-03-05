using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace APICTL.Utils
{
    public class ConfigReader
    {
        private static IConfiguration Config { get; set; }

        static ConfigReader()
        {
            Config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public static string Get(string key)
        {
            return Config[key];
        }
    }
}
