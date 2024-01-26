namespace MarbleDuels.VideoCreation {
    using MarbleDuels.Utils;
    using System;
    using System.Threading.Tasks;

    internal class GodotVideoCreator : VideoCreator {
        /// <summary>
        /// Determines whether to use godot or godot-mono.
        /// </summary>
        internal virtual bool Mono { get; set; }

        /// <summary>
        /// The key used in the configuration file assocciated with the Godot project's file path.
        /// </summary>
        internal virtual string ConfigProjectPath { get; }

        /// <param name="configProjectPath">
        /// The key used in the configuration file assocciated with the Godot project's file path.
        /// </param>
        internal GodotVideoCreator(string configProjectPath, bool mono = false) {
            ConfigProjectPath = configProjectPath;
            Mono = mono;
        }

        /// <inheritdoc/>
        /// <summary>
        /// Uses Godot's movie maker utility to create a video.
        /// </summary>
        internal override async Task<string?> CreateVideo(string outputDir, VideoCreationSettings settings) {
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
                return null;
            }

            var configMaxSeconds = Configuration.GetValue("max_video_seconds");
            if (configMaxSeconds is null) {
                return null;
            }

            int maxSeconds;
            try {
                maxSeconds = int.Parse(configMaxSeconds);
            } catch (Exception e) {
                Logger.Error(e);
                return null;
            }

            var godot = Mono ? "godot-mono" : "godot";
            var path = $"--path \"{projectPath}\"";
            var writeMovie = $"--write-movie \"{outputFilePath}\"";
            var resolution = $"--resolution {settings.ResolutionWidth}x{settings.ResolutionHeight}";
            var fixedFps = $"--fixed-fps {settings.FrameRate}";
            var quitAfter = $"--quit-after {maxSeconds * settings.FrameRate}";

            var exitCode = await Util.RunCMD($"{godot} {path} {writeMovie} {resolution} {fixedFps} {quitAfter}");

            if (exitCode != 0) {
                Logger.Warn("Godot did not exit cleanly.");
                return null;
            }

            return outputFilePath;
        }
    }
}
