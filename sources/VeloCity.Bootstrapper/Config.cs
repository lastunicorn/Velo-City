using Microsoft.Extensions.Configuration;

namespace DustInTheWind.VeloCity.Bootstrapper
{
    internal class Config
    {
        private readonly IConfiguration config;

        public bool DebugVerbose
        {
            get
            {
                IConfigurationSection debugConfigurationSection = config.GetSection("Debug");

                if (debugConfigurationSection.Exists())
                {
                    IConfigurationSection verboseConfigurationSection = debugConfigurationSection.GetSection("Verbose");

                    if (verboseConfigurationSection.Exists())
                    {
                        string rawValue = verboseConfigurationSection.Value;
                        bool success = bool.TryParse(rawValue, out bool value);

                        return success && value;
                    }
                }

                return false;
            }
        }

        public Config()
        {
            config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }
    }
}