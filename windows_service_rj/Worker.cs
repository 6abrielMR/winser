using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

using wsredjurista.services;

namespace wsredjurista
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;
        private readonly IFileProcessor fileProcessor;

        public Worker(ILogger<Worker> logger, IFileProcessor fileProcessor)
        {
            this.logger = logger;
            this.fileProcessor = fileProcessor;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.CompletedTask;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            fileProcessor.Start();
            return base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            fileProcessor.Stop();
            await base.StopAsync(cancellationToken);
        }
    }
}
