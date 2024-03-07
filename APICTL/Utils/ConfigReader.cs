namespace APICTL.Utils
{
    public class ConfigReader
    {
        private static IConfiguration Config { get; set; }

        static ConfigReader()
        {
            Config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json")
                .Build();
        }

        public static string Get(string key)
        {
            return Config[key];
        }
    }
}
