using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Threading;

namespace ConcurrentTest.Clients
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceClient serviceClient = new ServiceClient();
            var channel = serviceClient.ChannelFactory.CreateChannel();
            string clientId = args.Length > 0 ? args[0] : DateTime.Now.ToString("HH:mm:ss fff");
            IList<Task<string>> results = new List<Task<string>>();
            for (int i = 1; i < 1001; i++)
            {
                string inputMessage = $"The client's id is {clientId}, the index is {i}。";
                Console.WriteLine($"begin input: {inputMessage}");
                Task.Factory.StartNew(() =>
                {
                    var result = channel.Query(inputMessage);
                });


                Thread.Sleep(1000);
                //results.Add(result);

            }
            //foreach (var item in results)
            //{
            //    try
            //    {
            //        Console.WriteLine(item.Result);
            //    }
            //    catch (Exception e)
            //    {

            //        Console.WriteLine(e.Message);
            //    }



            //}


            Console.ReadLine();
        }
    }
}
