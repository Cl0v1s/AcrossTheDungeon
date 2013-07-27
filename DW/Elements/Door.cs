using System;

using System.Drawing;

namespace DW
{
    [Serializable]
    class Door : Special
    {
        private bool open;
        private int levelMax;
        private float level;
        private bool already=false;
        private Random rand=new Random();
        private string originalValue;


        //<summary>
        //créer et gère une porte
        //</summary>
        public Door(int[,] par1map,int par2x,int par3y)
            : base()
        {
            open = false;
            if (par1map[par2x, par3y - 1] == 2 && par1map[par2x, par3y + 1] == 2)
                value = "|o";
            else if (par1map[par2x - 1, par3y] == 2 && par1map[par2x + 1, par3y] == 2)
                value = "|";
            levelMax = rand.Next(1, 10);
            level = (float)levelMax;
            color = Color.Chocolate;
            originalValue = value;
        }

        //<summary>
        //retourne l'etat d'ouverture de la porte afin de savoir si un entitée peut passer
        //</summary>
        public override bool canPass()
        {
            return open;
        }

        public override Special update()
        {
            int[,] s = stair.getMap();
            if (s[x, y - 1] == 2 && s[x, y + 1] != 2 && (s[x + 1, y] != 2 || s[x - 1, y] != 2))
                return null;
            else if (s[x, y - 1] != 2 && s[x, y + 1] == 2 && (s[x + 1, y] != 2 || s[x - 1, y] != 2))
                return null;
            else if (s[x + 1, y] == 2 && s[x - 1, y] != 2 && (s[x, y + 1] != 2 || s[x, y - 1] != 2))
                return null;
            else if (s[x - 1, y] == 2 && s[x + 1, y] != 2 && (s[x, y + 1] != 2 || s[x, y - 1] != 2))
                return null;
            return this;
        }

        //<summary>
        //permet au joueur d'interagir avec la porte (ouverture/fermeture)
        //</summary>
        public override void interact(Entity par1entity)
        {
            if (par1entity is Player)
            {
                Player par1 = ((Player)par1entity);
                if (open == false)
                {
                    if (level == levelMax)
                        par1.showMsg("Vous essayez d'ouvrir cette porte manifestement vérouillée...");
                    level -= (float)par1.skills.getCheat() * rand.Next(1, 10) / 10;
                    if (level * 100 / levelMax <= 20 && already == false)
                        par1.showMsg("Vous sentez la resistance de la porte faiblir.");
                    if (level <= 0)
                    {
                        open = true;
                        value = ".";
                        par1.showMsg("Vous avez reussi à ouvrir la porte !");
                        if (already == false)
                            par1.skills.upgradeCheat(2F);
                        already = true;
                    }

                }
                else
                {
                    open = false;
                    level = 0.00001F;
                    value = originalValue;
                }
            }
        }
    }
}
