using EasyNetQ;
using Fibo.App.Second.IoC;
using Microsoft.Extensions.DependencyInjection;

namespace Fibo.App.Second.Tests.IoC
{
    public class RabbitContainerTests : BaseIocTests
    {
        public void asdf()
        {
            var provider = _serviceCollection
                .AddRabbitService(_configuration)
                .BuildServiceProvider();

            AssertNotNull<IBus>(provider);
        }
    }
}
