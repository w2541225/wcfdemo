using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CustomContractBehavior.DO
{
    public class Product : ICustomSerializable
    {
        public string Name;
        public string Unit;
        public int UnitPrice;

        public void WriteTo(Stream stream)
        {
            FormatHelper.WriteString(stream, this.Name);
            FormatHelper.WriteString(stream, this.Unit);
            FormatHelper.WriteInt(stream, this.UnitPrice);
        }

        public void InitializeFrom(Stream stream)
        {
            this.Name = FormatHelper.ReadString(stream);
            this.Unit = FormatHelper.ReadString(stream);
            this.UnitPrice = FormatHelper.ReadInt(stream);
        }
    }

}
