using System;

using System.Drawing;

namespace DW
{
    [Serializable]
    public class PacketNewPlayer : Packet
    {
        public OtherPlayer player;

        public PacketNewPlayer(OtherPlayer par1)
        {
            player = par1;
        }

        public override void processPacket(PacketManager par1)
        {
            par1.handleNewPlayer(this);
        }
    }

    [Serializable]
    public class PacketPlayer : Packet
    {
        public OtherPlayer player;

        public PacketPlayer(OtherPlayer par1)
        {
            player = par1;
        }

        public override void processPacket(PacketManager par1)
        {
            par1.handlePlayer(this);
        }
    }

    [Serializable]
    public class PacketPlayerMove : Packet
    {
        public Point coords;
        public string face;

        public PacketPlayerMove(OtherPlayer par1)
        {
            coords = new Point(par1.x, par1.y);
            face = par1.getFace();
        }

        public override void processPacket(PacketManager par1)
        {
            par1.handlePlayerMove(this);
        }
    }

    [Serializable]
    public class PacketPlayerUseSpell : Packet
    {
        public int power;
        public Entity target;

        public PacketPlayerUseSpell(int par1spellpower, Entity par2target)
        {
            power = par1spellpower;
            target = par2target;
        }

        public override void processPacket(PacketManager par1)
        {
            par1.handlePlayerUseSpell(this);
        }
    }
}
