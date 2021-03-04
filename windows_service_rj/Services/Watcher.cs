using System.IO;
using Microsoft.Extensions.Logging;

namespace wsredjurista.services
{
    public class Watcher : IWatcher
    {
        private readonly ILogger<Watcher> logger;
        private readonly IBlobStorage blobStorage;
        private FileSystemWatcher folderWatcher;
        public string Path { get; set; }
        public string Filter { get; set; }
        public NotifyFilters NotifyFilters { get; set; }
        public RenamedEventHandler Renamed { get; set; }

        public Watcher(ILogger<Watcher> logger, IBlobStorage blobStorage)
        {
            this.logger = logger;
            this.blobStorage = blobStorage;
        }

        public void Start()
        {
            logger.LogInformation("Watcher Starting");
            blobStorage.Start();
            folderWatcher = new FileSystemWatcher(Path, Filter)
            {
                NotifyFilter = NotifyFilters
            };
            folderWatcher.Renamed += Renamed;
            folderWatcher.EnableRaisingEvents = true;
        }

        public void Stop()
        {
            logger.LogInformation("Watcher Stop");
            folderWatcher.EnableRaisingEvents = false;
        }
    }
}