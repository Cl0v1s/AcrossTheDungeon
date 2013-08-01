using System;

using System.Threading;
namespace DW
{
    [Serializable]
    public class OtherPlayer :  Player
    {
        public bool initialized = false;

        //<summary>
        //créer et gère le joueur coté client
        //</summary>
        public OtherPlayer(String par1name, String par2class, int par3force, int par4endurance, int par5volonte, int par6agilite)
            : base(par1name, par2class, par3force, par4endurance, par5volonte, par6agilite)
        {
            value = "à";
            inventory = new Inventory(this);
        }

        //<summary>
        //Fait boire le joueur coté client
        //</summary>
        public void lap()
        {
            if (isOn(100))
            {
                soif = soif - 25F;
                if (soif < 0)
                    soif = 0;
            }
        }

        //<summary>
        //met à jour l'état du joueur
        //</summary>
        public override bool update()
        {
            if (lifeTmp <= 0)
                dead = true;
            return dead;
        }

        //<summary>
        //permet au joueur coté client de se déplacer
        //</summary>
        public bool move(int par1x, int par2y)
        {
                if (canWalkOn(par1x, par2y))
                {
                    x = par1x;
                    y = par2y;
                    return true;
                }
            return false;
        }

        public override void attack(Spell par1)
        {
            Entity[] e = stair.getEntities();
            Entity target=null;
            for (int i = 0; i < e.Length; i++)
            {
                if (e[i] != null && !(e[i] is Player) && isNear(e[i]))
                {
                    if (face == "left" && y == e[i].getY() && x > e[i].getX())
                        target = e[i];
                    else if (face == "right" && y == e[i].getY() && x < e[i].getX())
                        target = e[i];
                    else if (face == "back" && y > e[i].getY() && x == e[i].getX())
                        target = e[i];
                    else if (face == "front" && y < e[i].getY() && x == e[i].getX())
                        target = e[i];
                    break;
                }
            }
            if (target != null)
            {
                DW.client.addPacketToQueue(new PacketPlayerUseSpell(par1.id, target));
                par1.useSpell(this, target);
            }
        }


    }
}