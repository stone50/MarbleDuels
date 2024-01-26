using MarbleDuels;
using MarbleDuels.Testing;
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
     * some functionality will fail, but the application will not crash.
     * If this is not acceptable, uncomment the following:
     */
    // Environment.Exit(1);
}

if (!VideoCreator.Initialize()) {
    Logger.Warn("Could not initialize video creation.");
    /*
     * If the video creator fails to initialize,
     * creating videos with VideoCreator objects will fail, but not crash the application.
     * If this is not acceptable, uncomment the following:
     */
    // Environment.Exit(1);
}

// Try to get YouTube service immediately to test for a necessary authentication.
if (await YouTubeInterface.GetYouTubeService() is null) {
    Logger.Warn("Could not get YouTube service.");
    /*
     * If the YouTube service cannot be fetched,
     * api calls to YouTube will fail, but not crash the application.
     * If this is not acceptable, uncomment the following:
     */
    // Environment.Exit(1);
}

var tester = new Tester(new GodotVideoCreator("race_to_the_bottom_project_path", true), VideoCreationSettings.SD240p30fps);
Logger.Debug(tester.CreateMultipleVideos(3));
