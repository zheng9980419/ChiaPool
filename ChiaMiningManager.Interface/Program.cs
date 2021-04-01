﻿using ChiaPool.Api;
using CliFx;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace ChiaPool
{
    public class Program
    {
        private static CliApplication Application;
        private static IServiceProvider Provider;

        public static async Task Main(string[] args)
        {
            Provider = MakeServiceProvider();

            Application = new CliApplicationBuilder()
                .AddCommandsFromThisAssembly()
                .UseTypeActivator(Provider.GetService)
                .Build();

            await Application.RunAsync();
        }

        private static IServiceProvider MakeServiceProvider()
        {
            var services = new ServiceCollection();

            services.AddSingleton<HttpClient>();
            services.AddTransient<ClientApiAccessor>();
            services.AddTransient<ServerApiAccessor>();

            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(x => x.GetInterface(nameof(ICommand)) != null && !x.IsAbstract))
            {
                services.AddTransient(type);
            }

            return services.BuildServiceProvider();
        }
    }
}
