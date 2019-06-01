using System;

namespace Fibo.Common.Contracts.Requests
{
    public class FibonacciRequest
    {
        public int Count { get; set; }

        public long Value { get; set; }

        public Guid ChainId { get; set; }
    }
}
