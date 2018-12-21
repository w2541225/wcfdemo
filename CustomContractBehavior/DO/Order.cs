using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CustomContractBehavior.DO
{
    public class Order : ICustomSerializable
    {
        public int Id;
        public Product[] Items;
        public DateTime Date;

        public void WriteTo(Stream stream)
        {
            FormatHelper.WriteInt(stream, this.Id);
            FormatHelper.WriteInt(stream, this.Items.Length);
            for (int i = 0; i < this.Items.Length; i++)
            {
                this.Items[i].WriteTo(stream);
            }

            FormatHelper.WriteLong(stream, this.Date.ToBinary());
        }

        public void InitializeFrom(Stream stream)
        {
            this.Id = FormatHelper.ReadInt(stream);
            int itemCount = FormatHelper.ReadInt(stream);
            this.Items = new Product[itemCount];
            for (int i = 0; i < itemCount; i++)
            {
                this.Items[i] = new Product();
                this.Items[i].InitializeFrom(stream);
            }

            this.Date = DateTime.FromBinary(FormatHelper.ReadLong(stream));
        }
    }
}
