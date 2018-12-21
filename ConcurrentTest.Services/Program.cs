using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;

namespace ConcurrentTest.Services
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host = BuildConcurrentCallLimitHost(GetCallHost(), 4, 2, 2);
            host.Opened += Host_Opened;
            host.Open();
            Console.ReadLine();
        }

        private static ServiceHost GetSampleHost()
        {
            return new ServiceHost(typeof(ServiceImp));
        }

        private static ServiceHost GetCallHost()
        {
            var host = new ServiceHost(typeof(ServiceImp));
            var serviceBehavior = host.Description.Behaviors.Find<ServiceBehaviorAttribute>();
            serviceBehavior.InstanceContextMode = InstanceContextMode.PerCall;
            return host;
        }

        private static ServiceHost GetSingletonHost()
        {
            var host = new ServiceHost(new ServiceImp());
            var serviceBehavior = host.Description.Behaviors.Find<ServiceBehaviorAttribute>();
            serviceBehavior.InstanceContextMode = InstanceContextMode.Single;
            return host;
        }

        private static ServiceHost GetMultipleThreadSingletonHost()
        {
            var host = GetSingletonHost();
            return BuildMultipleHost(host);
        }

        private static ServiceHost GetMultipleThreadSampleHost()
        {
            var host = GetSampleHost();
            return BuildMultipleHost(host);
        }

        private static ServiceHost BuildMultipleHost(ServiceHost host)
        {
            var serviceBehavior = host.Description.Behaviors.Find<ServiceBehaviorAttribute>();
            serviceBehavior.ConcurrencyMode = ConcurrencyMode.Multiple;
            return host;
        }

        private static ServiceHost BuildConcurrentCallLimitHost(ServiceHost host, int? callCount, int? sessionCount, int? instanceCount)
        {
            var throttlingBehavior = host.Description.Behaviors.Find<ServiceThrottlingBehavior>();
            if (throttlingBehavior == null)
            {
                throttlingBehavior = new ServiceThrottlingBehavior();
                host.Description.Behaviors.Add(throttlingBehavior);
            }
            if (callCount.HasValue)
            {
                throttlingBehavior.MaxConcurrentCalls = callCount.Value;
            }

            if (sessionCount.HasValue)
            {
                throttlingBehavior.MaxConcurrentSessions = sessionCount.Value;

            }

            if (instanceCount.HasValue)
            {
                throttlingBehavior.MaxConcurrentInstances = instanceCount.Value;

            }

            Console.WriteLine($"MaxConcurrentCalls is { throttlingBehavior.MaxConcurrentCalls}");
            Console.WriteLine($"MaxConcurrentSessions is { throttlingBehavior.MaxConcurrentSessions}");
            Console.WriteLine($"MaxConcurrentInstances is { throttlingBehavior.MaxConcurrentInstances}");

            return host;
        }

        private static void Host_Opened(object sender, EventArgs e)
        {
            Console.WriteLine("service is opened");
        }
    }
}
