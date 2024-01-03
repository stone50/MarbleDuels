namespace MarbleDuels {
    using System;
    using System.Collections.Generic;

    internal static class Logger {
        private static readonly Queue<string> MessageQueue = new();
        private static bool Dumping = false;
        private static string? LogFilePath;

        internal static bool Initialize() {
            Info("Initializing logger.");

            string logDir;
            try {
                logDir = Path.Combine(Directory.GetCurrentDirectory(), "logs");
            } catch (Exception e) {
                Console.WriteLine(e);
                return false;
            }

            if (!Directory.Exists(logDir)) {
                try {
                    _ = Directory.CreateDirectory(logDir);
                } catch (Exception e) {
                    Console.WriteLine(e);
                    return false;
                }
            }

            var logFileName = $"[{DateTime.UtcNow:MM-dd-yyyy HH-mm-ss}]_log.txt";
            try {
                LogFilePath = Path.Combine(logDir, logFileName);
                File.CreateText(LogFilePath).Close();
            } catch (Exception e) {
                LogFilePath = null;
                Console.WriteLine(e);
                return false;
            }

            Info("Done initializing logger.");
            return true;
        }

        private static bool Log(string message) {
            Console.WriteLine(message);

            if (LogFilePath is null) {
                return false;
            }

            StreamWriter logStream;
            try {
                logStream = new StreamWriter(LogFilePath, true);
            } catch (Exception e) {
                Console.WriteLine(e);
                return false;
            }

            using (logStream) {
                logStream.WriteLine(message);
            }

            return true;
        }

        private static void LogLevel(string level, object message) {
            MessageQueue.Enqueue($"[{DateTime.UtcNow}] [{level}]: {message}");
            _ = DumpQueue();
        }

        private static bool DumpQueue() {
            if (Dumping) {
                return false;
            }

            Dumping = true;
            while (MessageQueue.Count > 0) {
                _ = Log(MessageQueue.Dequeue());
            }

            Dumping = false;

            return true;
        }

        internal static void Debug(object message) => LogLevel("DEBUG", message);
        internal static void Info(object message) => LogLevel("INFO", message);
        internal static void Warn(object message) => LogLevel("WARN", message);
        internal static void Error(object message) => LogLevel("ERROR", message);
    }
}
