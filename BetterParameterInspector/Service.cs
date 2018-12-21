using System;

namespace BetterParameterInspector
{
    public class Service : ITest, ITestWithAsyncOperation
    {
        public int Add(int x, int y)
        {
            int result = x + y;
            Console.WriteLine("{0} + {1} = {2}", x, y, result);
            return result;
        }

        public int Subtract(int x, int y)
        {
            int result = x - y;
            Console.WriteLine("{0} - {1} = {2}", x, y, result);
            return result;
        }

        public int Divide(int x, int y)
        {
            int result = x / y;
            Console.WriteLine("{0} / {1} = {2}", x, y, result);
            return result;
        }

        public int Multiply(int x, int y)
        {
            int result = x * y;
            Console.WriteLine("{0} * {1} = {2}", x, y, result);
            return result;
        }

        public IAsyncResult BeginEchoString(string text, AsyncCallback callback, object userState)
        {
            Func<string, string> func = this.EchoStringDoWork;
            return func.BeginInvoke(text, callback, userState);
        }

        public string EndEchoString(IAsyncResult asyncResult)
        {
            Func<string, string> func = ((System.Runtime.Remoting.Messaging.AsyncResult)asyncResult).AsyncDelegate as Func<string, string>;
            return func.EndInvoke(asyncResult);
        }

        private string EchoStringDoWork(string input) { return input; }
    }
}
