namespace MarbleDuels.VideoCreation {

    internal abstract class VideoCreator {
        /// <summary>
        /// The root directory for generated video files.
        /// </summary>
        internal static string? VideoDir { get; private set; }

        /// <summary>
        /// Initializes the root directory for generated video files.
        /// </summary>
        /// <remarks>
        /// Creates a new directory if an existing one is not found.
        /// </remarks>
        internal static bool Initialize() {
            Logger.Info("Initializing video creation.");

            try {
                VideoDir = Path.Combine(Directory.GetCurrentDirectory(), "videos");
            } catch (Exception e) {
                Logger.Error(e);
                return false;
            }

            if (!Directory.Exists(VideoDir)) {
                try {
                    _ = Directory.CreateDirectory(VideoDir);
                } catch (Exception e) {
                    Logger.Error(e);
                    return false;
                }
            }

            Logger.Info("Done initializing video creation.");
            return true;
        }

        /// <remarks>
        /// When overriding this function, only create/edit/delete files within the given directory.
        /// </remarks>
        /// <returns>
        /// The file path of the created video, or null if creation failed.
        /// </returns>
        internal abstract Task<string?> CreateVideo(string outputDir, VideoCreationSettings settings);
    }
}
