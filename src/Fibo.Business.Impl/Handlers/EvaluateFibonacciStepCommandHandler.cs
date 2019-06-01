using Fibo.Business.Interfaces.Handlers;
using Fibo.Business.Interfaces.Handlers.Commands;
using Fibo.Business.Interfaces.Services;
using log4net;
using System;
using System.Threading.Tasks;

namespace Fibo.App.First.Handlers
{
    public class EvaluateFibonacciStepCommandHandler : ICommandHandler<EvaluateFibonacciStepCommand>
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(EvaluateFibonacciStepCommandHandler));
        private readonly IFibonacciEvaluationService _evaluationService;
        private readonly IFibonacciEvaluationSender _evaluationSender;

        public EvaluateFibonacciStepCommandHandler(
            IFibonacciEvaluationService evaluationService,
            IFibonacciEvaluationSender evaluationSender
            )
        {
            _evaluationService = evaluationService;
            _evaluationSender = evaluationSender;
        }

        public async Task ExecuteAsync(EvaluateFibonacciStepCommand message)
        {
            try
            {
                var result = _evaluationService.EvaluateNext(message.Count, message.Value);
                _logger.InfoFormat("{0} evaulated count {1} value {2}", 
                    message.ChainId, 
                    result.Count, 
                    result.Value);

                await _evaluationSender.SendAsync(result.Count, result.Value, message.ChainId);
            }
            catch (Exception e)
            {
                _logger.Error("Error occured", e);
            }
        }
    }
}
