using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DW
{
    [Serializable]
    public class PacketStair : Packet
    {
        public Stair stair;

        public PacketStair(Stair par1)
        {
            stair = par1;
        }

        public override void processPacket(PacketManager par1)
        {
            par1.handleStair(this);
        }
    }
}
