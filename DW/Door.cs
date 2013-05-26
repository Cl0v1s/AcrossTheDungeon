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


        //<summary>
        //créer et gère une porte
        //</summary>
        public Door()
            : base()
        {
            open = false;
            value = "|";
            levelMax = rand.Next(1, 20);
            level = (float)levelMax;
            color = Color.Chocolate;
        }

        //<summary>
        //retourne l'etat d'ouverture de la porte afin de savoir si un entitée peut passer
        //</summary>
        public override bool canPass()
        {
            return open;
        }

        //<summary>
        //permet au joueur d'interagir avec la porte (ouverture/fermeture)
        //</summary>
        public override void interact(Player par1)
        {
            if (open == false)
            {
                if(level==levelMax)
                    par1.showMsg("Vous essayez d'ouvrir cette porte manifestement vérouillée...");
                level -= (float)par1.skills.getCheat() * rand.Next(1, 10) / 10;
                if (level * 100 / levelMax <= 20 && already==false)
                    par1.showMsg("Vous sentez la resistance de la porte faiblir.");
                if (level <= 0)
                {
                    open = true;
                    value = ".";
                    par1.showMsg("Vous avez reussi à ouvrir la porte !");
                    if(already==false)
                        par1.skills.upgradeCheat(2F);
                    already = true;
                }
                
            }
            else
            {
                open = false;
                level = 0.00001F;
                value = "|";
            }
        }
    }
}
