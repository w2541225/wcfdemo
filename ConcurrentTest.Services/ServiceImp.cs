using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;

namespace ConcurrentTest.Services
{
    public class ServiceImp : IService
    {
        static int  requstCount = 0;
        public string Query(string input)
        {
            requstCount = requstCount + 1;
            var hashCode = this.GetHashCode();
            Console.WriteLine($"this is :{hashCode}");
            //Console.WriteLine($"request count is :{requstCount}");
            var rsp = $"{DateTime.Now.ToString("HH:mm:ss fff")}[{Thread.CurrentThread.ManagedThreadId}] received:{input}";
            Console.WriteLine(rsp);
            Thread.Sleep(5000);
            return rsp;
        }
    }
}
