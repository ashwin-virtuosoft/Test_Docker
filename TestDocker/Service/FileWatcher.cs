using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.FileSystemGlobbing;
using System.IO;

namespace TestDocker.Service
{
    public class FileWatcher : IHostedService
    {
        private readonly List<string> modifiedFiles=new List<string>();
        private FileSystemWatcher fileSystemWatcher;
        public Task StartAsync(CancellationToken cancellationToken)
        {
            fileSystemWatcher = new FileSystemWatcher(@"/app/TestFolder")
            {
                NotifyFilter = NotifyFilters.LastWrite |  NotifyFilters.FileName | NotifyFilters.DirectoryName,
                Filter = "*.txt",
                EnableRaisingEvents = true
            };

            fileSystemWatcher.Changed += OnChanged;
            fileSystemWatcher.Created += OnCreated;
            fileSystemWatcher.Deleted += OnDeleted;

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            fileSystemWatcher.Dispose();
            return Task.CompletedTask;
        }
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            Console.WriteLine($"File: {e.FullPath} {e.ChangeType}");
            modifiedFiles.Add(e.FullPath);
        }

        private void OnCreated(object source, FileSystemEventArgs e)
        {
            Console.WriteLine($"File created: {e.FullPath}");
            modifiedFiles.Add(e.FullPath);
        }

        private void OnDeleted(object source, FileSystemEventArgs e)
        {
            Console.WriteLine($"File deleted: {e.FullPath}");
        }

        public List<string> GetModifiedFiles()
        {
            return new List<string>(modifiedFiles);
        }
    }
}
