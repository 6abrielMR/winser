using System;
using System.IO;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Extensions.Logging;

namespace wsredjurista.services
{
    public class FileProcessor : IFileProcessor
    {
        private readonly ILogger<FileProcessor> logger;
        private readonly IBlobStorage blobStorage;
        private readonly ITextFile textFile;
        private readonly Timer timer;
        private readonly string inputFolder;

        public FileProcessor(ILogger<FileProcessor> logger, IServiceConfig configuration, IBlobStorage blobStorage, ITextFile textFile)
        {
            this.logger = logger;
            this.blobStorage = blobStorage;
            this.textFile = textFile;
            timer = new Timer(120000);
            inputFolder = configuration.inputFolder;
        }

        public void Process(object sender, ElapsedEventArgs e)
        {
            blobStorage.UploadAsync();
            blobStorage.DeleteAsync(textFile.Lines);
        }

        public Task Start()
        {
            logger.LogInformation("Service Starting");
            timer.Elapsed += Process;
            timer.Start();
            if (!Directory.Exists(inputFolder))
            {
                logger.LogInformation($"Please make the InputFolder [{inputFolder}] exists, then restart the service.");
                return Task.CompletedTask;
            }

            return Task.CompletedTask;
        }

        public void Stop()
        {
            logger.LogInformation("Service Stop");
        }
    }
}