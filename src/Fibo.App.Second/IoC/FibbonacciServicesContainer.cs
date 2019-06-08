using Fibo.App.First.Handlers;
using Fibo.Business.Impl.Services;
using Fibo.Business.Impl.Services.EvaluationSenders;
using Fibo.Business.Interfaces.Handlers;
using Fibo.Business.Interfaces.Handlers.Commands;
using Fibo.Business.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fibo.App.Second.IoC
{
    public static class FibbonacciServicesContainer
    {
        public static IServiceCollection AddFibonacciServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ICommandHandler<EvaluateFibonacciStepCommand>, EvaluateFibonacciStepCommandHandler>();

            services.AddTransient<IFibonacciEvaluationSender, FibonacciEvaluationBusSender>();
            services.Configure<BusSenderConfig>(configuration.GetSection("BusSender"));

            services.AddTransient<IFibonacciEvaluationService, FibonacciEvaluationService>();

            services.AddSingleton<IFibonacciStorage, FibonacciStorage>();

            return services;
        }
    }
}
