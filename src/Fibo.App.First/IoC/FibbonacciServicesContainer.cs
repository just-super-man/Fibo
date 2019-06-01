using Fibo.App.First.Handlers;
using Fibo.Business.Impl.Services;
using Fibo.Business.Impl.Services.EvaluationSenders;
using Fibo.Business.Interfaces.Handlers;
using Fibo.Business.Interfaces.Handlers.Commands;
using Fibo.Business.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RestSharp;

namespace Fibo.App.First.IoC
{
    public static class FibbonacciServicesContainer
    {
        public static IServiceCollection AddFibonacciServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICommandHandler<EvaluateFibonacciStepCommand>, EvaluateFibonacciStepCommandHandler>();

            services.AddScoped<IFibonacciEvaluationSender, FibonacciEvaluationWebSender>();
            services.AddScoped<IRestClient>(p => new RestClient(p.GetService<IOptions<WebSenderConfig>>().Value.BaseUrl));
            services.Configure<WebSenderConfig>(configuration.GetSection("WebSender"));

            services.AddScoped<IFibonacciEvaluationService, FibonacciEvaluationService>();

            services.AddSingleton<IFibonacciStorage, FibonacciStorage>();

            return services;
        }
    }
}
