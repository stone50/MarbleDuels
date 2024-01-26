namespace MarbleDuels.Utils {
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    internal static class Util {
        /// <summary>
        /// Creates a new process and asynchronouly runs the given command.
        /// </summary>
        /// <returns>
        /// The process's exit code, or 1 if the process could not be started.
        /// </returns>
        internal static async Task<int> RunCMD(string command) {
            var process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = $"/c {command}";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.OutputDataReceived += (sender, output) => {
                if (output.Data is null) {
                    return;
                }

                Logger.Info($"Process {((Process)sender).Id}: {output.Data}");
            };
            process.ErrorDataReceived += (sender, error) => {
                if (error.Data is null) {
                    return;
                }

                Logger.Error($"Process {((Process)sender).Id}: {error.Data}");
            };

            try {
                _ = process.Start();
                Logger.Info($"Executing '{command}' with process id '{process.Id}'.");
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
            } catch (Exception e) {
                Logger.Error(e);
                return 1;
            }

            await process.WaitForExitAsync();

            return process.ExitCode;
        }
    }
}
