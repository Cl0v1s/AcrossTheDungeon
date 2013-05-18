using System;

using System.Drawing;

namespace DW
{
    [Serializable]
    class Door : Special
    {
        private bool open;
        private Random rand=new Random();

        public Door()
            : base()
        {
            if (rand.Next(0, 50) == 0)
                open = true;
            else
                open = false;

            if (open == false)
                value = "|";
            else
                value = ".";
            color = Color.Chocolate;
        }

        public override bool canPass()
        {
            return open;
        }

        public override void interact(Player par1)
        {
            if (open == false)
            {
                par1.showMsg("Vous essayez d'ouvrir cette porte manifestement vérouillée...");
                if (par1.skills.tryAction("cheating", 2))
                {
                    open = true;
                    value = ".";
                    par1.showMsg("Grace à votre talent, vous avez ouvert la porte !");
                }
                else
                    par1.showMsg("Apres de nombreux essais vous n'arrivez pas à ouvrir la porte...");
            }
            else
            {
                open = false;
                value = "|";
            }
        }
    }
}
