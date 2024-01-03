namespace MarbleDuels.VideoCreation {
    using System;
    using System.Threading.Tasks;

    internal class GodotVideoCreator : VideoCreator {
        /// <summary>
        /// The key used in the configuration file assocciated with the Godot project's file path.
        /// </summary>
        internal virtual string ConfigProjectPath { get; }

        /// <param name="configProjectPath">
        /// The key used in the configuration file assocciated with the Godot project's file path.
        /// </param>
        internal GodotVideoCreator(string configProjectPath) => ConfigProjectPath = configProjectPath;

        /// <inheritdoc/>
        /// <summary>
        /// Uses Godot's movie maker utility to create a video.
        /// </summary>
        internal override async Task<string?> CreateVideo(string outputDir, VideoCreationSettings settings) {
            Logger.Info($"Creating '{settings}' video in \"{outputDir}\".");

            var outputFileName = Path.GetRandomFileName();
            string outputFilePath;
            try {
                outputFilePath = Path.Combine(outputDir, $"{outputFileName}.avi");
            } catch (Exception e) {
                Logger.Error(e);
                return null;
            }

            var projectPath = Configuration.GetValue($"godot.{ConfigProjectPath}");
            if (projectPath is null) {
                Logger.Warn($"Could not find project path \"{ConfigProjectPath}\" in \"{Configuration.ConfigFileName}\".");
                return null;
            }

            var configMaxSeconds = Configuration.GetValue("max_video_seconds");
            if (configMaxSeconds is null) {
                Logger.Warn($"Could not find max_video_seconds in \"{Configuration.ConfigFileName}\".");
                return null;
            }

            int maxSeconds;
            try {
                maxSeconds = int.Parse(configMaxSeconds);
            } catch (Exception e) {
                Logger.Error(e);
                return null;
            }

            var godotCommand = $"godot-mono --path \"{projectPath}\" --write-movie \"{outputFilePath}\" --resolution {settings.ResolutionWidth}x{settings.ResolutionHeight} --fixed-fps {settings.FrameRate} --quit-after {maxSeconds * settings.FrameRate}";

            Logger.Info($"Executing '{godotCommand}'.");
            System.Diagnostics.Process godotProcess;
            try {
                godotProcess = System.Diagnostics.Process.Start("CMD.exe", $"/C {godotCommand}");
            } catch (Exception e) {
                Logger.Error(e);
                return null;
            }

            await godotProcess.WaitForExitAsync();

            if (godotProcess.ExitCode != 0) {
                Logger.Warn("Godot did not exit cleanly.");
                return null;
            }

            Logger.Info($"Done creating '{settings}' video at \"{outputFilePath}\".");
            return outputFilePath;
        }
    }
}
