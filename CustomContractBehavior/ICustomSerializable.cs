using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CustomContractBehavior
{
    //https://blogs.msdn.microsoft.com/carlosfigueira/2011/03/28/wcf-extensibility-icontractbehavior/
    public interface ICustomSerializable
    {
        void WriteTo(Stream stream);
        void InitializeFrom(Stream stream);
    }
}
