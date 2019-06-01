using Hangfire;
using System;

namespace Fibo.App.Second.Hangfire
{
    public class ContainerJobActivator : JobActivator
    {
        private readonly IServiceProvider _serviceProvider;

        public ContainerJobActivator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override object ActivateJob(Type type)
        {
            return _serviceProvider.GetService(type);
        }
    }
}
