namespace MarbleDuels {
    using Testing;
    using VideoCreation;
    using YTInterface;

    internal class Program {
        private static async Task Main(string[] args) {
            if (!Logger.Initialize()) {
                Logger.Warn("Could not initialize logger.");
            }

            if (!Configuration.Initialize()) {
                Logger.Error("Could not initialize configuration.");
                Environment.Exit(1);
            }

            if (!VideoCreator.Initialize()) {
                Logger.Warn("Could not initialize video creation.");
            }

            /*var playlists = await YouTubeInterface.FetchPlaylists();
            if (playlists is not null) {
                foreach (var playlist in playlists) {
                    Logger.Debug($"{playlist.Snippet.Title}: {playlist.Id}");
                }
            }

            _ = await new Tester(
                new GodotVideoCreator("race_to_the_bottom_project_path"),
                VideoCreationSettings.SD240p30fps
            ).CreateAndUpload();*/
        }
    }
}