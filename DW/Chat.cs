using System;

using System.Net.Sockets;

namespace DW
{
    class Chat
    {

        private UdpClient other;
        private Text[] values;
        private string[] history;
        private bool change = false;
        private TextInput input;

        public Chat()
        {
            values = new Text[6];
            history = new string[6];
            for (int i = 0; i < values.Length; i++)
            {
                history[i] = "";
                values[i] = new Text("pixel.ttf", 15, 10, 100 - i * 15, "");
            }
            input = new TextInput(10, 115, 100, 20, 100);
            input.activate(false);
        }

        public void setOther(UdpClient par1)
        {
            other = par1;
        }

        public void send(Player par1player, string par2msg)
        {
            if (other != null)
            {
                Packet.Send(new ChatPacket(par1player, par2msg), other);
            }
        }

        public void add(ChatPacket par1)
        {
            Player p = par1.getSender();
            string m = par1.getMsg();
            string v = p.getName() + " : " + m;
            if (v != history[0])
            {
                for (int i = 5; i > 0; i--)
                {
                    history[i] = history[i - 1];
                }
                history[0] = v;
                change = true;
            }
            
        }

        public void add(string par1)
        {
            if (par1 != history[0])
            {
                for (int i = 5; i > 0; i--)
                {
                    history[i] = history[i - 1];
                }
                history[0] = par1;
                change = true;
            }
        }

        public void update()
        {
            if (change)
            {
                for (int i = 0; i < (values.Length); i++)
                {
                    values[i].changeText(history[i]);
                }
                change = false;
            }
	        for( int i=0;i<(values.Length);i++)
	        {
                values[(i)].update();
		    }
        }

        public string[] getValues()
        {
            string[] v = new string[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                v[i] = values[i].getText();
            }
            return v;
        }

        public void changeValues(string[] par1)
        {
            for (int i = 0; i < values.Length; i++)
            {
                values[i].changeText(par1[i]);
            }
        }

        public bool write()
        {
            input.activate(true);
            input.update();
            if (DW.input.equals(SdlDotNet.Input.Key.KeypadEnter))
            {
                if (DW.client != null)
                {
                    send(DW.player, input.getText());
                }
                else
                    add(new ChatPacket(DW.player, input.getText()));
                input.activate(false);
                input.reset();
                return false;
            }
            return true;
        }




    }
}