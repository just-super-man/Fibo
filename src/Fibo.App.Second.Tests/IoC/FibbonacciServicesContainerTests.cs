using Fibo.App.Second.IoC;
using Fibo.Business.Interfaces.Handlers;
using Fibo.Business.Interfaces.Handlers.Commands;
using Fibo.Business.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Fibo.App.Second.Tests.IoC
{
    public class FibbonacciServicesContainerTests : BaseIocTests
    {
        [Test]
        public void AddFibonacciServices()
        {
            var provider = _serviceCollection
                .AddFibonacciServices(_configuration)
                .AddRabbitService(_configuration)
                .BuildServiceProvider();

            AssertNotNull<ICommandHandler<EvaluateFibonacciStepCommand>>(provider);
            AssertNotNull<IFibonacciEvaluationSender>(provider);
            AssertNotNull<IFibonacciEvaluationService>(provider);
            AssertNotNull<IFibonacciStorage>(provider);
        }
    }
}
