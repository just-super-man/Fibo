using EasyNetQ;
using Fibo.Business.Impl.Services.EvaluationSenders;
using Fibo.Common.Contracts.Messages;
using log4net.Core;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fibo.Business.Impl.Tests.Services.EvaluationSenders
{
    public class FibonacciEvaluationBusSenderTest : BaseTests
    {
        private Mock<IBus> _busMock;

        public FibonacciEvaluationBusSender _sut { get; private set; }

        [SetUp]
        public void Setup()
        {
            _busMock = new Mock<IBus>();
            _sut = new FibonacciEvaluationBusSender(_busMock.Object);
        }

        [Test]
        public async Task SendAsync()
        {
            var chainId = Guid.NewGuid();
            await _sut.SendAsync(1, 20, chainId);

            _busMock.Verify(b => b.PublishAsync(It.Is<FibonacciMessage>(m =>
                m.Count == 1 &&
                m.Value == 20 &&
                m.ChainId == chainId
            )), Times.Once);

            AssertLog((Level.Info, $"{chainId} send_bus count {1} value {20}"));
        }
    }
}
