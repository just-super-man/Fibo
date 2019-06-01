using System.Threading.Tasks;
using Fibo.Business.Interfaces.Handlers;
using Fibo.Business.Interfaces.Handlers.Commands;
using Fibo.Business.Interfaces.Services;
using Fibo.Common.Contracts.Requests;
using Hangfire;
using log4net;
using Microsoft.AspNetCore.Mvc;

namespace Fibo.App.Second.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FibonacciController : ControllerBase
    {
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly ILog _log = LogManager.GetLogger(typeof(FibonacciController));

        public FibonacciController(
            IBackgroundJobClient backgroundJobClient
            )
        {
            _backgroundJobClient = backgroundJobClient;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] FibonacciRequest request)
        {
            _log.InfoFormat("{0} recieved count {1} value {2}", request.ChainId, request.Count, request.Value);

            _backgroundJobClient.Enqueue<ICommandHandler<EvaluateFibonacciStepCommand>>(h => h.ExecuteAsync(new EvaluateFibonacciStepCommand
            {
                Value = request.Value,
                Count = request.Count,
                Source = EvaluationSource.SecondService,
                ChainId = request.ChainId
            }));

            return Ok();
        }
    }
}
