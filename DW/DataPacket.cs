using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DW
{
    [Serializable]
    class DataPacket : Packet
    {
        private object data;

        public DataPacket(object par1)
            : base(TypePaquet.Data)
        {
            data = par1;
        }

        public object get()
        {
            return data;
        }

    }
}
