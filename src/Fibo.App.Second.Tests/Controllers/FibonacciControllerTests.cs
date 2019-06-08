using Fibo.App.Second.Controllers;
using Fibo.Business.Interfaces.Handlers;
using Fibo.Business.Interfaces.Handlers.Commands;
using Fibo.Common.Contracts.Requests;
using FluentAssertions;
using Hangfire;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Fibo.App.Second.Tests.Controllers
{
    public class FibonacciControllerTests
    {
        private Mock<IBackgroundJobClient> _jobCLientMock;
        private FibonacciController _sut;
        private MemoryAppender _memoryAppender;

        [SetUp]
        public void Setup()
        {
            _jobCLientMock = new Mock<IBackgroundJobClient>();
            _sut = new FibonacciController(_jobCLientMock.Object);

            var logRepository = log4net.LogManager.GetRepository(Assembly.GetCallingAssembly());
            _memoryAppender = new MemoryAppender();
            BasicConfigurator.Configure(logRepository, _memoryAppender);
        }

        [Test]
        public async Task PostAsync()
        {
            var chainId = Guid.NewGuid();

            var result = await _sut.Post(new FibonacciRequest
            {
                ChainId = chainId,
                Count = 2,
                Value = 222
            });

            Assert.NotNull(result);
            Assert.IsInstanceOf<OkResult>(result);

            var events = _memoryAppender.GetEvents();
            
            events.Should().BeEquivalentTo(new[] {
                new
                {
                    level = Level.Info,
                    message = $"{chainId} recieved count {2} value {222}"
                }
            }, c => c.ExcludingMissingMembers());
        }
    }
}
