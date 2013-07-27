using System;
using System.Net;

namespace DW
{
    [Serializable]
    public class PacketAuth : Packet
    {
        public IPEndPoint adress;

        public PacketAuth()
        {
            adress = new IPEndPoint(Dns.GetHostByName(Dns.GetHostName()).AddressList[0], 1213);
        }

        public override void processPacket(PacketManager par1)
        {
            par1.handleAuth(this);
        }
    }

    [Serializable]
    public class PacketValidation : Packet
    {

        public PacketValidation()
        {
            //TOADD (VERSION)
        }

        public override void processPacket(PacketManager par1)
        {
            par1.handleValidation(this);
        }
    }
}
