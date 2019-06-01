using Fibo.Business.Interfaces.Services;
using log4net;
using System;

namespace Fibo.Business.Impl.Services
{
    public class FibonacciEvaluationService : IFibonacciEvaluationService
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(FibonacciEvaluationService));
        private readonly IFibonacciStorage _fibonacciStorage;

        public FibonacciEvaluationService(
            IFibonacciStorage fibonacciStorage
            )
        {
            _fibonacciStorage = fibonacciStorage;
        }

        public (int Count, long Value) EvaluateNext(int count, long value)
        {
            _fibonacciStorage.Add(count, value);
            try
            {
                var prevValue = _fibonacciStorage.Get(count - 1);
                var nextValue = checked(value + prevValue);
                var nextCount = count + 1;
                _fibonacciStorage.Add(nextCount, nextValue);
                return (nextCount, nextValue);
            }
            catch (OverflowException e)
            {
                _logger.Info("-----------Evaluation ended------------", e);
                throw;
            }
        }
    }
}
