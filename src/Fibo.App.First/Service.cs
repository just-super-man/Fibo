using EasyNetQ;
using Fibo.App.First.Configs;
using Fibo.Business.Impl.Services.EvaluationSenders;
using Fibo.Business.Interfaces.Handlers;
using Fibo.Business.Interfaces.Handlers.Commands;
using Fibo.Common.Contracts.Messages;
using log4net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Fibo.App.First
{
    class Service : IHostedService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IOptions<BusSenderConfig> _busSenderConfig;
        private readonly IOptions<StartConfig> _starConfig;
        private readonly ILog _logger = LogManager.GetLogger(typeof(Service));
        private readonly IBus _bus;

        public Service(
            IBus bus,
            IServiceScopeFactory serviceScopeFactory,
            IOptions<BusSenderConfig> busSenderConfig,
            IOptions<StartConfig> starConfig
            )
        {
            _bus = bus;
            _serviceScopeFactory = serviceScopeFactory;
            _busSenderConfig = busSenderConfig;
            _starConfig = starConfig;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _bus.SubscribeAsync<FibonacciMessage>("fiboqwerty", async m =>
            {
                _logger.InfoFormat("first recived {0} count {1} value {2}", m.ChainId, m.Count, m.Value);

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var handler = scope.ServiceProvider.GetService<ICommandHandler<EvaluateFibonacciStepCommand>>();
                    await handler.ExecuteAsync(new EvaluateFibonacciStepCommand
                    {
                        Count = m.Count,
                        Value = m.Value,
                        Source = EvaluationSource.FirstService,
                        ChainId = m.ChainId
                    });
                }
            }, c => c.WithQueueName(_busSenderConfig.Value.QueueName));

            InitCalculation();

            return Task.CompletedTask;
        }

        private void InitCalculation()
        {
            var startDelayInSeconds = _starConfig.Value.StartDelayInSeconds;
            Thread.Sleep(1000 * startDelayInSeconds);

            var numberOfInitialRequests = _starConfig.Value.NumberOfInitialRequests;

            var tasks = new Task[numberOfInitialRequests];
            for (var i = 0; i < numberOfInitialRequests; i++)
            {
                var task = new Task(InitialSend);
                task.Start();
                tasks[i] = task;
            }

            Task.WaitAll(tasks);
        }

        private void InitialSend()
        {
            var initCount = _starConfig.Value.InitCount;
            var initValue = _starConfig.Value.InitValue;
            var chainId = Guid.NewGuid();

            _logger.InfoFormat("frst init {0} count {1} value {2}", chainId, initCount, initValue);
            _bus.Publish(new FibonacciMessage
            {
                ChainId = chainId,
                Value = initValue,
                Count = initCount
            });
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
