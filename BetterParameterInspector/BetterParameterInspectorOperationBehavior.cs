using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace BetterParameterInspector
{
    public class BetterParameterInspectorOperationBehavior : IOperationBehavior
    {
        IBetterParameterInspector inspector;
        public BetterParameterInspectorOperationBehavior(IBetterParameterInspector inspector)
        {
            if (inspector == null)
            {
                throw new ArgumentNullException("inspector");
            }

            this.inspector = inspector;
        }

        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
        }

        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            dispatchOperation.Invoker = new BetterParameterInspectorOperationInvoker(operationDescription.Name, dispatchOperation.Invoker, this.inspector);
        }

        public void Validate(OperationDescription operationDescription)
        {
        }
    }
}
