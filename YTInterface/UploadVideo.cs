namespace MarbleDuels.YTInterface {
    using Google.Apis.Upload;
    using Google.Apis.YouTube.v3;
    using Google.Apis.YouTube.v3.Data;
    using System;
    using System.Threading.Tasks;

    internal static partial class YouTubeInterface {
        /// <param name="filePath">
        /// The file path of the video file to upload.
        /// </param>
        /// <returns>
        /// A Video object associated with the uploaded video, or null if video upload failed.
        /// </returns>
        internal static async Task<Video?> UploadVideo(string filePath, Video settings) {
            Logger.Info($"Beginning upload process for '{settings.Snippet.Title}'.");

            var youTubeService = await GetYouTubeService();
            if (youTubeService is null) {
                Logger.Warn("YouTube service is null.");
                return null;
            }

            FileStream fileStream;
            try {
                fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            } catch (Exception e) {
                Logger.Error(e);
                return null;
            }

            VideosResource.InsertMediaUpload videosInsertRequest;
            try {
                videosInsertRequest = youTubeService.Videos.Insert(settings, "contentDetails,fileDetails,id,liveStreamingDetails,localizations,player,processingDetails,recordingDetails,snippet,statistics,status,suggestions,topicDetails", fileStream, "video/*");
            } catch (Exception e) {
                Logger.Error(e);
                return null;
            }

            IUploadProgress progress;
            try {
                progress = await videosInsertRequest.UploadAsync();
            } catch (Exception e) {
                fileStream.Close();
                Logger.Error(e);
                return null;
            }

            fileStream.Close();

            if (progress.Status != UploadStatus.Completed) {
                Logger.Error(progress.Exception.Message);
                return null;
            }

            Logger.Info($"Finished upload process for '{videosInsertRequest.ResponseBody.Snippet.Title}'.");
            return videosInsertRequest.ResponseBody;
        }
    }
}
