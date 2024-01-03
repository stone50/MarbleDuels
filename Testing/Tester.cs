namespace MarbleDuels.Testing {
    using Google.Apis.YouTube.v3.Data;
    using MarbleDuels.VideoCreation;
    using System.Threading.Tasks;

    internal class Tester {
        internal VideoCreator VideoCreator { get; }
        internal VideoCreationSettings VideoCreationSettings { get; }
        internal string VideoCreatorName { get; }
        internal string VideoCreationSettingsDescription { get; }

        // TODO: add videos to Testing playlist

        internal Tester(VideoCreator videoCreator, VideoCreationSettings videoCreationSettings) {
            VideoCreator = videoCreator;
            VideoCreationSettings = videoCreationSettings;
            VideoCreatorName = VideoCreator.GetType().Name;
            VideoCreationSettingsDescription = $"{VideoCreationSettings.ResolutionWidth}x{VideoCreationSettings.ResolutionHeight} {VideoCreationSettings.FrameRate}fps";
        }

        internal async Task<VideoCreation.VideoCreationResults?> CreateVideo() {
            return await VideoCreation.CreateVideo(
                VideoCreator,
                VideoCreationSettings
            );
        }

        internal async Task<VideoCreation.VideoCreationAndUploadResults> CreateAndUpload() {
            return await VideoCreation.CreateAndUpload(
                VideoCreator,
                VideoCreationSettings,
                new() {
                    Snippet = new() {
                        Title = $"{VideoCreatorName} Single Upload",
                        Description = VideoCreationSettingsDescription
                    },
                    Status = new VideoStatus {
                        PrivacyStatus = "private"
                    }
                }
            );
        }

        internal Task<VideoCreation.VideoCreationResults?>[] CreateMultipleVideos(int numVideos) {
            var tasks = new Task<VideoCreation.VideoCreationResults?>[numVideos];
            for (var i = 0; i < numVideos; i++) {
                tasks[i] = VideoCreation.CreateVideo(
                    VideoCreator,
                    VideoCreationSettings
                );
            }

            return tasks;
        }

        internal Task<VideoCreation.VideoCreationAndUploadResults>[] CreateAndUploadMultiple(int numVideos) {
            var tasks = new Task<VideoCreation.VideoCreationAndUploadResults>[numVideos];
            for (var i = 0; i < numVideos; i++) {
                tasks[i] = VideoCreation.CreateAndUpload(
                    VideoCreator,
                    VideoCreationSettings,
                    new() {
                        Snippet = new() {
                            Title = $"{VideoCreatorName} Multi Upload {i}",
                            Description = VideoCreationSettingsDescription
                        },
                        Status = new VideoStatus {
                            PrivacyStatus = "private"
                        }
                    }
                );
            }

            return tasks;
        }
    }
}
