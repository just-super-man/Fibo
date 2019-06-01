using Fibo.App.Second.Hangfire;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fibo.App.Second.IoC
{
    public static class HangfireContainer
    {
        public static IServiceCollection AddHangfireStuff(this IServiceCollection services)
        {
            services.AddSingleton<ContainerJobActivator>();

            services.AddHangfire((serviceProvider, configuration) =>
            {
                configuration
                    .UseLog4NetLogProvider()

                    .UseActivator(
                        serviceProvider.GetService<ContainerJobActivator>())
                    .UseSqlServerStorage(
                        serviceProvider.GetService<IConfiguration>().GetConnectionString("hangfire"));
            });

            services.AddHangfireServer();
            return services;
        }
    }
}
