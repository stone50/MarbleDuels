namespace MarbleDuels.Testing {
    using Google.Apis.YouTube.v3.Data;
    using MarbleDuels.VideoCreation;
    using MarbleDuels.YTInterface;
    using System.Threading.Tasks;

    internal class Tester {
        internal VideoCreator VideoCreator { get; }
        internal VideoCreationSettings VideoCreationSettings { get; }
        internal string VideoCreatorName { get; }
        internal string VideoCreationSettingsDescription { get; }

        internal Tester(VideoCreator videoCreator, VideoCreationSettings videoCreationSettings) {
            VideoCreator = videoCreator;
            VideoCreationSettings = videoCreationSettings;
            VideoCreatorName = VideoCreator.GetType().Name;
            VideoCreationSettingsDescription = $"{VideoCreationSettings.ResolutionWidth}x{VideoCreationSettings.ResolutionHeight} {VideoCreationSettings.FrameRate}fps";
        }

        internal async Task<bool> CreateVideo() {
            return await VideoCreation.CreateVideo(
                VideoCreator,
                VideoCreationSettings
            ) is not null;
        }

        internal async Task<bool> CreateAndUpload() {
            var videoCreationResults = await VideoCreation.CreateVideo(
                VideoCreator,
                VideoCreationSettings
            );
            if (videoCreationResults is null) {
                return false;
            }

            var uploadedVideo = await YouTubeInterface.UploadVideo(
                videoCreationResults.VideoFilePath,
                new() {
                    Snippet = new() {
                        Title = VideoCreatorName,
                        Description = VideoCreationSettingsDescription
                    },
                    Status = new VideoStatus {
                        PrivacyStatus = "private"
                    }
                }
            );
            if (uploadedVideo is null) {
                return false;
            }

            var testingPlaylistId = Configuration.GetValue("youtube.testing_playlist_id");
            if (testingPlaylistId is null) {
                return false;
            }

            var playlistItem = await YouTubeInterface.AddVideoToPlaylist(uploadedVideo.Id, testingPlaylistId);

            return playlistItem is not null;
        }

        internal bool CreateMultipleVideos(int numVideos) {
            var tasks = new Task<bool>[numVideos];
            for (var i = 0; i < numVideos; i++) {
                tasks[i] = CreateVideo();
            }

            Task.WaitAll(tasks);

            return tasks.All(task => task.Result);
        }

        internal bool CreateAndUploadMultiple(int numVideos) {
            var tasks = new Task<bool>[numVideos];
            for (var i = 0; i < numVideos; i++) {
                tasks[i] = CreateAndUpload();
            }

            Task.WaitAll(tasks);

            return tasks.All(task => task.Result);
        }
    }
}
