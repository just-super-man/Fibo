using Fibo.App.First.IoC;
using Fibo.Business.Interfaces.Handlers;
using Fibo.Business.Interfaces.Handlers.Commands;
using Fibo.Business.Interfaces.Services;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using RestSharp;
using System.IO;

namespace Fibo.App.First.Tests.IoC
{
    public class FibbonacciServicesContainerTests : BaseIocTests
    {
        [Test]
        public void AddFibonacciServices()
        {
            var provider = _serviceCollection
                .AddFibonacciServices(_configuration)
                .BuildServiceProvider();

            AssertNotNull<ICommandHandler<EvaluateFibonacciStepCommand>>(provider);
            AssertNotNull<IFibonacciEvaluationSender>(provider);
            AssertNotNull<IRestClient>(provider);
            AssertNotNull<IFibonacciEvaluationService>(provider);
            AssertNotNull<IFibonacciStorage>(provider);
        }
    }
}
