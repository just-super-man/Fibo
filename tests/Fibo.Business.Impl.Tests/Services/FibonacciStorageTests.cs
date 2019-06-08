using Fibo.Business.Impl.Services;
using NUnit.Framework;
using System;

namespace Fibo.Business.Impl.Tests.Services
{
    public class FibonacciStorageTests : BaseTests
    {
        private FibonacciStorage _sut;

        [SetUp]
        public void Setup()
        {

            _sut = new FibonacciStorage();
        }

        [TestCase(0, 0)]
        [TestCase(1, 1)]
        public void CheckInitData(int count, long value)
        {
            var factValue = _sut.Get(count);
            Assert.AreEqual(value, factValue);
        }

        [TestCase(0, 100, 0)]
        [TestCase(1, 200, 1)]
        [TestCase(2, 300, 300)]
        [TestCase(int.MaxValue, long.MaxValue, long.MaxValue)]
        public void AddGetSuccess(int count, long value, long expected)
        {
            _sut.Add(count, value);
            var factValue = _sut.Get(count);
            Assert.AreEqual(expected, factValue);
        }

        [Test]
        public void GetException()
        {
            Assert.Catch<ArgumentOutOfRangeException>(() =>_sut.Get(777));
        }
    }
}
