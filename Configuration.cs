namespace MarbleDuels {
    using Microsoft.Extensions.Configuration;

    internal static class Configuration {
        internal const string ClientSecretFilePath = "client_secrets.json";
        internal const string ConfigFilePath = "config.json";

        private static IConfigurationRoot? Config;

        internal static bool Initialize() {
            Logger.Info("Initializing configuration.");

            try {
                Config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile(ConfigFilePath)
                    .Build();
            } catch (Exception e) {
                Logger.Error(e);
                return false;
            }

            Logger.Info("Done initializing configuration.");
            return true;
        }

        internal static IConfigurationSection? GetSection(string sectionName) {
            Logger.Info($"Getting configuration section {sectionName}");

            if (Config is null) {
                Logger.Warn("Configuration not initialized.");
                return null;
            }

            Logger.Info($"Done getting configuration section {sectionName}.");
            return Config.GetSection(sectionName);
        }

        internal static string? GetValue(string sectionName) {
            Logger.Info($"Getting configuration value {sectionName}.");

            var section = GetSection(sectionName);
            if (section is null) {
                Logger.Warn($"Could not find section {sectionName}.");
                return null;
            }

            Logger.Info($"Done getting configuration value {sectionName}.");
            return section.Value;
        }

        internal static string? GetValueFromPath(string path) {
            Logger.Info($"Getting configuration value from path \"{path}\"");

            if (Config is null) {
                Logger.Warn("Configuration not initialized.");
                return null;
            }

            var sections = path.Split(".");
            IConfiguration currentSection = Config;
            for (var i = 0; i < sections.Length - 1; i++) {
                var nextSection = currentSection.GetSection(sections[i]);

                if (nextSection is null) {
                    Logger.Warn($"Could not find section \"{sections[i]}\" in \"{path}\"");
                    return null;
                }

                currentSection = nextSection;
            }

            var value = currentSection.GetSection(sections.Last()).Value;
            if (value is null) {
                Logger.Warn($"Could not find value \"{sections.Last()}\" in \"{path}\"");
                return null;
            }

            Logger.Info($"Done getting configuration value from path \"{path}\"");
            return value;
        }
    }
}
