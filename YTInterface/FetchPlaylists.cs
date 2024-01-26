namespace MarbleDuels.YTInterface {
    using Google.Apis.YouTube.v3.Data;
    using System.Threading.Tasks;

    internal static partial class YouTubeInterface {
        /// <returns>
        /// An array of Playlist objects associated with the currently authenticated channel, or null if fetching playlists fails.
        /// </returns>
        internal static async Task<Playlist[]?> FetchPlaylists() {
            Logger.Info("Fetching playlists.");

            Playlist[] playlists = [];
            PlaylistListResponse? results = null;
            do {
                results = await FetchPlaylistPage(results?.NextPageToken);

                if (results is null) {
                    Logger.Warn("Could not retrieve all pages.");
                    return null;
                }

                playlists = [.. playlists, .. results.Items];
            } while (results.NextPageToken is not null);

            return playlists;
        }

        private static async Task<PlaylistListResponse?> FetchPlaylistPage(string? pageToken) {
            var youTubeService = await GetYouTubeService();
            if (youTubeService is null) {
                Logger.Warn("YouTube service is null.");
                return null;
            }

            var playlistFetchRequest = youTubeService.Playlists.List("contentDetails,id,localizations,snippet");
            playlistFetchRequest.Mine = true;
            playlistFetchRequest.PageToken = pageToken;

            try {
                return await playlistFetchRequest.ExecuteAsync();
            } catch (Exception e) {
                Logger.Error(e);
                return null;
            }
        }
    }
}
