using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace OperationProfiler
{
    public class OperationProfilerEndpointBehavior : IEndpointBehavior
    {
        private OperationProfilerManager manager;
        public OperationProfilerEndpointBehavior(OperationProfilerManager manager)
        {
            this.manager = manager;
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            foreach (ClientOperation operation in clientRuntime.Operations)
            {
                operation.ParameterInspectors.Add(new OperationProfilerParameterInspector(this.manager, operation.IsOneWay));
            }
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            foreach (DispatchOperation operation in endpointDispatcher.DispatchRuntime.Operations)
            {
                operation.ParameterInspectors.Add(new OperationProfilerParameterInspector(this.manager, operation.IsOneWay));
            }
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }
}
