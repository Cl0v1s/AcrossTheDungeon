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

        //<summary>
        //créer le client
        //</summary>
        //<param name="par1">l'adresse ip du serveur à rejoindre</param>
        public Client(String par1)
        {
            chat = new Chat();
            createConnexion(par1);
        }

        private void inputUpdate()
        {
            if (DW.input.equals(Key.I))
            {
                DW.render.openInventory();
            }
            if (DW.render.isUIOpenned())
                return;
            if (DW.input.equals(Key.UpArrow) == true)
                changePlayerPos(DW.player.getX(), DW.player.getY() - 1, "back");
            else if (DW.input.equals(Key.DownArrow) == true)
                changePlayerPos(DW.player.getX(), DW.player.getY() + 1, "front");
            else if (DW.input.equals(Key.RightArrow) == true)
                changePlayerPos(DW.player.getX() + 1, DW.player.getY(), "right");
            else if (DW.input.equals(Key.LeftArrow) == true)
                changePlayerPos(DW.player.getX() - 1, DW.player.getY(), "left");
            else if (DW.input.equals(Key.L))
                ((OtherPlayer)DW.player).lap();
            else if (DW.input.equals(Key.KeypadEnter) || DW.input.equals(Key.Return))
                interactPlayer();
            DW.render.move(DW.player.getX() * -30 + 640 / 2, DW.player.getY() * -30 + 480 / 2);
        }

        //<summary>
        //actualise le client de manière à géréer les entrées de touches et à afficher le donjon
        //</summary>
        public void update()
        {
            inputUpdate();
            DW.render.renderEntityVision(DW.player);
            if (chat != null)
                chat.update();
            Thread.Sleep(100);
        }

        //<summary>
        //demande au serveur de changer la position du client dans le donjon 
        //</summary>
        //<param name="par1x">La postion x à atteindre</param>
        //<param name="par2">la position y à atteindre</param>
        //<param name="par3dir">la direction du sprite dui joueur à afficher</param>
        private void changePlayerPos(int par1x, int par2y,string par3dir)
        {
            Packet.Send(new CommandPacket("moveplayer", new object[]{new Point(par1x, par2y),par3dir}), server);
            while (true)
            {
                Packet p = Packet.Receive(server);
                if (p is DataPacket && ((DataPacket)p).get() is OtherPlayer)
                {
                    DW.player = (OtherPlayer)((DataPacket)p).get();
                    DW.player.turn();
                    break;
                }
                else
                    break;
            }
        }

        //<summary>
        //demande au serveur d'entammer une procédure de recherche d'élements avec lesquels le client peut interragir
        //</summary>
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

        //<summary>
        //retourne si le client est connecté ou non
        //</summary>
        public bool isConnected()
        {
            return connected;
        }

        public void setConnected(bool par1)
        {
            connected = par1;
        }

        //<summary>
        //entamme al procedure de connexion au client
        //</summary>
        //<param name="par1"> l'adresse ip du serveur à rejoindre</param>
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

        //<summary>
        //demande au serveur de retourner le contenu de la boite de chat
        //afin del'actualiser conté client
        //</summary>
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

        //<summary>
        //retourne le client udp chargé de la connexion
        //</summary>
        public UdpClient getServer()
        {
            return server;
        }

        //<summary>
        //ajoute un message dans le chat
        //</summary>
        public void showMsg(string par1)
        {
            chat.add(par1);
        }

        //<summary>
        //fnction chargé de l'actualisation des données entre le serveur et le client
        //</summary>
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

        //<summary>
        //envoie le joueur coté client au serveur afin de l'intégrer au jeu
        //</summary>
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

        //<summary>
        //fonction chargé d'actualiser le client en téléchargeant les données du donjon depuis le serveur
        //</summary>
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
                    DW.render.getStatUI().setOwner((Player)e[i]);
                    DW.player = (Player)e[i];
                    DW.render.setInventory(DW.player.getInventory());
                }
            }
            DW.player.setStair(stair);
        }
    }
}