using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace wsredjurista.services
{
    public class FileProcessor : IFileProcessor
    {
        private readonly ILogger<FileProcessor> logger;
        private readonly IConfiguration configuration;
        private readonly IWatcher watcher;
        private readonly IBlobStorage blobStorage;
        private readonly ITextFile textFile;
        private readonly string inputFolder;
        private readonly string filesToDelete;
        private readonly string filter;

        public FileProcessor(ILogger<FileProcessor> logger, IConfiguration configuration, IWatcher watcher, IBlobStorage blobStorage, ITextFile textFile)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.watcher = watcher;
            this.blobStorage = blobStorage;
            this.textFile = textFile;
            inputFolder = configuration.GetSection("Path:InputFolder").Value;
            filesToDelete = configuration.GetSection("Path:FilesToDelete").Value;
            filter = configuration.GetSection("Path:Filter").Value;
        }

        public void Process(object source, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Renamed) return;
            textFile.Read(filesToDelete);
            blobStorage.UploadAsync(File.OpenRead(e.FullPath), e.Name);
            blobStorage.DeleteAsync(textFile.Lines);
        }

        public Task Start()
        {
            logger.LogInformation("Service Starting");
            if (!Directory.Exists(inputFolder))
            {
                logger.LogInformation($"Please make the InputFolder [{inputFolder}] exists, then restart the service.");
                return Task.CompletedTask;
            }

            watcher.Path = inputFolder;
            watcher.Filter = filter;
            watcher.NotifyFilters = NotifyFilters.CreationTime | NotifyFilters.LastWrite | NotifyFilters.FileName |
                                    NotifyFilters.DirectoryName;
            watcher.Renamed = Process;
            watcher.Start();

            return Task.CompletedTask;
        }

        public void Stop()
        {
            logger.LogInformation("Service Stop");
            watcher.Stop();
        }
    }
}