using System;
using System.IO;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace wsredjurista.services
{
    public class BlobStorage : IBlobStorage
    {
        private readonly ILogger<BlobStorage> logger;
        private readonly IConfiguration configuration;
        private readonly string ConnectionString;
        private readonly string ContainerName;
        private BlobContainerClient ContainerClient;

        public BlobStorage(ILogger<BlobStorage> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
            ConnectionString = configuration.GetConnectionString("DefaultConnection");
            ContainerName = configuration.GetSection("Container:Name").Value;
        }

        public void Start()
        {
            logger.LogInformation("Connection Ready");
            ContainerClient = new BlobContainerClient(ConnectionString, ContainerName);
        }

        public void Stop()
        {
            throw new System.NotImplementedException();
        }

        public async void UploadAsync(FileStream uploadFileStream, string filename)
        {
            BlobClient blobClient = ContainerClient.GetBlobClient(filename);
            logger.LogInformation("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);
            await blobClient.UploadAsync(uploadFileStream, true);
            uploadFileStream.Close();
        }
        
        public async void DeleteAsync(string[] filesToDelete)
        {
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