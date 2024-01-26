namespace MarbleDuels.YTInterface {
    using Google.Apis.YouTube.v3;
    using Google.Apis.YouTube.v3.Data;

    internal static partial class YouTubeInterface {
        /// <param name="position">
        /// The position in the playlist to insert the video.
        /// </param>
        internal static async Task<PlaylistItem?> AddVideoToPlaylist(string videoId, string playlistId) {
            Logger.Info($"Moving video {videoId} to playlist {playlistId}.");

            var youTubeService = await GetYouTubeService();
            if (youTubeService is null) {
                Logger.Warn("YouTube service is null.");
                return null;
            }

            var playlistItem = new PlaylistItem {
                Snippet = new() {
                    PlaylistId = playlistId,
                    ResourceId = new() {
                        Kind = "youtube#video",
                        VideoId = videoId
                    }
                }
            };

            PlaylistItemsResource.InsertRequest playlistItemInsertRequest;
            try {
                playlistItemInsertRequest = youTubeService.PlaylistItems.Insert(playlistItem, "contentDetails,id,snippet");
            } catch (Exception e) {
                Logger.Error(e);
                return null;
            }

            PlaylistItem addedVideo;
            try {
                addedVideo = await playlistItemInsertRequest.ExecuteAsync();
            } catch (Exception e) {
                Logger.Error(e);
                return null;
            }

            return addedVideo;
        }
    }
}
