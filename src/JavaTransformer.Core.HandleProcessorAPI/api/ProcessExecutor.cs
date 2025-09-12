using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.Core.HandleProcessorAPI.api
{
    public class ProcessExecutor
    {
        public static ProcessExecutionResult Execute(string executablePath, string arguments)
        {
            if (!File.Exists(executablePath)) throw new FileNotFoundException(executablePath);

            var processStartInfo = CreateProcessStartInfo(executablePath, arguments);

            using (var process = new Process { StartInfo = processStartInfo })
            {
                process.Start();
                process.WaitForExit();

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                string combinedOutput = CombineOutput(output, error);

                return new ProcessExecutionResult(process.ExitCode, combinedOutput);
            }
        }

        private static ProcessStartInfo CreateProcessStartInfo(string executablePath, string arguments)
        {
            return new ProcessStartInfo
            {
                FileName = executablePath,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
        }

        private static string CombineOutput(string output, string error)
        {
            var result = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(output))
            {
                result.AppendLine(output);
            }

            if (!string.IsNullOrWhiteSpace(error))
            {
                result.AppendLine(error);
            }

            return result.ToString().Trim();
        }

        public record ProcessExecutionResult(int ExitCode, string Output)
        {
            public bool IsError()
            {
                if (ExitCode != 0)
                    return true;

                /**
                 *  раздал стилька хах
                 */
                if(Output.Contains("error") || Output.Contains("terminate"))
                    return true;

                return false;
            }
        }
    }
}
