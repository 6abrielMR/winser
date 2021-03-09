using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace wsredjurista.services
{
    public class TextFile : ITextFile
    {
        private readonly ILogger<TextFile> logger;
        private readonly DirectoryInfo dir;
        public string[] Lines { get; set; }
        public FileInfo[] Files { get; set; }

        public TextFile(ILogger<TextFile> logger, IServiceConfig config)
        {
            this.logger = logger;
            dir = new DirectoryInfo(config.inputFolder);
        }

        public void Read(string path)
        {
            logger.LogInformation("Reading file");
            Lines = File.ReadAllText(path).Split(Environment.NewLine);
            logger.LogInformation($"File read, contains {Lines.Length} lines");
        }

        public void Move(string sourceFile, string destinationFile)
        {
            logger.LogInformation("Moving file to processed");
            File.Move(sourceFile, destinationFile);
        }

        public void ReadDir()
        {
            Files = dir.GetFiles();
        }
    }
}