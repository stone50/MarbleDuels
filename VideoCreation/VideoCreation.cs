namespace MarbleDuels.VideoCreation {
    using System;
    using System.Threading.Tasks;

    internal static class VideoCreation {
        internal class VideoCreationResults {
            internal string VideoFilePath = "";
            internal string VideoDirPath = "";
        }

        /// <summary>
        /// Creates a new directory for videoCreator, then creates a new video using videoCreator.
        /// </summary>
        /// <remarks>
        /// The created directory will be deleted if video creation is not successful.
        /// </remarks>
        /// <returns>
        /// A VideoCreationResults object, or null if creation failed.
        /// </returns>
        internal static async Task<VideoCreationResults?> CreateVideo(VideoCreator videoCreator, VideoCreationSettings settings) {
            Logger.Info($"Creating '{settings}' video using '{videoCreator.GetType().Name}' creator.");

            if (VideoCreator.VideoDir is null) {
                Logger.Warn("Video directory is null.");
                return null;
            }

            string videoWorkingDirPath;
            try {
                videoWorkingDirPath = Path.Combine(VideoCreator.VideoDir, Path.GetRandomFileName());
                _ = Directory.CreateDirectory(videoWorkingDirPath);
            } catch (Exception e) {
                Logger.Error(e);
                return null;
            }

            var videoFilePath = await videoCreator.CreateVideo(videoWorkingDirPath, settings);
            if (videoFilePath is null) {
                Logger.Warn($"Could not create video in \"{videoWorkingDirPath}\".");

                try {
                    Directory.Delete(videoWorkingDirPath, true);
                } catch (Exception e) {
                    Logger.Error(e);
                }

                return null;
            }

            Logger.Info($"Created '{settings}' video using '{videoCreator.GetType().Name}' creator at \"{videoWorkingDirPath}\".");
            return new() {
                VideoFilePath = videoFilePath,
                VideoDirPath = videoWorkingDirPath
            };
        }
    }
}
