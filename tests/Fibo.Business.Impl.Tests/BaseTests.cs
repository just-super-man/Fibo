using FluentAssertions;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using NUnit.Framework;
using System.Reflection;

namespace Fibo.Business.Impl.Tests
{
    public class BaseTests
    {
        protected MemoryAppender MemoryAppender;

        [SetUp]
        public void BaseSetup()
        {
            var logRepository = log4net.LogManager.GetRepository(Assembly.GetCallingAssembly());
            MemoryAppender = new MemoryAppender();
            BasicConfigurator.Configure(logRepository, MemoryAppender);
        }

        protected void AssertLog(params (Level level, string message)[] items)
        {
            var events = MemoryAppender.GetEvents();
            events.Should().BeEquivalentTo(items, c => c.ExcludingMissingMembers());
        }
    }
}