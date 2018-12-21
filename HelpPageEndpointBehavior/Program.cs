using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace HelpPageEndpointBehavior
{
    //代码下载自：https://code.msdn.microsoft.com/WCF-Custom-Help-Page-6f5a90f0

    [ServiceContract]
    public interface ITest
    {
        [OperationContract]
        int Add(int x, int y);
        [OperationContract]
        [Description("Multiplication operation")]
        int Multiply(int x, int y);
        [OperationContract]
        string Echo(string text);
    }

    public class Service : ITest
    {
        public int Add(int x, int y)
        {
            return x + y;
        }

        public int Multiply(int x, int y)
        {
            return x * y;
        }

        public string Echo(string text)
        {
            return text;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string baseAddress = "http://" + Environment.MachineName + ":8000/Service";
            ServiceHost host = new ServiceHost(typeof(Service), new Uri(baseAddress));
            ServiceEndpoint endpoint = host.AddServiceEndpoint(typeof(ITest), new BasicHttpBinding(), "basic");
            endpoint.Behaviors.Add(new HelpPageEndpointBehavior("ACME LLC"));
            host.Open();
            Console.WriteLine("Host opened");

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(baseAddress + "/basic/help");
            req.Method = "GET";
            HttpWebResponse resp;
            try
            {
                resp = (HttpWebResponse)req.GetResponse();
            }
            catch (WebException e)
            {
                resp = (HttpWebResponse)e.Response;
            }

            Console.WriteLine("HTTP/{0} {1} {2}", resp.ProtocolVersion, (int)resp.StatusCode, resp.StatusDescription);
            foreach (var header in resp.Headers.AllKeys)
            {
                Console.WriteLine("{0}: {1}", header, resp.Headers[header]);
            }
            if (resp.ContentLength > 0)
            {
                var rsp = new StreamReader(resp.GetResponseStream()).ReadToEnd();
                Console.WriteLine(rsp);
            }

            Console.WriteLine("Press ENTER to close");
            Console.ReadLine();
            host.Close();
        }
    }
}
