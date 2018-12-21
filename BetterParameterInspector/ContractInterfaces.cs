using System;
using System.ServiceModel;

namespace BetterParameterInspector
{
    [ServiceContract]
    public interface ITest
    {
        [OperationContract]
        int Add(int x, int y);
        [OperationContract]
        int Subtract(int x, int y);
        [OperationContract]
        int Divide(int x, int y);
        [OperationContract]
        int Multiply(int x, int y);
    }

    [ServiceContract]
    public interface ITestWithAsyncOperation
    {
        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginEchoString(string text, AsyncCallback callback, object userState);
        string EndEchoString(IAsyncResult asyncResult);
    }
}
