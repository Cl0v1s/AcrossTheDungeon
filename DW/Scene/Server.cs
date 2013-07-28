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
        //server
        private UdpClient server;
        private IPEndPoint customer;
        private PacketManager packetManager=new PacketManager();
        private Thread listen;
        private Thread talk;
        private Packet[] queue = new Packet[20];
        public OtherPlayer other;
        private string[] otherChatMsg = new string[6];
        private int otherChatMsgIndex = 0;
        private int clientFrame = 0;
        //local
        private Stair[] stairs;
        private int stairNb = -1;
        private Chat chat;


        public Server(bool par1 = false)
        {
            int rand = new Random().Next(10, 25);
            stairs = new Stair[rand];
            up(DW.player);
            spawnPlayer(DW.player);
            chat = new Chat();
            waitForConnexion();
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
            inputUpdate();
            DW.player.update();
            DW.player.getStair().time();
            if(customer != null)
                updateClientData();
            if (chat != null)
                chat.update();
        }

        //<summary>
        //met à jour les informations coté client
        //</summary>
        private void updateClientData()
        {
            clientFrame += 1;
            if (clientFrame >= 30)
            {
                if(other != null)
                    addPacketToQueue(new PacketPlayer(other));
                clientFrame = 0;
            }
        }

        private void inputUpdate()
        {
            if (DW.render.isUIOpenned())
                return;
            if (DW.input.equals(Key.I))
            {
                DW.render.openInventory();
            }
            if (DW.input.equals(Key.C))
            {
                DW.render.setRecipe(null);
                DW.render.openRecipe();
            }
            if (DW.input.equals(Key.B))
            {
                DW.player.getInventory().addItem(DW.player.getItemInHand(), false);
                DW.player.setItemInHand(null);
            }
            if (DW.input.equals(Key.UpArrow) == true)
            {
                DW.player.move(0, -1);
                DW.player.setFace("back");
            }
            else if (DW.input.equals(Key.DownArrow) == true)
            {
                DW.player.move(0, 1);
                DW.player.setFace("front");
            }
            else if (DW.input.equals(Key.RightArrow) == true)
            {
                DW.player.move(1, 0);
                DW.player.setFace("right");
            }
            else if (DW.input.equals(Key.LeftArrow) == true)
            {
                DW.player.move(-1, 0);
                DW.player.setFace("left");
            }
            else if (DW.input.equals(Key.KeypadEnter) || DW.input.equals(Key.Return))
            {
                DW.player.interact();
                Thread.Sleep(200);
            }
            else if (DW.input.equals(Key.Space))
            {
                DW.player.attack();
            }
            else if (DW.input.equals(Key.L))
                DW.player.lap();
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

        //<summary>
        //attends la connexion d'un autre joueur
        //</summary>
        private void waitForConnexion()
        {
            server = new UdpClient(1212);
            showMsg("Server Started",DW.player);
            listen = new Thread(new ThreadStart(listenData));
            listen.Start();
            talk = new Thread(new ThreadStart(talkData));
            talk.Start();
        }

        //<summary>
        //retourne le gestionnaire de connexion coté serveur
        //</summary>
        public UdpClient getServer()
        {
            return server;
        }

        //<summary>
        //ecoute le gestionnaire de connexion afin de recevoir des paquets et de la traiter
        //</summary>
        public void listenData()
        {
            while (true)
            {
                Packet packet = Packet.Receive(server);
                packet.processPacket(packetManager);
            }
        }

        //<summary>
        //parcours la liste de paquet a envoyer et envoie les paquets
        //</summary>
        public void talkData()
        {
            int queueIndex = 0;
            while (true)
            {
                if (queueIndex < queue.Length && queue[queueIndex] != null)
                {
                    Console.WriteLine("Sending " + queue[queueIndex]);
                    Packet.Send(queue[queueIndex], server, customer);
                    queue[queueIndex] = null;
                    queueIndex += 1;
                }
                else if (queueIndex < queue.Length)
                    queueIndex = 0;
                Thread.Sleep(200);
            }
        }

        public void addPacketToQueue(Packet par1)
        {
            for (int i = 0; i < queue.Length; i++)
            {
                if (queue[i] == null)
                {
                    queue[i] = par1;
                    break;
                }
            }
        }

        //<summary>
        //paramètre l'adresse de destination des paquets
        //</summary>
        public void setCustomer(IPEndPoint par1)
        {
            customer = par1;
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

    }
}