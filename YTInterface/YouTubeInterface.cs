namespace MarbleDuels.YTInterface {
    using Google.Apis.Auth.OAuth2;
    using Google.Apis.Services;
    using Google.Apis.Util;
    using Google.Apis.YouTube.v3;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    internal static partial class YouTubeInterface {
        private static UserCredential? UserCredential { get; set; }
        private static YouTubeService? YouTubeService { get; set; }

        private static readonly string[] Scopes = {
            YouTubeService.Scope.Youtube,
            YouTubeService.Scope.YoutubeChannelMembershipsCreator,
            YouTubeService.Scope.YoutubeForceSsl,
            YouTubeService.Scope.YoutubeReadonly,
            YouTubeService.Scope.YoutubeUpload,
            YouTubeService.Scope.Youtubepartner,
            YouTubeService.Scope.YoutubepartnerChannelAudit
        };

        internal static async Task<YouTubeService?> GetYouTubeService() {
            Logger.Info("Getting YouTube service.");

            if (YouTubeService is not null && IsUserCredentialValid()) {
                return YouTubeService;
            }

            if (!await ValidateUserCredential()) {
                Logger.Warn("User credentials are invalid.");
                return null;
            }

            YouTubeService service;
            try {
                service = new YouTubeService(new BaseClientService.Initializer() {
                    HttpClientInitializer = UserCredential,
                    ApplicationName = Assembly.GetExecutingAssembly().GetName().Name
                });
            } catch (Exception e) {
                Logger.Error(e);
                return null;
            }

            Logger.Info("Done getting YouTube service.");
            return service;
        }

        private static bool IsUserCredentialValid() => UserCredential is not null && !UserCredential.Token.IsExpired(SystemClock.Default);

        private static async Task<bool> ValidateUserCredential() {
            Logger.Info("Validating user credential.");

            if (UserCredential is null) {
                Logger.Info("User credential is null. Creating user credential.");

                var secretFileStream = Util.OpenFile(Configuration.ClientSecretFilePath);
                if (secretFileStream is null) {
                    UserCredential = null;
                    Logger.Warn($"Could not open \"{Configuration.ClientSecretFilePath}\".");
                    return false;
                }

                using (secretFileStream) {
                    Logger.Info("Please provide authentication in your browser.");
                    try {
                        UserCredential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                            GoogleClientSecrets.FromStream(secretFileStream).Secrets,
                            Scopes,
                            Configuration.GetValueFromPath("youtube.user"),
                            CancellationToken.None
                        );
                    } catch (Exception e) {
                        UserCredential = null;
                        Logger.Error(e);
                        return false;
                    }
                }
            }

            if (UserCredential.Token.IsExpired(SystemClock.Default)) {
                Logger.Info("User credential is expired. Refreshing user credential.");

                Logger.Info("Please provide authentication in your browser.");

                if (!await UserCredential.RefreshTokenAsync(CancellationToken.None)) {
                    UserCredential = null;
                    Logger.Warn("User credential refresh was not successful.");
                    return false;
                }
            }

            Logger.Info("User credential is valid.");
            return true;
        }
    }
}
