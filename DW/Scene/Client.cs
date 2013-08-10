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
        private Packet[] queue = new Packet[20];
        private Thread listen;
        private Thread talk;
        private UdpClient client;
        private IPEndPoint server;
        private int ValidationTimeOut = 100;
        public bool connected = false;
        private Chat chat;
        private PacketManager packetManager=new PacketManager();

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
            try
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
                if (DW.input.equals(Key.UpArrow) == true)
                    changePlayerPos(DW.player.x, DW.player.y - 1, "back");
                else if (DW.input.equals(Key.DownArrow) == true)
                    changePlayerPos(DW.player.x, DW.player.y + 1, "front");
                else if (DW.input.equals(Key.RightArrow) == true)
                    changePlayerPos(DW.player.x + 1, DW.player.y, "right");
                else if (DW.input.equals(Key.LeftArrow) == true)
                    changePlayerPos(DW.player.x - 1, DW.player.y, "left");
                else if (DW.input.equals(Key.L))
                    ((OtherPlayer)DW.player).lap();
                else if (DW.input.equals(Key.KeypadEnter) || DW.input.equals(Key.Return))
                    interactPlayer();
                DW.render.move(DW.player.x * -30 + 640 / 2, DW.player.y * -30 + 480 / 2);
                updateSpells();
            }
            catch (Exception)
            {

            }
        }

        private void updateSpells()
        {
//TOADD
        }


        //<summary>
        //actualise le client de manière à géréer les entrées de touches et à afficher le donjon
        //</summary>
        public void update()
        {
            if (!connected)
            {
                ValidationTimeOut -= 1;
                if (ValidationTimeOut < 0)
                {
                    listen.Abort();
                    talk.Abort();
                    DW.close(DW.client);
                    DW.changeScene("GameMenu", "Aucune réponse de l'hote distant.");
                }
            }
            inputUpdate();
            if(DW.render != null)
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
            DW.player.setFace(par3dir);
            if (((OtherPlayer)DW.player).move(par1x, par2y))
                Packet.Send(new PacketPlayerMove((OtherPlayer)DW.player),client,server);
            Thread.Sleep(50);
        }

        //<summary>
        //demande au serveur d'entammer une procédure de recherche d'élements avec lesquels le client peut interragir
        //</summary>
        private void interactPlayer()
        {
        }

        //<summary>
        //attends la connexion d'un autre joueur
        //</summary>
        private void createConnexion(string par1)
        {
            client = new UdpClient(1213);
            server = new IPEndPoint(IPAddress.Parse(par1), 1212);
            addPacketToQueue(new PacketAuth());
            listen = new Thread(new ThreadStart(listenData));
            listen.Start();
            talk = new Thread(new ThreadStart(talkData));
            talk.Start();
        }

        //<summary>
        //retourne le gestionnaire de connexion coté client
        //</summary>
        public UdpClient getClient()
        {
            return client;
        }

        //<summary>
        //ecoute le gestionnaire de connexion afin de recevoir des paquets et de la traiter
        //</summary>
        public void listenData()
        {
            while (true)
            {
                Packet packet = Packet.Receive(client);
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
                    Packet.Send(queue[queueIndex], client, server);
                    queue[queueIndex] = null;
                    queueIndex += 1;
                }
                else if (queueIndex < queue.Length)
                    queueIndex = 0;
                Thread.Sleep(200);
            }
        }

        //<summary>
        //ajoute un paquet à la file d'attente pour l'envoi vers le serveur
        //</summary>
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
        //ajoute un message dans le chat
        //</summary>
        public void showMsg(string par1)
        {
            chat.add(par1);
        }
    }
}