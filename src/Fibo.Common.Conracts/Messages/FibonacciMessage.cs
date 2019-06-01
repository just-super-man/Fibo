using System;

namespace Fibo.Common.Contracts.Messages
{
    public class FibonacciMessage
    {
        public int Count { get; set; }

        public long Value { get; set; }

        public Guid ChainId { get; set; }
    }
}
