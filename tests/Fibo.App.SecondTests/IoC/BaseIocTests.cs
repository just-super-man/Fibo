using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.IO;

namespace Fibo.App.Second.Tests.IoC
{
    public class BaseIocTests
    {
        protected IConfigurationRoot _configuration;
        protected ServiceCollection _serviceCollection;

        [SetUp]
        public void Setup()
        {
           var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            _configuration = builder.Build();

            _serviceCollection = new ServiceCollection();
        }

        protected void AssertNotNull<T>(ServiceProvider provider)
        {
            var service = provider.GetService<T>();
            service.Should().NotBeNull();
        }
    }
}