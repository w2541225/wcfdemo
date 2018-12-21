using Clients.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clients
{
    class Program
    {
        static void Main(string[] args)
        {
            Service1Client service1Client = new Service1Client();
            service1Client.ChannelFactory.Open();
            IService1 service1 = service1Client.ChannelFactory.CreateChannel();
            var str = service1.GetData(2);
            Console.WriteLine(str);
            Console.ReadLine();
        }
    }
}
