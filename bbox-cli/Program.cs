using System.IO;
using BBox.Cli.Commands;
using BBox.Cli.Configuration;
using BBox.Client;
using BBox.Client.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BBox.Cli
{
    static class Program
    {
        public static int Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            return host.Services.GetRequiredService<BBoxCli>().Execute();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(builder =>
                {
                    builder.Sources.Clear();
                    builder.SetBasePath(Directory.GetCurrentDirectory());
                    builder.AddJsonFile("appsettings.json", optional: true);
                    builder.AddCommandLine(args);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services
                        .AddSingleton(hostContext.Configuration.Get<BBoxClientConfiguration>())
                        .AddSingleton(new ArgsContainer(args))
                        .AddSingleton<CommandLineApplicationBuilder>()
                        .AddSingleton<IBBoxClient, BBoxClient>()
                        .AddSingleton(provider => provider.GetRequiredService<CommandLineApplicationBuilder>().Build())
                        .AddSingleton<BBoxCli>()
                        .AddSingleton<IBBoxCommand, BBoxWirelessCommand>()
                        ;
                });
    }
}