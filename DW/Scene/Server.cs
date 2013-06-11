using System;
using System.Drawing;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SdlDotNet.Core;
using SdlDotNet.Input;
using SdlDotNet.Graphics.Sprites;
using SdlDotNet.Graphics;

namespace DW
{
    class Server
    {
        private Stair[] stairs;
        private int stairNb = -1;
        IPEndPoint send;
        IPEndPoint listen = new IPEndPoint(Dns.GetHostByName(Dns.GetHostName()).AddressList[0], 1212);
        private Thread exchange;
        private UdpClient client;
        private bool connected = false;
        private bool online;
        OtherPlayer other;
        private Chat chat;
        private string[] otherChatMsg = new string[6];
        private int otherChatMsgIndex = 0;
        private int tmp = 50;

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
            tmp = tmp - 1;
            if (tmp <= 0)
            {
                roll();
                tmp = 50;
            }
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
            DW.player.getStair().roll();
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
            showMsg("Serveur lancé à l'adresse locale "+listen.Address.ToString(),DW.player);
            exchange = new Thread(new ThreadStart(exchangeData));
            exchange.Start();
        }

        public UdpClient getClient()
        {
            return client;
        }

        public void showMsg(string par1,Player par2receiver)
        {
            if (par2receiver == DW.player)
                chat.add(par1);
            else
            {
                otherChatMsg[otherChatMsgIndex] = par1;
                otherChatMsgIndex += 1;
                if (otherChatMsgIndex > 5)
                    otherChatMsgIndex = 0;
            }

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
                    if (par2args is object[])
                    {
                        object[] o = (object[])par2args;
                        Point p = (Point)o[0];
                        other.move(p.X, p.Y);
                        other.setFace((string)o[1]);
                        Entity[] e = other.getStair().getEntities();
                        for (int i = 0; i < e.Length; i++)
                        {
                            if(e[i] != null && !(e[i] is Player) && other.isNear(e[i]))
                            {
                                other.fight(other,e[i]);
                                Console.WriteLine("other FIGHT");
                                break;
                            }
                        }
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
                case "getchatmsg":
                    Packet.Send(new DataPacket(otherChatMsg), client);
                    otherChatMsg = new string[6];
                    otherChatMsgIndex = 0;
                    break;

            }
            //Console.WriteLine("Execution de " + par1command);
        }
    }
}