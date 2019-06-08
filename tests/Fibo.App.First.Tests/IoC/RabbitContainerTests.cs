using EasyNetQ;
using Fibo.App.First.IoC;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Fibo.App.First.Tests.IoC
{
    class RabbitContainerTests : BaseIocTests
    {
        [Test]
        public void AddRabbitService()
        {
            var provider = _serviceCollection.AddRabbitService(_configuration).BuildServiceProvider();
            AssertNotNull<IBus>(provider);
        }
    }
}
