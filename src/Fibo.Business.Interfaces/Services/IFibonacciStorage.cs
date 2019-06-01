namespace Fibo.Business.Interfaces.Services
{
    public interface IFibonacciStorage
    {
        void Add(int count, long value);
        long Get(int count);
    }
}