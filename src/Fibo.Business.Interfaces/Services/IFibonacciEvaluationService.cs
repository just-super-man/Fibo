namespace Fibo.Business.Interfaces.Services
{
    public interface IFibonacciEvaluationService
    {
        (int Count, long Value) EvaluateNext(int count, long value);
    }
}
