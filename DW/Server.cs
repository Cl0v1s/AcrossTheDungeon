using System;
using System.Drawing;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SdlDotNet.Core;
using SdlDotNet.Input;

namespace DW
{
    class Server
    {
        private Stair[] stairs;
        private int stairNb = -1;
        IPEndPoint send;
        IPEndPoint listen = new IPEndPoint(IPAddress.Any, 1212);
        private Thread exchange;
        private UdpClient client;
        private bool connected = false;
        private bool online;
        OtherPlayer other;
        private Chat chat;

        public Server(bool par1 = false)
        {
            int rand = new Random().Next(10, 25);
            online = par1;
            stairs = new Stair[rand];
            up(DW.player);
            spawnPlayer(DW.player);
            chat = new Chat();
            if (online == true)
                createConnexion();
        }

        public Server(int stairsNumber)
        {
            stairs = new Stair[stairsNumber];
        }

        private void spawnPlayer(Player par1)
        {
            Point p = par1.getStair().getFreeSpecialCase();
            par1.setPos(p.X, p.Y);
            par1.getStair().putEntity(par1);
            par1.setCanvas();
        }

        public void update()
        {
            if (chat != null)
                chat.update();
            DW.player.update();
        }

        public void up(Player par1)
        {
            if (stairNb < stairs.Length - 1)
            {
                if (stairs[par1.getStairId() + 1] == null)
                {
                    Stair s = new Stair();
                    stairNb += 1;
                    par1.changeStair(s, stairNb);
                    stairs[stairNb] = s;
                }
            }

        }

        public void roll()
        {

        }

        public bool isConnected()
        {
            return connected;
        }

        public void setConnected(bool par1)
        {
            connected = par1;
        }

        private void createConnexion()
        {
            client = new UdpClient(listen);
            exchange = new Thread(new ThreadStart(exchangeData));
            exchange.Start();
        }

        public UdpClient getClient()
        {
            return client;
        }

        public void showMsg(string par1)
        {
            chat.add(par1);
        }

        public void exchangeData()
        {
            while (true)
            {
                Packet packet = Packet.Receive(client);
                if (packet is CommandPacket)
                {
                    CommandPacket p = (CommandPacket)packet;
                    examine(p.getCommand(), p.getArgs());
                }
                else if (packet is ChatPacket && chat != null)
                {
                    chat.add(((ChatPacket)packet));
                }
            }

        }

        private void examine(string par1command, object par2args)
        {
            switch (par1command)
            {
                case "sendip":
                    if (par2args is IPAddress)
                    {
                        send = new IPEndPoint((IPAddress)par2args, 1213);
                        client.Connect(send);
                        chat.setOther(client);
                        setConnected(true);
                        Console.WriteLine("Client (" + par2args.ToString() + ") est connecté.");
                    }
                    break;
                case "getstair":
                    DataPacket ap = new DataPacket(DW.player.getStair());
                    Packet.Send(ap, client, send);
                    break;
                case "sendplayer":
                    if (par2args is OtherPlayer)
                    {
                        other = (OtherPlayer)par2args;
                        other.setStair(DW.player.getStair());
                        Point p = other.getStair().getFreeSpecialCase();
                        other.setPos(p.X, p.Y);
                        DW.player.getStair().putEntity(other);
                        Packet.Send(new DataPacket(other), client);
                    }
                    break;
                case "moveplayer":
                    if (par2args is Point)
                    {
                        Point p = (Point)par2args;
                        other.move(p.X, p.Y);
                        Packet.Send(new DataPacket(other), client);
                    }
                    break;
                case "updatechat":
                    Packet.Send(new DataPacket(chat.getValues()), client);
                    break;
                case "interactplayer":
                    other.interact();
                    Packet.Send(new DataPacket(other), client);
                    break;

            }
            //Console.WriteLine("Execution de " + par1command);
        }
    }
}