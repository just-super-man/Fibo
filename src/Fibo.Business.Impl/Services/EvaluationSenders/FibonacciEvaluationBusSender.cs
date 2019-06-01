using Fibo.Business.Interfaces.Services;
using log4net;
using System.Threading.Tasks;
using EasyNetQ;
using Fibo.Common.Contracts.Messages;
using System;

namespace Fibo.Business.Impl.Services.EvaluationSenders
{
    public class FibonacciEvaluationBusSender : IFibonacciEvaluationSender
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(FibonacciEvaluationBusSender));

        private readonly IBus _bus;

        public FibonacciEvaluationBusSender(
            IBus bus
            )
        {
            _bus = bus;
        }

        public async Task SendAsync(int count, long value, Guid chainId)
        {
            _logger.InfoFormat("{0} send_bus count {1} value {2}", chainId, count, value);

            await _bus.PublishAsync(new FibonacciMessage
            {
                Count = count,
                Value = value,
                ChainId = chainId
            });
        }
    }
}
