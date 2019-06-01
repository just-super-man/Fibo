using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Fibo.App.First.IoC;
using Fibo.App.First.Configs;

namespace Fibo.App.First
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.AddJsonFile("appsettings.json");
                })
                .ConfigureServices((context, services) =>
                {
                    services
                        .AddHostedService<Service>()
                        .AddOptions()
                        .Configure<StartConfig>(context.Configuration.GetSection("Start"))
                        .AddRabbitService(context.Configuration)
                        .AddFibonacciServices(context.Configuration);
                })
                .ConfigureLogging((context, builder) =>
                {
                    builder
                        .AddLog4Net()
                        .AddConsole();
                })
                .Build();

            await host.RunAsync();
        }
    }
}
