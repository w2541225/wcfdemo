using System;
using System.Threading;

namespace OperationProfiler
{
    public class Service : ITest
    {
        static Random rndGen = new Random(1);
        public int Add(int x, int y)
        {
            return x + y;
        }

        public int ProcessOrder(Order order)
        {
            Thread.Sleep(rndGen.Next(1, 100));
            return order.Id;
        }

        public void ProcessOneWay(Order order)
        {
            Thread.Sleep(rndGen.Next(1, 100));
        }
    }
}
