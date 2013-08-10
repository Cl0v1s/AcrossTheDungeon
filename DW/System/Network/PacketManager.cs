using System;
using System.Drawing;

namespace DW
{
    public class PacketManager
    {

        //<summary>
        //examine le paquet d'authentification envoyé par le client
        //</summary>
        //<param name="par1">paquet d'authentification</param>
        public void handleAuth(PacketAuth par1)
        {
            Console.WriteLine("Client connecté depuis " + par1.adress);
            DW.server.setCustomer(par1.adress);
            DW.server.addPacketToQueue(new PacketValidation());
        }

        //<summary>
        //examine le paquet contentant l'etage, en extrait de le joueur l'affecte au client puis affecte l'etage recu au joueur
        //</summary>
        //<param name="par1">paquet contenant l'etage</param>
        public void handleStair(PacketStair par1)
        {
            Stair stair = par1.stair;
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

        //<summary>
        //spawn le client sur dans l'etage du joueur coté serveur
        //</summary>
        public void handleNewPlayer(PacketNewPlayer par1)
        {
            OtherPlayer player = par1.player;
            Point p=DW.player.getStair().getFreeSpecialCase();
            player.setPos(p.X, p.Y);
            DW.player.getStair().putEntity(player);
            DW.server.other = player;
            DW.server.other.setStair(DW.player.getStair());
            Console.WriteLine("Joueur inséré en " + DW.server.other.x + ":" + DW.server.other.y);
            DW.server.addPacketToQueue(new PacketPlayer(DW.server.other));
        }

        //<summary>
        //recoit le joueur coté lient envoyé par le serveur suite à une modification d'etat
        //</summary>
        public void handlePlayer(PacketPlayer par1)
        {
            DW.player = par1.player;
        }

        //<summary>
        //indique au client que la connexion a bien été etablie entre le serveur et le client
        //</summary>
        public void handleValidation(PacketValidation par1)
        {
            DW.client.connected = true;
            Console.WriteLine("Connexion stable établie entre serveur et client.");
            DW.client.addPacketToQueue(new PacketNewPlayer((OtherPlayer)DW.player));
        }

        //<summary>
        //recu par le serveur, change la position du joueur client dans l'etage
        //</summary>
        public void handlePlayerMove(PacketPlayerMove par1)
        {
            DW.server.other.setPos(par1.coords.X, par1.coords.Y);
            DW.server.other.setFace(par1.face);
        }

        //<summary>
        //recu par le serveur, indique que le joueur a lancéun sort.
        //</summary>
        public void handlePlayerUseSpell(PacketPlayerUseSpell par1)
        {
            Console.WriteLine("before:" + par1.target.getStat()[0]);
            DW.server.other.fight(par1.target, par1.power);
            Console.WriteLine("after:" + par1.target.getStat()[0]);
        }
    }
}
