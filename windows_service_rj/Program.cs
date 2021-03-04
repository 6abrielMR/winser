using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Enrichers;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using wsredjurista.services;

namespace wsredjurista
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            const string loggerTemplate = @"{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u4}]<{ThreadId}> [{SourceContext:l}] {Message:lj}{NewLine}{Exception}";
            const string baseDir = "/home/gabriel/Documentos/tmp/call/logs";
            var logfile = Path.Combine(baseDir, "log.txt");
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.With(new ThreadIdEnricher())
                .Enrich.FromLogContext()
                .WriteTo.Console(LogEventLevel.Information, loggerTemplate, theme: AnsiConsoleTheme.Literate)
                .WriteTo.File(logfile, LogEventLevel.Information, loggerTemplate,
                    rollingInterval: RollingInterval.Day, retainedFileCountLimit: 90)
                .CreateLogger();

            try
            {
                Log.Information("====================================================================");
                Log.Information($"Application Starts. Version: {System.Reflection.Assembly.GetEntryAssembly()?.GetName().Version}");
                Log.Information($"Application Directory: {AppDomain.CurrentDomain.BaseDirectory}");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Application terminated unexpectedly");
            }
            finally
            {
                Log.Information("====================================================================\r\n");
                Log.CloseAndFlush();
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureAppConfiguration((context, config) =>
                {
                    // Configure the app here.
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddTransient<IFileProcessor, FileProcessor>();
                    services.AddTransient<IWatcher, Watcher>();
                    services.AddTransient<IBlobStorage, BlobStorage>();
                    services.AddTransient<ITextFile, TextFile>();
                    services.AddHostedService<Worker>();
                    
                })
                .UseSerilog();
    }
}
