using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace BetterParameterInspector
{
    //代码下载自：https://code.msdn.microsoft.com/Better-parameter-inspector-1b8cbf4d
    class Program
    {
        class NoNegativesInspector : IBetterParameterInspector
        {
            public object BeforeCall(string operationName, object[] inputs)
            {
                for (int i = 0; i < inputs.Length; i++)
                {
                    inputs[i] = Math.Abs((int)inputs[i]);
                }

                return null;
            }

            public void AfterCall(string operationName, object[] outputs, ref object returnValue, object correlationState)
            {
                returnValue = Math.Abs((int)returnValue);
                if (outputs != null)
                {
                    for (int i = 0; i < outputs.Length; i++)
                    {
                        outputs[i] = Math.Abs((int)outputs[i]);
                    }
                }
            }
        }

        class NoNullReturnValue : IBetterParameterInspector
        {
            public object BeforeCall(string operationName, object[] inputs)
            {
                return null;
            }

            public void AfterCall(string operationName, object[] outputs, ref object returnValue, object correlationState)
            {
                if (returnValue == null)
                {
                    returnValue = "<<null>>";
                }
            }
        }

        static void Main(string[] args)
        {
            string baseAddress = "http://" + Environment.MachineName + ":8000/Service";
            ServiceHost host = new ServiceHost(typeof(Service), new Uri(baseAddress));
            ServiceEndpoint endpoint = host.AddServiceEndpoint(typeof(ITest), new BasicHttpBinding(), "");
            ServiceEndpoint asyncEndpoint = host.AddServiceEndpoint(typeof(ITestWithAsyncOperation), new BasicHttpBinding(), "async");

            foreach (OperationDescription od in endpoint.Contract.Operations)
            {
                od.Behaviors.Add(new BetterParameterInspectorOperationBehavior(new NoNegativesInspector()));
            }

            asyncEndpoint.Contract.Operations.Find("EchoString").Behaviors.Add(
                new BetterParameterInspectorOperationBehavior(new NoNullReturnValue()));
            host.Open();

            ChannelFactory<ITest> factory = new ChannelFactory<ITest>(new BasicHttpBinding(), new EndpointAddress(baseAddress));
            ITest proxy = factory.CreateChannel();
            Console.WriteLine("Abs(Abs(33) + Abs(-44)) = {0}", proxy.Add(33, -44));
            Console.WriteLine("Abs(Abs(33) - Abs(-44)) = {0}", proxy.Subtract(33, -44));
            Console.WriteLine("Abs(Abs(33) * Abs(-44)) = {0}", proxy.Multiply(33, -44));
            ((IClientChannel)proxy).Close();
            factory.Close();

            ChannelFactory<ITestWithAsyncOperation> asyncFactory = new ChannelFactory<ITestWithAsyncOperation>(new BasicHttpBinding(), new EndpointAddress(baseAddress + "/async"));
            ITestWithAsyncOperation asyncProxy = asyncFactory.CreateChannel();
            Console.WriteLine(asyncProxy.EndEchoString(asyncProxy.BeginEchoString("hello", null, null)));
            Console.WriteLine(asyncProxy.EndEchoString(asyncProxy.BeginEchoString(null, null, null)));
            ((IClientChannel)asyncProxy).Close();
            asyncFactory.Close();
        }
    }
}
