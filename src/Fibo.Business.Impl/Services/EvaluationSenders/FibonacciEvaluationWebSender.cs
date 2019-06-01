using Fibo.Business.Interfaces.Services;
using Fibo.Common.Contracts.Requests;
using log4net;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace Fibo.Business.Impl.Services.EvaluationSenders
{
    public class FibonacciEvaluationWebSender : IFibonacciEvaluationSender
    {
        private const string methodPath = "api/Fibonacci";
        private readonly ILog _logger = LogManager.GetLogger(typeof(FibonacciEvaluationWebSender));
        private readonly IRestClient _restClient;

        public FibonacciEvaluationWebSender(IRestClient restClient)
        {
            _restClient = restClient;
        }

        public async Task SendAsync(int count, long value, Guid chainId)
        {
            try
            {
                _logger.InfoFormat("{0} send_web count {1} value {2}", chainId, count, value);

                var request = new RestRequest(methodPath, Method.POST)
                    .AddJsonBody(new FibonacciRequest
                    {
                        Count = count,
                        Value = value,
                        ChainId = chainId
                    });

                var result = await _restClient.ExecuteTaskAsync(request);
                if (!result.IsSuccessful)
                {
                    _logger.Error(
                        $"{chainId} Request to {_restClient.BaseUrl} {methodPath} not successfull\n" +
                        $"Http status {result.StatusCode}\n" +
                        $"Body {result.Content}");
                }
                else
                {
                    _logger.Debug("Success");
                }
            }
            catch(Exception e)
            {
                _logger.Error("Unhandled exception occured", e);
            }
        }
    }
}
