using MarbleDuels;
using MarbleDuels.VideoCreation;
using MarbleDuels.YTInterface;

if (!Logger.Initialize()) {
    Logger.Warn("Could not initialize logger.");
    /*
     * If the logger fails to initialize,
     * logs will still be written to the console.
     * If this is not acceptable, uncomment the following:
     */
    // Environment.Exit(1);
}

if (!Configuration.Initialize()) {
    Logger.Warn("Could not initialize configuration.");
    /*
     * If the configuration fails to initialize,
     * some functionality will fail, but not crash.
     * If this is not acceptable, uncomment the following:
     */
    // Environment.Exit(1);
}

if (!VideoCreator.Initialize()) {
    Logger.Warn("Could not initialize video creation.");
    /*
     * If the video creator fails to initialize,
     * creating videos with VideoCreator objects will fail, but not crash.
     * If this is not acceptable, uncomment the following:
     */
    // Environment.Exit(1);
}

var playlists = await YouTubeInterface.FetchPlaylists();
if (playlists is not null) {
    foreach (var playlist in playlists) {
        Logger.Debug($"{playlist.Snippet.Title}: {playlist.Id}");
    }
}
