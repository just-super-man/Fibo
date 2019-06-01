using System;
using System.Threading.Tasks;

namespace Fibo.Business.Interfaces.Services
{
    public interface IFibonacciEvaluationSender
    {
        Task SendAsync(int count, long value, Guid chainId);
    }
}
