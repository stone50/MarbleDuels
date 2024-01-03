namespace MarbleDuels {
    using System;

    internal static class Util {
        internal static FileStream? OpenFile(string filePath) {
            Logger.Info($"Opening \"{filePath}\".");

            FileStream fileStream;
            try {
                fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            } catch (Exception e) {
                Logger.Error(e);
                return null;
            }

            Logger.Info($"Done opening \"{filePath}\".");
            return fileStream;
        }

        internal static bool DeleteFile(string filePath) {
            Logger.Info($"Deleting \"{filePath}\".");

            if (!File.Exists(filePath)) {
                Logger.Warn($"Could not find \"{filePath}\".");
                return false;
            }

            try {
                File.Delete(filePath);
            } catch (Exception e) {
                Logger.Error(e);
                return false;
            }

            Logger.Info($"Done deleting \"{filePath}\".");
            return true;
        }

        internal static bool DeleteDir(string dirPath) {
            Logger.Info($"Deleting \"{dirPath}\".");

            if (!Directory.Exists(dirPath)) {
                Logger.Warn($"Could not find \"{dirPath}\".");
                return false;
            }

            try {
                Directory.Delete(dirPath, true);
            } catch (Exception e) {
                Logger.Error(e);
                return false;
            }

            Logger.Info($"Done deleting \"{dirPath}\".");
            return true;
        }
    }
}
