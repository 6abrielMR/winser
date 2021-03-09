using System;
using System.IO;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Logging;

namespace wsredjurista.services
{
    public class BlobStorage : IBlobStorage
    {
        private readonly ILogger<BlobStorage> logger;
        private readonly IServiceConfig configuration;
        private readonly ITextFile textFile;
        private readonly BlobContainerClient ContainerClient;
        private FileInfo[] files;

        public BlobStorage(ILogger<BlobStorage> logger, IServiceConfig configuration, ITextFile textFile)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.textFile = textFile;
            ContainerClient = new BlobContainerClient(configuration.defaultConnection, configuration.containerName);
        }

        public void Start()
        {
            logger.LogInformation("Connection Ready");
        }

        public void Stop()
        {
            throw new System.NotImplementedException();
        }

        public async void UploadAsync()
        {
            textFile.ReadDir();
            files = textFile.Files;
            if (files == null) return;
            foreach (var file in files)
            {
                var uploadFileStream = File.OpenRead(file.FullName);
                BlobClient blobClient = ContainerClient.GetBlobClient(file.Name);
                logger.LogInformation("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);
                await blobClient.UploadAsync(uploadFileStream, true);
                uploadFileStream.Close();
                textFile.Move(file.FullName, Path.Combine(configuration.filesProcessed, file.Name));
            }
        }
        
        public async void DeleteAsync(string[] filesToDelete)
        {
            textFile.Read(configuration.filesToDelete);
            foreach (var filename in filesToDelete)
            {
                if (!String.IsNullOrEmpty(filename))
                {
                    BlobClient blobClient = ContainerClient.GetBlobClient(filename);
                    logger.LogInformation("Deleting to Blob storage as blob:\n\t {0}\n", blobClient.Uri);
                    await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
                }
            }
        }
    }
}