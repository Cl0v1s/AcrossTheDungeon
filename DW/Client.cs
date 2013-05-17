using System;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Net;
using System.Net.Sockets;

using SdlDotNet.Core;
using SdlDotNet.Input;

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

           /* if (DW.input.equals(Key.T))
                isWriting = true;*/

            if (isWriting == false)
            {
                if (DW.input.equals(Key.UpArrow) == true)
                    changePlayerPos(DW.player.getX(), DW.player.getY() - 1);
                else if (DW.input.equals(Key.DownArrow) == true)
                    changePlayerPos(DW.player.getX(), DW.player.getY() + 1);
                else if (DW.input.equals(Key.RightArrow) == true)
                    changePlayerPos(DW.player.getX() + 1, DW.player.getY());
                else if (DW.input.equals(Key.LeftArrow) == true)
                    changePlayerPos(DW.player.getX() - 1, DW.player.getY());
                DW.render.move(DW.player.getX() * -30 + 640 / 2, DW.player.getY() * -30 + 480 / 2);
            }
            else
                isWriting=chat.write();
            DW.render.renderEntityVision(DW.player);
            DW.player.statUI.update();
            Thread.Sleep(100);
        }

        private void changePlayerPos(int par1x, int par2y)
        {
            Packet.Send(new CommandPacket("moveplayer", new Point(par1x, par2y)), server);
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
                //updateChat();
                Thread.Sleep(100);
            }
        }

        private void updateChat()
        {
            if (chat != null)
            {
                Packet.Send(new CommandPacket("updatechat"), server);

            }
        }



        private void sendPlayer()
        {
            CommandPacket ap = new CommandPacket("sendplayer", DW.player);
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