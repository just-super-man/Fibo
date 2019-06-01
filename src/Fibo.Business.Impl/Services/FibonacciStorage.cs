using Fibo.Business.Interfaces.Services;
using System;
using System.Collections.Concurrent;

namespace Fibo.Business.Impl.Services
{
    public class FibonacciStorage : IFibonacciStorage
    {
        private ConcurrentDictionary<int, long> dictionary = new ConcurrentDictionary<int, long>();

        public FibonacciStorage()
        {
            // fibonachi initial data 
            dictionary.TryAdd(0, 0);
            dictionary.TryAdd(1, 1);
        }

        public void Add(int count, long value)
        {
            dictionary.TryAdd(count, value);
        }

        public long Get(int count)
        {
            if (dictionary.TryGetValue(count, out var value))
            {
                return value;
            }

            throw new ArgumentOutOfRangeException(nameof(count));
        }
    }
}
