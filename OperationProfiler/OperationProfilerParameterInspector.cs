using System;
using System.ServiceModel.Dispatcher;

namespace OperationProfiler
{
    class OperationProfilerParameterInspector : IParameterInspector
    {
        OperationProfilerManager manager;
        bool isOneWay;
        public OperationProfilerParameterInspector(OperationProfilerManager manager, bool isOneWay)
        {
            this.manager = manager;
            this.isOneWay = isOneWay;
        }

        public void AfterCall(string operationName, object[] outputs, object returnValue, object correlationState)
        {
            DateTime endCall = DateTime.Now;
            DateTime startCall = (DateTime)correlationState;
            TimeSpan operationDuration = endCall.Subtract(startCall);
            this.manager.AddCall(operationName, operationDuration.TotalMilliseconds);
        }

        public object BeforeCall(string operationName, object[] inputs)
        {
            if (this.isOneWay)
            {
                this.manager.AddOneWayCall(operationName);
                return null;
            }
            else
            {
                return DateTime.Now;
            }
        }
    }
}
