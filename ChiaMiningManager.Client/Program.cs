using ChiaMiningManager.Services;
using Common.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace ChiaMiningManager
{
    public class Program
    {
        public const int ApplicationPort = 8888;
        private static IHost Application;

        public static async Task Main(string[] args)
        {
            Application = CreateHostBuilder(args).Build();
            var logger = Application.Services.GetRequiredService<ILogger<Startup>>();
            var assembly = Assembly.GetExecutingAssembly();

            var validationResult = await Application.Services.ValidateOptionsAsync(assembly);

            if (!validationResult.IsSuccessful)
            {
                logger.LogError($"Config Validation failed: {validationResult.Reason}");
            }

            await Application.Services.InitializeApplicationServicesAsync(assembly);

            if (args.Length == 1 && args[0] == "init")
            {
                await RunInitAsync();
                Application.Dispose();
                return;
            }

            Application.Services.RunApplicationServices(assembly);

            await Application.StartAsync();
            await Application.WaitForShutdownAsync();

            Application.Dispose();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();

                    webBuilder.ConfigureKestrel((context, options) =>
                    {
                        options.Listen(IPAddress.Any, ApplicationPort);
                    });
                })
                .ConfigureAppConfiguration(config =>
                {
                    config.Sources.Clear();
                    config.AddJsonFile("appsettings.json", false);
                });

        private static async Task RunInitAsync()
        {
            var client = Application.Services.GetRequiredService<MinerClient>();
            var logger = Application.Services.GetRequiredService<ILogger<Startup>>();

            logger.LogInformation("Starting mining session...");
            if (!await client.SendStartRequest())
            {
                return;
            }

            logger.LogInformation("Downloading private keys...");
            if (!await client.RefreshCAKeysAsync())
            {
                return;
            }
        }
    }
}
