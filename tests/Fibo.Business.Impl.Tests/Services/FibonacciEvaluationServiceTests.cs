using Fibo.Business.Impl.Services;
using Fibo.Business.Interfaces.Services;
using log4net.Core;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fibo.Business.Impl.Tests.Services
{
    public class FibonacciEvaluationServiceTests : BaseTests
    {
        private Mock<IFibonacciStorage> _storageMock;
        private FibonacciEvaluationService _sut;

        [SetUp]
        public void Setup()
        {
            _storageMock = new Mock<IFibonacciStorage>();
            _sut = new FibonacciEvaluationService(_storageMock.Object);
        }

        [TestCase(10, 1, 2, 3)]
        [TestCase(100, 101, 202, 303)]
        [TestCase(1000, int.MaxValue, int.MaxValue, 4294967294)]
        public void EvaluateNextSuccess(int currentCount, long currentValue, long previouseValue, long expectedValue)
        {
            _storageMock
                .Setup(s => s.Get(currentCount - 1))
                .Returns(previouseValue)
                .Verifiable();

            var result = _sut.EvaluateNext(currentCount, currentValue);
            _storageMock.Verify();
            Assert.AreEqual(result.Count, currentCount + 1);
            Assert.AreEqual(result.Value, expectedValue);

            _storageMock.Verify(s => s.Add(currentCount, currentValue), Times.Once);
            _storageMock.Verify(s => s.Add(currentCount + 1, expectedValue), Times.Once);
        }

        [Test]
        public void EvaluateNextOverflow()
        {
            _storageMock
                .Setup(s => s.Get(It.IsAny<int>()))
                .Returns(1);

            Assert.Catch<OverflowException>(() => _sut.EvaluateNext(1, long.MaxValue));

            AssertLog((Level.Info, "-----------Evaluation ended------------"));
        }
    }
}
