namespace MarbleDuels.VideoCreation {
    internal struct VideoCreationSettings {
        internal int ResolutionWidth;
        internal int ResolutionHeight;
        internal int FrameRate;

        public override readonly string ToString() => $"{ResolutionWidth}x{ResolutionHeight} {FrameRate} fps";

        #region Commonly Used Settings
        internal static readonly VideoCreationSettings UHD4320p60fps = new() {
            ResolutionWidth = 7680,
            ResolutionHeight = 4320,
            FrameRate = 60
        };

        internal static readonly VideoCreationSettings UHD4320p30fps = new() {
            ResolutionWidth = 7680,
            ResolutionHeight = 4320,
            FrameRate = 30
        };

        internal static readonly VideoCreationSettings UHD2160p60fps = new() {
            ResolutionWidth = 3840,
            ResolutionHeight = 2160,
            FrameRate = 60
        };

        internal static readonly VideoCreationSettings UHD2160p30fps = new() {
            ResolutionWidth = 3840,
            ResolutionHeight = 2160,
            FrameRate = 30
        };

        internal static readonly VideoCreationSettings UHD1440p60fps = new() {
            ResolutionWidth = 2560,
            ResolutionHeight = 1440,
            FrameRate = 60
        };

        internal static readonly VideoCreationSettings UHD1440p30fps = new() {
            ResolutionWidth = 2560,
            ResolutionHeight = 1440,
            FrameRate = 30
        };

        internal static readonly VideoCreationSettings HD1080p60fps = new() {
            ResolutionWidth = 1920,
            ResolutionHeight = 1080,
            FrameRate = 60
        };

        internal static readonly VideoCreationSettings HD1080p30fps = new() {
            ResolutionWidth = 1920,
            ResolutionHeight = 1080,
            FrameRate = 30
        };

        internal static readonly VideoCreationSettings HD720p60fps = new() {
            ResolutionWidth = 1280,
            ResolutionHeight = 720,
            FrameRate = 60
        };

        internal static readonly VideoCreationSettings HD720p30fps = new() {
            ResolutionWidth = 1280,
            ResolutionHeight = 720,
            FrameRate = 30
        };

        internal static readonly VideoCreationSettings SD480p30fps = new() {
            ResolutionWidth = 854,
            ResolutionHeight = 480,
            FrameRate = 30
        };

        internal static readonly VideoCreationSettings SD360p30fps = new() {
            ResolutionWidth = 640,
            ResolutionHeight = 360,
            FrameRate = 30
        };

        internal static readonly VideoCreationSettings SD240p30fps = new() {
            ResolutionWidth = 426,
            ResolutionHeight = 240,
            FrameRate = 30
        };
        #endregion Commonly Used Settings
    }
}