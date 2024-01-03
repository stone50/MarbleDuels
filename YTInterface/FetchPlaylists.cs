namespace MarbleDuels.YTInterface {
    using Google.Apis.YouTube.v3.Data;
    using System.Linq;
    using System.Threading.Tasks;

    internal static partial class YouTubeInterface {
        /// <returns>
        /// An array of Playlist objects associated with the currently logged in user, or null if fetching playlists fails.
        /// </returns>
        internal static async Task<Playlist[]?> FetchPlaylists() {
            Logger.Info("Fetching playlists.");

            var playlists = Array.Empty<Playlist>();
            PlaylistListResponse? results = null;
            do {
                results = await FetchPlaylistPage(results?.NextPageToken);

                if (results is null) {
                    Logger.Warn("Could not retrieve all pages.");
                    return null;
                }

                try {
                    playlists = playlists.Concat(results.Items).ToArray();
                } catch (Exception e) {
                    Logger.Error(e);
                    return null;
                }
            } while (results.NextPageToken is not null);

            Playlist[] playlistArray;
            try {
                playlistArray = playlists.ToArray();
            } catch (Exception e) {
                Logger.Error(e);
                return null;
            }

            Logger.Info("Done fetching playlists.");
            return playlistArray;
        }

        private static async Task<PlaylistListResponse?> FetchPlaylistPage(string? pageToken) {
            var youTubeService = await GetYouTubeService();
            if (youTubeService is null) {
                Logger.Warn("YouTube service is null.");
                return null;
            }

            var playlistFetchRequest = youTubeService.Playlists.List("contentDetails,id,localizations,player,snippet,status");
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
