using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            var stopwatch = Stopwatch.StartNew();
            string targetFilePath = Path.Combine(folderPath, fileName);

            if (!File.Exists(targetFilePath))
                throw new FileNotFoundException($"File {targetFilePath} does not exist");

            //DateTime targetTime = File.GetLastWriteTime(targetFilePath);
            //long targetTimestamp = new DateTimeOffset(targetTime).ToUnixTimeSeconds();

            var newerFiles = new List<string>();

            string command = $"find {folderPath} -maxdepth 1 -type f -newer {targetFilePath}";

            // Use CliWrap to execute the command
            var result = await Cli.Wrap("/bin/bash")
                .WithArguments($"-c \"{command}\"")
                .ExecuteBufferedAsync();

            if (result.ExitCode != 0)
                throw new Exception($"Command failed with exit code {result.ExitCode}");

            var res = result.StandardOutput.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            newerFiles.AddRange(res);

            stopwatch.Stop();
            Console.WriteLine($"GetFilesFromOS execution time: {stopwatch.ElapsedMilliseconds} ms");
            return newerFiles;
        }

        public List<string> GetFiles(string folderPath, string fileName)
        {
            var stopwatch = Stopwatch.StartNew();

            string targetFilePath = Path.Combine(folderPath, fileName);
            if (!File.Exists(targetFilePath))
                throw new FileNotFoundException($"File {targetFilePath} does not exist");

            DateTime targetTime = File.GetLastWriteTime(targetFilePath);
            var newerFiles = new List<string>();
            foreach (string file in Directory.GetFiles(folderPath))
            {
                DateTime fileTime = File.GetLastWriteTime(file);
                if (file != targetFilePath && fileTime > targetTime)
                    newerFiles.Add(file);
            }

            stopwatch.Stop();

            Console.WriteLine($"GetFiles execution time: {stopwatch.ElapsedMilliseconds} ms");
            return newerFiles;
        }
    }
}
    