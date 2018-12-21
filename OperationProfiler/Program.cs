using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace OperationProfiler
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseAddress = "http://" + Environment.MachineName + ":8000/Service";
            ServiceHost host = new ServiceHost(typeof(Service), new Uri(baseAddress));
            ServiceEndpoint endpoint = host.AddServiceEndpoint(typeof(ITest), new BasicHttpBinding(), "");
            OperationProfilerManager serverProfilerManager = new OperationProfilerManager();
            endpoint.Behaviors.Add(new OperationProfilerEndpointBehavior(serverProfilerManager));
            Console.WriteLine("Opening the host");
            host.Open();

            ChannelFactory<ITest> factory = new ChannelFactory<ITest>(new BasicHttpBinding(), new EndpointAddress(baseAddress));
            OperationProfilerManager clientProfilerManager = new OperationProfilerManager();
            factory.Endpoint.Behaviors.Add(new OperationProfilerEndpointBehavior(clientProfilerManager));
            ITest proxy = factory.CreateChannel();

            Order oneOrder = new Order
            {
                Id = 1,
                Client = new Person
                {
                    Name = "John Doe",
                    Address = "111 223th Ave",
                },
                Items = new List<OrderItem>
                {
                    new OrderItem { Name = "bread", Unit = "un", UnitPrice = 0.56, Amount = 3 },
                    new OrderItem { Name = "milk", Unit = "gal", UnitPrice = 2.79, Amount = 1 },
                    new OrderItem { Name = "eggs", Unit = "doz", UnitPrice = 2.23, Amount = 1 },
                }
            };

            Console.WriteLine("Calling some operations...");

            for (int i = 0; i < 200; i++)
            {
                proxy.Add(i, i * i);
                if ((i % 3) == 0)
                {
                    proxy.ProcessOneWay(oneOrder);
                }
                else
                {
                    proxy.ProcessOrder(oneOrder);
                }
            }

            Console.WriteLine("Now printing statistics (server)");
            serverProfilerManager.PrintSummary();

            Console.WriteLine();

            Console.WriteLine("Now printing statistics (client)");
            clientProfilerManager.PrintSummary();

            Console.WriteLine("Press ENTER to close");
            Console.ReadLine();
            host.Close();
            Console.WriteLine("Host closed");
        }
    }
}
