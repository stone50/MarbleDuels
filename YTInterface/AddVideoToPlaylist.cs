namespace MarbleDuels.YTInterface {
    internal static partial class YouTubeInterface {
        // TODO
        /*internal static async Task<bool> AddVideoToPlaylist(string videoId, string playlistId) {
            Logger.Info($"Moving video {videoId} to playlist {playlistId}.");

            var youTubeService = await GetYouTubeService();
            if (youTubeService is null) {
                Logger.Warn("YouTube service is null.");
                return false;
            }

            PlaylistItemsResource.InsertRequest playlistItemsInsertRequest;
            try {
                playlistItemsInsertRequest = youTubeService.PlaylistItems.Insert("contentDetails,id,snippet,status");
            } catch (Exception e) {
                Logger.Error(e);
                return false;
            }

            //await playlistItemsInsertRequest.ExecuteAsync();

            Logger.Info($"Finished upload process for \"{settings.Snippet.Title}\".");
            return true;
        }*/
    }
}
