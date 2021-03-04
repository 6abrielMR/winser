using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace wsredjurista.services
{
    public class TextFile : ITextFile
    {
        private readonly ILogger<TextFile> logger;
        public string[] Lines { get; set; }

        public TextFile(ILogger<TextFile> logger)
        {
            this.logger = logger;
        }

        public void Read(string path)
        {
            logger.LogInformation("Reading file");
            Lines = File.ReadAllText(path).Split(Environment.NewLine);
            logger.LogInformation($"File read, contains {Lines.Length} lines");
        }
    }
}