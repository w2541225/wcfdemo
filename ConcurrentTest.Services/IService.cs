using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;


namespace ConcurrentTest.Services
{

    [ServiceContract]
    interface IService
    {
        [OperationContract]
        string Query(string input);
    }
}
