using System;
using System.ServiceModel.Dispatcher;

namespace BetterParameterInspector
{
    class BetterParameterInspectorOperationInvoker : IOperationInvoker
    {
        string operationName;
        IOperationInvoker originalInvoker;
        IBetterParameterInspector inspector;
        public BetterParameterInspectorOperationInvoker(string operationName, IOperationInvoker originalInvoker, IBetterParameterInspector inspector)
        {
            this.operationName = operationName;
            this.originalInvoker = originalInvoker;
            this.inspector = inspector;
        }

        public object[] AllocateInputs()
        {
            return this.originalInvoker.AllocateInputs();
        }

        public object Invoke(object instance, object[] inputs, out object[] outputs)
        {
            object correlationState = this.inspector.BeforeCall(this.operationName, inputs);
            object result = this.originalInvoker.Invoke(instance, inputs, out outputs);
            this.inspector.AfterCall(this.operationName, outputs, ref result, correlationState);
            return result;
        }

        public IAsyncResult InvokeBegin(object instance, object[] inputs, AsyncCallback callback, object state)
        {
            object correlationState = this.inspector.BeforeCall(this.operationName, inputs);
            if (correlationState != null)
            {
                throw new InvalidOperationException("Correlation not implemented yet for asynchronous methods");
            }

            return this.originalInvoker.InvokeBegin(instance, inputs, callback, state);
        }

        public object InvokeEnd(object instance, out object[] outputs, IAsyncResult result)
        {
            object operationResult = this.originalInvoker.InvokeEnd(instance, out outputs, result);
            this.inspector.AfterCall(this.operationName, outputs, ref operationResult, null);
            return operationResult;
        }

        public bool IsSynchronous
        {
            get { return this.originalInvoker.IsSynchronous; }
        }
    }
}
