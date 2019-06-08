using Fibo.App.First.Handlers;
using Fibo.Business.Interfaces.Handlers.Commands;
using Fibo.Business.Interfaces.Services;
using log4net.Core;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Fibo.Business.Impl.Tests.Handlers
{
    public class EvaluateFibonacciStepCommandHandlerTests : BaseTests
    {
        private EvaluateFibonacciStepCommandHandler _sut;
        private Mock<IFibonacciEvaluationService> _evaluatingServices;
        private Mock<IFibonacciEvaluationSender> _evaluatingSender;

        [SetUp]
        public void Setup()
        {
            _evaluatingServices = new Mock<IFibonacciEvaluationService > ();
            _evaluatingSender = new Mock<IFibonacciEvaluationSender>();

            _sut = new EvaluateFibonacciStepCommandHandler(_evaluatingServices.Object, _evaluatingSender.Object);
        }

        [Test]
        public async Task ExecuteAsyncSuccess()
        {
            Guid chainId = Guid.NewGuid();
            var command = new EvaluateFibonacciStepCommand
            {
                ChainId = chainId,
                Count =  11,
                Value = 12L,
                Source = EvaluationSource.FirstService
            };

            _evaluatingServices
                .Setup(s => s.EvaluateNext(11, 12L))
                .Returns((21, 22L))
                .Verifiable();

            _evaluatingSender
                .Setup(s => s.SendAsync(21, 22L, chainId))
                .Returns(Task.CompletedTask)
                .Verifiable();

            await _sut.ExecuteAsync(command);
            _evaluatingServices.Verify();
            _evaluatingSender.Verify();

            AssertLog(
                (Level.Info, $"{chainId} evaulated count {21} value {22}"));
        }

        [Test]
        public async Task ExecuteAsyncException()
        {
            _evaluatingServices
                .Setup(s => s.EvaluateNext(It.IsAny<int>(), It.IsAny<long>()))
                .Throws<Exception>()
                .Verifiable();

            await _sut.ExecuteAsync(new EvaluateFibonacciStepCommand());

            _evaluatingServices.Verify();
            _evaluatingSender
                .Verify(s =>
                    s.SendAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()),
                    Times.Never);

            AssertLog((Level.Error, "Error occured"));
        }

        [Test]
        public async Task ExecuteAsyncExceptionWhileSending()
        {
            _evaluatingServices
                .Setup(s => s.EvaluateNext(It.IsAny<int>(), It.IsAny<long>()))
                .Returns((11, 22L))
                .Verifiable();
            _evaluatingSender
                .Setup(s => s.SendAsync(It.IsAny<int>(), It.IsAny<long>(), It.IsAny<Guid>()))
                .Throws<Exception>()
                .Verifiable();

            var chainId = Guid.NewGuid();
            await _sut.ExecuteAsync(new EvaluateFibonacciStepCommand { ChainId = chainId});

            _evaluatingServices.Verify();
            _evaluatingSender.Verify();
            AssertLog(
                (Level.Error, "Error occured"),
                (Level.Info, $"{chainId} evaulated count {21} value {22}"));
        }
    }
}