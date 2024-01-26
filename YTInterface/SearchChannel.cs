namespace MarbleDuels.YTInterface {
    using Google.Apis.YouTube.v3.Data;
    using System.Threading.Tasks;

    internal static partial class YouTubeInterface {
        /// <returns>
        /// An array of SearchResult objects associated with the currently authenticated channel, or null if searching fails.
        /// </returns>
        internal static async Task<SearchResult[]?> SearchChannel() {
            Logger.Info("Searching channel.");

            SearchResult[] results = [];
            SearchListResponse? pageResults = null;
            do {
                pageResults = await SearchChannelPage(pageResults?.NextPageToken);

                if (pageResults is null) {
                    Logger.Warn("Could not retrieve all pages.");
                    return null;
                }

                results = [.. results, .. pageResults.Items];
            } while (pageResults.NextPageToken is not null);

            return results;
        }

        private static async Task<SearchListResponse?> SearchChannelPage(string? pageToken) {
            var youTubeService = await GetYouTubeService();
            if (youTubeService is null) {
                Logger.Warn("YouTube service is null.");
                return null;
            }

            var searchRequest = youTubeService.Search.List("id,snippet");
            searchRequest.PageToken = pageToken;
            // Leaving ForMine as null returns results from everywhere.
            // Setting it to false only returns results from the currently authenticated channel.
            searchRequest.ForMine = false;

            try {
                return await searchRequest.ExecuteAsync();
            } catch (Exception e) {
                Logger.Error(e);
                return null;
            }
        }
    }
}
