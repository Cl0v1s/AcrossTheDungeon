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

        //<summary>
        //inflige des degats à l'unité attaquée et paramètre son comportement en cas de danger de mort
        //</summary>
        //<param name="par2victim">l'entité victime de l'attaque</param>
        public void fight(Entity par1victim, int par2spellpower)
        {
            if (peur == 0)
                peur = -5;
            setEnemy(par1victim);
            par1victim.setEnemy(this);
            lookTo(par1victim);
            par1victim.lookTo(this);
            WantFight = true;
            par1victim.WantFight = true;
            int d = par2spellpower;
            if (d < 0)
                return;
            int atk = (int)(d * (enduranceTmp * 100 / endurance) / 100);
            double cc = rand.NextDouble();
            enduranceTmp -= atk * cc * enduranceTmp / 100;
            cc = rand.NextDouble();
            if (cc <= 1 / 280 * agilite)
                atk = (int)(atk * (1 + cc));
            atk = atk * (1 - (par1victim.getAgilite() / 100));
            par1victim.setLife(par1victim.getStat()[0] - atk);
            if (par1victim.getStat()[0] <= 10 * par1victim.getLife() / 100)
            {
                par1victim.WantFight = false;
                WantFight = false;
                par1victim.setFear(10);
            }

        }


    }
}