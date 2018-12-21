using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace HelpPageEndpointBehavior
{
    class HelpPageInvoker : IOperationInvoker
    {
        HelpPageService service;
        public HelpPageInvoker(HelpPageService service)
        {
            this.service = service;
        }

        public object[] AllocateInputs()
        {
            return new object[1];
        }

        public object Invoke(object instance, object[] inputs, out object[] outputs)
        {
            outputs = new object[0];
            return this.service.Process((Message)inputs[0]);
        }

        public IAsyncResult InvokeBegin(object instance, object[] inputs, AsyncCallback callback, object state)
        {
            throw new NotSupportedException();
        }

        public object InvokeEnd(object instance, out object[] outputs, IAsyncResult result)
        {
            throw new NotSupportedException();
        }

        public bool IsSynchronous
        {
            get { return true; }
        }
    }
}
