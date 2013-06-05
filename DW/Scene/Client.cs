using System;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;

using SdlDotNet.Core;
using SdlDotNet.Input;
using SdlDotNet.Graphics.Sprites;
using SdlDotNet.Graphics;

namespace DW
{
    class Client
    {
        private Thread exchange;
        UdpClient server;
        IPEndPoint send;
        IPEndPoint listen = new IPEndPoint(IPAddress.Any, 1213);
        private bool connected = false;
        private Chat chat;
        private bool isWriting=false;


        public Client(String par1)
        {
            chat = new Chat();
            createConnexion(par1);
        }

        public void update()
        {
            if (chat != null)
            {
                chat.update();
            }

            if (isWriting == false)
            {
                if (DW.input.equals(Key.UpArrow) == true)
                    changePlayerPos(DW.player.getX(), DW.player.getY() - 1,"back");
                else if (DW.input.equals(Key.DownArrow) == true)
                    changePlayerPos(DW.player.getX(), DW.player.getY() + 1,"front");
                else if (DW.input.equals(Key.RightArrow) == true)
                    changePlayerPos(DW.player.getX() + 1, DW.player.getY(),"right");
                else if (DW.input.equals(Key.LeftArrow) == true)
                    changePlayerPos(DW.player.getX() - 1, DW.player.getY(),"left");
                else if (DW.input.equals(Key.L))
                    ((OtherPlayer)DW.player).lap();
                else if (DW.input.equals(Key.KeypadEnter))
                    interactPlayer();
                DW.render.move(DW.player.getX() * -30 + 640 / 2, DW.player.getY() * -30 + 480 / 2);
            }
            else
                isWriting=chat.write();
            DW.render.renderEntityVision(DW.player);
            DW.player.statUI.update();
            Thread.Sleep(100);
        }

        private void changePlayerPos(int par1x, int par2y,string par3dir)
        {
            Packet.Send(new CommandPacket("moveplayer", new object[]{new Point(par1x, par2y),par3dir}), server);
            while (true)
            {
                Packet p = Packet.Receive(server);
                if (p is DataPacket && ((DataPacket)p).get() is OtherPlayer)
                {
                    DW.player = (OtherPlayer)((DataPacket)p).get();
                    break;
                }
                else
                    break;
            }
        }

        private void interactPlayer()
        {
            Packet.Send(new CommandPacket("interactplayer"), server);
            while (true)
            {
                Packet p = Packet.Receive(server);
                if (p is DataPacket && ((DataPacket)p).get() is OtherPlayer)
                {
                    DW.player = (OtherPlayer)((DataPacket)p).get();
                    Console.WriteLine("reussi");
                    break;
                }
                else
                    break;
            }
            Thread.Sleep(200);
        }

        public bool isConnected()
        {
            return connected;
        }

        public void setConnected(bool par1)
        {
            connected = par1;
        }

        private void createConnexion(String par1)
        {
            server = new UdpClient(listen);
            send = new IPEndPoint(IPAddress.Parse(par1), 1212);
            server.Connect(send);
            Packet.Send(new CommandPacket("sendip", Dns.GetHostByName(Dns.GetHostName()).AddressList[0]), server);
            setConnected(true);
            chat.setOther(server);
            exchange = new Thread(new ThreadStart(exchangeData));
            exchange.Start();


        }

        private void getChatMsg()
        {
            Packet.Send(new CommandPacket("getchatmsg"), server);
            while (true)
            {
                Packet p = Packet.Receive(server);
                if (p is DataPacket && ((DataPacket)p).get() is string[])
                {
                    string[] s = (string[])((DataPacket)p).get();
                    for (int i = 0; i < s.Length; i++)
                    {
                        if(s[i] != null && s[i] != "")
                            chat.add(s[i]);
                    }
                    break;
                }
                else 
                    break;
            }
        }

        public UdpClient getServer()
        {
            return server;
        }

        public void showMsg(string par1)
        {
            chat.add(par1);
        }

        public void exchangeData()
        {
            sendPlayer();
            while (true)
            {
                updateStair();
                getChatMsg();
                Thread.Sleep(100);
            }
        }

        private void sendPlayer()
        {
            CommandPacket ap = new CommandPacket("sendplayer", DW.player);
            Console.WriteLine(DW.player.GetType());
            Packet.Send(ap, server);
            while (true)
            {
                Packet p = Packet.Receive(server);
                if (p is DataPacket && ((DataPacket)p).get() is OtherPlayer)
                {
                    DW.player = (OtherPlayer)((DataPacket)p).get();
                    Console.WriteLine("Joueur " + DW.player.getName() + " inséré en " + DW.player.getX() + ":" + DW.player.getY());
                    break;
                }
            }
        }

        private void updateStair()
        {
            CommandPacket ap = new CommandPacket("getstair");
            Packet.Send(ap, server);
            Stair stair = null;
            while (stair == null)
            {
                Packet d = Packet.Receive(server);
                if (d is DataPacket && ((DataPacket)d).get() is Stair)
                {
                    stair = (Stair)((DataPacket)d).get();
                }
                else
                    return;
   
            }
            Entity[] e = stair.getEntities();
            for (int i = 0; i < e.Length; i++)
            {
                if (e[i] != null && e[i] is Player && e[i].getName() == DW.player.getName())
                {
                    DW.player = (Player)e[i];
                }
            }
            DW.player.setStair(stair);
        }
    }
}