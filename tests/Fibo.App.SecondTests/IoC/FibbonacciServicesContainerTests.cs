using Fibo.App.Second.IoC;
using Fibo.App.Second.Tests.IoC;
using Fibo.Business.Interfaces.Handlers.Commands;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Fibo.Business.Interfaces.Handlers;
using Fibo.Business.Interfaces.Services;

namespace Fibo.App.SecondTests.IoC
{
    public class FibbonacciServicesContainerTests : BaseIocTests
    {
        [Test]
        public void AddFibonacciServices()
        {
            var provider = _serviceCollection.AddFibonacciServices(_configuration).BuildServiceProvider();
            AssertNotNull<ICommandHandler<EvaluateFibonacciStepCommand>>(provider);
            AssertNotNull<IFibonacciEvaluationSender>(provider);
            AssertNotNull<IFibonacciEvaluationService>(provider);
            AssertNotNull<IFibonacciStorage>(provider);
        }
    }
}
