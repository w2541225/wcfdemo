using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace HelpPageEndpointBehavior
{
    public class HelpPageEndpointBehavior : IEndpointBehavior
    {
        string companyName;
        public HelpPageEndpointBehavior(string companyName)
        {
            this.companyName = companyName;
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            Uri endpointAddress = endpoint.Address.Uri;
            if (endpointAddress.Scheme == Uri.UriSchemeHttp || endpointAddress.Scheme == Uri.UriSchemeHttps)
            {
                string address = endpointAddress.ToString();
                if (!address.EndsWith("/"))
                {
                    address = address + "/";
                }

                Uri helpPageUri = new Uri(address + "help");
                ServiceHostBase host = endpointDispatcher.ChannelDispatcher.Host;
                ChannelDispatcher helpDispatcher = this.CreateChannelDispatcher(host, endpoint, helpPageUri);
                host.ChannelDispatchers.Add(helpDispatcher);
            }
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }

        private ChannelDispatcher CreateChannelDispatcher(ServiceHostBase host, ServiceEndpoint endpoint, Uri helpPageUri)
        {
            Binding binding = new WebHttpBinding();
            EndpointAddress address = new EndpointAddress(helpPageUri);
            BindingParameterCollection bindingParameters = new BindingParameterCollection();
            IChannelListener channelListener = binding.BuildChannelListener<IReplyChannel>(helpPageUri, bindingParameters);
            ChannelDispatcher channelDispatcher = new ChannelDispatcher(channelListener, "HelpPageBinding", binding);
            channelDispatcher.MessageVersion = MessageVersion.None;

            HelpPageService service = new HelpPageService(endpoint, this.companyName);

            EndpointDispatcher endpointDispatcher = new EndpointDispatcher(address, "HelpPageContract", "", true);
            DispatchOperation operationDispatcher = new DispatchOperation(endpointDispatcher.DispatchRuntime, "GetHelpPage", "*", "*");
            operationDispatcher.Formatter = new PassthroughMessageFormatter();
            operationDispatcher.Invoker = new HelpPageInvoker(service);
            operationDispatcher.SerializeReply = false;
            operationDispatcher.DeserializeRequest = false;

            endpointDispatcher.DispatchRuntime.InstanceProvider = new SingletonInstanceProvider(service);
            endpointDispatcher.DispatchRuntime.Operations.Add(operationDispatcher);
            endpointDispatcher.DispatchRuntime.InstanceContextProvider = new SimpleInstanceContextProvider();
            endpointDispatcher.DispatchRuntime.SingletonInstanceContext = new InstanceContext(host, service);
            endpointDispatcher.ContractFilter = new MatchAllMessageFilter();
            endpointDispatcher.FilterPriority = 0;

            channelDispatcher.Endpoints.Add(endpointDispatcher);
            return channelDispatcher;
        }
    }
}
