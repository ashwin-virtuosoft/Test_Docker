using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CliWrap;
using CliWrap.Buffered;

namespace TestDocker.Service
{
    public class FileService
    {
        public async Task<List<string>> GetNewerFilesAsync(string folderPath, string fileName)
        {
            string targetFilePath = Path.Combine(folderPath, fileName);

            if (!File.Exists(targetFilePath))
                throw new FileNotFoundException($"File {targetFilePath} does not exist");

            DateTime targetTime = File.GetLastWriteTime(targetFilePath);
            long targetTimestamp = new DateTimeOffset(targetTime).ToUnixTimeSeconds();

            var newerFiles = new List<string>();

            string command = $"find {folderPath} -maxdepth 1 -type f -newermt @{targetTimestamp} ! -name \"{fileName}\"";

            // Use CliWrap to execute the command
            var result = await Cli.Wrap("/bin/bash")
                .WithArguments($"-c \"{command}\"")
                .ExecuteBufferedAsync();

            if (result.ExitCode != 0)
                throw new Exception($"Command failed with exit code {result.ExitCode}");

            var res = result.StandardOutput.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            newerFiles.AddRange(res);

            return newerFiles;
        }
    }
}
    