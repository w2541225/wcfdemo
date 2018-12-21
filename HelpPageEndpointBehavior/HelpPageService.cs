using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace HelpPageEndpointBehavior
{
    class HelpPageService
    {
        ServiceEndpoint endpoint;
        string companyName;
        public HelpPageService(ServiceEndpoint endpoint, string companyName)
        {
            this.endpoint = endpoint;
            this.companyName = companyName;
        }
        public Message Process(Message input)
        {
            return new HelpPageMessage(this.endpoint, this.companyName);
        }
    }
}
