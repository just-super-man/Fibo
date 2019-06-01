using EasyNetQ;
using Fibo.Business.Impl.Services.EvaluationSenders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fibo.App.Second.IoC
{
    public static class RabbitContainer
    {
        public static IServiceCollection AddRabbitService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<BusSenderConfig>(configuration.GetSection("BusSender"));

            services.AddSingleton(serviceProvider =>
            {
                var config = serviceProvider.GetService<IOptions<BusSenderConfig>>();
                return RabbitHutch.CreateBus(config.Value.ConnectionString);
            });
            
            return services;
        }
    }
}
