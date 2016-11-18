using System.Diagnostics;
using System.IO;

namespace ApkParser
{
    internal class ConsoleExecutor
    {
        /// <summary>
        /// Executes a comand
        /// </summary>
        /// <param name="command">comand</param>
        /// <param name="args">args</param>
        /// <returns>output</returns>
        public static string Execute(string command, params string[] args)
        {
            // Start the child process.
            var p = new Process
            {
                StartInfo =
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    WorkingDirectory = Directory.GetCurrentDirectory(),
                    FileName = command,
                    Arguments = string.Join(" ", args)
                }
            };
            // Redirect the output stream of the child process.
            p.Start();
            // Do not wait for the child process to exit before
            // reading to the end of its redirected stream.
            // p.WaitForExit();
            // Read the output stream first and then wait.
            var output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            return output;
        }
    }
}
