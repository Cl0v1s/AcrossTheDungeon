using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DW
{
    [Serializable]
    public class PacketRequest : Packet
    {
        public string request;

        public PacketRequest(string par1)
        {
            request=par1;
        }

        public override void processPacket(PacketManager par1)
        {
            //par1.handleRequest(this);
        }

    }


}
