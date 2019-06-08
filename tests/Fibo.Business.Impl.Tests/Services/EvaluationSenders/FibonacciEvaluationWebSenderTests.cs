using Fibo.Business.Impl.Services.EvaluationSenders;
using log4net.Core;
using Moq;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Fibo.Business.Impl.Tests.Services.EvaluationSenders
{
    public class FibonacciEvaluationWebSenderTests : BaseTests
    {
        private Mock<IRestClient> _restClientMock;
        private FibonacciEvaluationWebSender _sut;
        private Guid _chainId;

        [SetUp]
        public void Setup()
        {
            _restClientMock = new Mock<IRestClient>();
            _sut = new FibonacciEvaluationWebSender(_restClientMock.Object);
            _chainId = Guid.NewGuid();
        }

        [Test]
        public async Task SendAsyncException()
        {
            _restClientMock
                .Setup(s => s.ExecuteTaskAsync(It.IsAny<RestRequest>()))
                .Throws<Exception>();

            await _sut.SendAsync(1, 100, _chainId);

            AssertLog(
                (Level.Info, $"{_chainId} send_web count {1} value {2}"),
                (Level.Error, "Unhandled exception occured"));
        }

        [Test]
        public async Task SendAsyncSuccessfull()
        {
            var responseMock = new Mock<IRestResponse>();
            responseMock
                .Setup(r => r.IsSuccessful)
                .Returns(true);

            _restClientMock
                .Setup(s => s.ExecuteTaskAsync(It.IsAny<RestRequest>()))
                .ReturnsAsync(responseMock.Object);

            await _sut.SendAsync(1, 200, _chainId);

            AssertLog(
                (Level.Info, $"{_chainId} send_web count {1} value {2}"),
                (Level.Debug, "Success"));
        }

        [Test]
        public async Task SendAsyncFailed()
        {
            var responseMock = new Mock<IRestResponse>();
            responseMock.Setup(r => r.IsSuccessful).Returns(false);
            responseMock.Setup(r => r.StatusCode).Returns(HttpStatusCode.BadGateway);
            responseMock.Setup(r => r.Content).Returns("qwerty");

            Uri clientUri = new Uri("http://buyelefant.ru");
            _restClientMock.Setup(c => c.BaseUrl).Returns(clientUri);

            _restClientMock
                .Setup(s => s.ExecuteTaskAsync(It.IsAny<RestRequest>()))
                .ReturnsAsync(responseMock.Object);

            await _sut.SendAsync(1, 200, _chainId);

            AssertLog(
                (Level.Info, $"{_chainId} send_web count {1} value {2}"),
                (Level.Error, $"{_chainId} Request to http://buyelefant.ru api/Fibonacci not successfull\n" +
                    $"Http status {(int) HttpStatusCode.BadGateway}\n" +
                    $"Body qwerty"));
        }
    }
}
