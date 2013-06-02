using System;


namespace DW
{
    [Serializable]
    class ChatPacket : Packet
    {
        private Player sender;
        private string msg;


        public ChatPacket(Player par1player, string par2msg)
            : base(TypePaquet.Chat)
        {
            sender = par1player;
            msg = par2msg;
        }

        public Player getSender()
        {
            return sender;
        }

        public string getMsg()
        {
            return msg;
        }
    }
}