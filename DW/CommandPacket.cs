using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DW
{
    [Serializable]
    class CommandPacket : Packet
    {
        private String data;
        private object args;

        public CommandPacket(string par1command, object par2args=null)
            : base(TypePaquet.Command)
        {
            data = par1command;
            args = par2args;
        }

        public string getCommand()
        {
            return data;
        }

        public object getArgs()
        {
            return args;
        }
    }
}
