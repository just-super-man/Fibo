using System;

namespace Fibo.Business.Interfaces.Handlers.Commands
{
    public class EvaluateFibonacciStepCommand
    {
        public Guid ChainId { get; set; }

        public int Count { get; set; }

        public long Value { get; set; }

        public EvaluationSource Source { get; set; }
    }
}
