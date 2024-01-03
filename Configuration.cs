namespace MarbleDuels {
    using Microsoft.Extensions.Configuration;

    internal static class Configuration {
        internal const string ClientSecretFileName = "client_secrets.json";
        internal const string ConfigFileName = "config.json";

        private static IConfigurationRoot? Config;

        /// <summary>
        /// Prepares for reading configuration values from config.json.
        /// </summary>
        /// <returns>
        /// Whether or not initialization is successful.
        /// </returns>
        internal static bool Initialize() {
            Logger.Info("Initializing configuration.");

            try {
                Config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile(ConfigFileName)
                    .Build();
            } catch (Exception e) {
                Logger.Error(e);
                return false;
            }

            Logger.Info("Done initializing configuration.");
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">
        /// A period separated path to find the configuration value at.
        /// For example: "youtube.user" will return the 'user' property of the 'youtube' object.
        /// </param>
        /// <returns>
        /// The configuration value as a string at the given path, or null if getting the value failed.
        /// </returns>
        internal static string? GetValue(string path) {
            Logger.Info($"Getting configuration value from path '{path}'.");

            if (Config is null) {
                Logger.Warn("Configuration not initialized.");
                return null;
            }

            var sections = path.Split(".");
            IConfiguration currentSection = Config;
            for (var i = 0; i < sections.Length - 1; i++) {
                var nextSection = currentSection.GetSection(sections[i]);

                if (nextSection is null) {
                    Logger.Warn($"Could not find section '{sections[i]}' in '{path}'.");
                    return null;
                }

                currentSection = nextSection;
            }

            var value = currentSection.GetSection(sections.Last()).Value;
            if (value is null) {
                Logger.Warn($"Could not find value '{sections.Last()}' in '{path}'.");
                return null;
            }

            Logger.Info($"Done getting configuration value from path '{path}'.");
            return value;
        }
    }
}
