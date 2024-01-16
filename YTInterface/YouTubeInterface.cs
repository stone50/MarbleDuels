namespace MarbleDuels.YTInterface {
    using Google.Apis.Auth.OAuth2;
    using Google.Apis.Services;
    using Google.Apis.YouTube.v3;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    internal static partial class YouTubeInterface {
        private static UserCredential? UserCredential { get; set; }
        private static YouTubeService? YouTubeService { get; set; }

        internal static readonly string[] Scopes = [
            YouTubeService.Scope.Youtube,
            YouTubeService.Scope.YoutubeChannelMembershipsCreator,
            YouTubeService.Scope.YoutubeForceSsl,
            YouTubeService.Scope.YoutubeReadonly,
            YouTubeService.Scope.YoutubeUpload,
            YouTubeService.Scope.Youtubepartner,
            YouTubeService.Scope.YoutubepartnerChannelAudit
        ];

        /// <remarks>
        /// This may require the user to manually provide authentication in their browser.
        /// </remarks>
        /// <returns>
        /// A YouTubeService object, or null if authentication failed.
        /// </returns>
        internal static async Task<YouTubeService?> GetYouTubeService() {
            Logger.Info("Getting YouTube service.");

            if (
                YouTubeService is not null &&
                UserCredential is not null &&
                !UserCredential.Token.IsStale
            ) {
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

        /// <remarks>
        /// If UserCredential is null, this fetches the user's credentials,
        /// which will require the user to manually provide authentication in their browser
        /// if they have not authenticated before.
        /// If UserCredential is expired, this refreshes the user's credentials,
        /// which will require the user to manually provide authentication in their browser.
        /// </remarks>
        /// <returns>
        /// Whether or not UserCredential is valid.
        /// </returns>
        private static async Task<bool> ValidateUserCredential() {
            if (UserCredential is null) {
                Logger.Info("User credential is null. Creating user credential.");

                FileStream secretFileStream;
                try {
                    secretFileStream = new FileStream(Configuration.ClientSecretFileName, FileMode.Open, FileAccess.Read);
                } catch (Exception e) {
                    Logger.Error(e);
                    return false;
                }

                Logger.Info("You may need to provide authentication in your browser.");
                try {
                    using (secretFileStream) {
                        UserCredential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                            GoogleClientSecrets.FromStream(secretFileStream).Secrets,
                            Scopes,
                            Configuration.GetValue("youtube.user"),
                            CancellationToken.None
                        );
                    }
                } catch (Exception e) {
                    UserCredential = null;
                    Logger.Error(e);
                    return false;
                }
            }

            if (UserCredential.Token.IsStale) {
                Logger.Info("User credential is expired. Please provide authentication in your browser.");

                if (!await UserCredential.RefreshTokenAsync(CancellationToken.None)) {
                    UserCredential = null;
                    Logger.Warn("User credential refresh was not successful.");
                    return false;
                }
            }

            return true;
        }
    }
}
