using CustomContractBehavior.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace CustomContractBehavior.Services
{

    [MyNewSerializerContractBehavior]
    [ServiceContract]
    public interface IOrderProcessing
    {
        [OperationContract]
        void ProcessOrder(Order order);
        [OperationContract]
        Order GetOrder(int id);
    }
}
