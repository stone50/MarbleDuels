namespace MarbleDuels.VideoCreation {
    using Google.Apis.YouTube.v3.Data;
    using MarbleDuels.YTInterface;
    using System;
    using System.Threading.Tasks;

    internal static class VideoCreation {
        internal class VideoCreationResults {
            internal string VideoFilePath = "";
            internal string VideoDirPath = "";
        }

        internal class VideoCreationAndUploadResults {
            internal VideoCreationResults? CreationResults;
            internal Video? UploadedVideo;
        }

        /// <summary>
        /// Creates a new directory, then creates a new video using videoCreator.
        /// </summary>
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
                _ = Util.DeleteDir(videoWorkingDirPath);
                return null;
            }

            Logger.Info($"Created '{settings}' video using '{videoCreator.GetType().Name}' creator at \"{videoWorkingDirPath}\".");
            return new() {
                VideoFilePath = videoFilePath,
                VideoDirPath = videoWorkingDirPath
            };
        }

        /// <returns>
        /// A VideoCreationAndUploadResults object.
        /// If video creation failed, CreationResults will be null.
        /// If the upload failed, UploadedVideo will be null.
        /// </returns>
        internal static async Task<VideoCreationAndUploadResults> CreateAndUpload(VideoCreator videoCreator, VideoCreationSettings creationSettings, Video uploadSettings) {
            var videoCreationResults = await CreateVideo(videoCreator, creationSettings);
            if (videoCreationResults is null) {
                Logger.Warn($"Could not create '{creationSettings}' video using '{videoCreator.GetType().Name}' creator.");
                return new();
            }

            var uploadedVideo = await YouTubeInterface.UploadVideo(videoCreationResults.VideoFilePath, uploadSettings);
            if (uploadedVideo is null) {
                Logger.Warn($"Could not upload '{creationSettings}' video using '{videoCreator.GetType().Name}' creator.");
                return new() {
                    CreationResults = videoCreationResults
                };
            }

            return new() {
                CreationResults = videoCreationResults,
                UploadedVideo = uploadedVideo
            };
        }
    }
}
