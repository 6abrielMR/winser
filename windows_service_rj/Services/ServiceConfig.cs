using Microsoft.Extensions.Configuration;

namespace wsredjurista.services
{
    public class ServiceConfig : IServiceConfig
    {
        public string defaultConnection { get; set; }
        public string containerName { get; set; }
        public string inputFolder { get; set; }
        public string filesProcessed { get; set; }
        public string filesToDelete { get; set; }
        public string filter { get; set; }

        public ServiceConfig(IConfiguration configuration)
        {
            defaultConnection = configuration.GetConnectionString("DefaultConnection");
            containerName = configuration.GetSection("Container:Name").Value;
            inputFolder = configuration.GetSection("Path:InputFolder").Value;
            filesProcessed = configuration.GetSection("Path:FilesProcessed").Value;
            filesToDelete = configuration.GetSection("Path:FilesToDelete").Value;
            filter = configuration.GetSection("Path:Filter").Value;
        }
    }
}