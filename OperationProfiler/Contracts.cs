using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace OperationProfiler
{
    [DataContract]
    public class Person
    {
        [DataMember]
        public string Name;
        [DataMember]
        public string Address;
    }

    [DataContract]
    public class Order
    {
        [DataMember]
        public int Id;
        [DataMember]
        public List<OrderItem> Items;
        [DataMember]
        public Person Client;
    }

    [DataContract]
    public class OrderItem
    {
        [DataMember]
        public string Name;
        [DataMember]
        public string Unit;
        [DataMember]
        public double UnitPrice;
        [DataMember]
        public double Amount;
    }

    [ServiceContract]
    public interface ITest
    {
        [OperationContract]
        int Add(int x, int y);
        [OperationContract]
        int ProcessOrder(Order order);
        [OperationContract(IsOneWay = true)]
        void ProcessOneWay(Order order);
    }
}
