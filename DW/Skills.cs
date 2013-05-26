using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DW
{
    [Serializable]
    class Skills
    {
        private Player owner;
        private Random rand = new Random();
        private int wizardry = 1;
        private int survival = 1;
        private int cheating = 1;
        private float wTemp = 0;
        private float sTemp = 0;
        private float cTemp = 0;

        public Skills(Player par1)
        {
            owner = par1;
            if (owner.getClass() == "Barbare")
                survival = 5;
            else if (owner.getClass() == "Pretre")
                wizardry = 5;
            else if (owner.getClass() == "Escroc")
                cheating = 5;
        }

        public bool tryAction(string par1skill,float par2amount)
        {
            bool v = true;
            if (par1skill == "wizardry")
            {
                if (rand.Next(0, wizardry) == 0)
                {
                    wTemp += par2amount * rand.Next(0, 50) / 100;
                    v = false;
                }
                wTemp += par2amount * rand.Next(50, 100) / 100;
            }
            else if (par1skill == "cheating")
            {
                if (rand.Next(0, cheating) == 0)
                {
                    cTemp += par2amount * rand.Next(0, 50) / 100;
                    v = false;
                }
                cTemp+=par2amount*rand.Next(50,100)/100;
            }
            else if (par1skill == "survival")
            {
                if (rand.Next(0, survival) == 0)
                {
                    sTemp += par2amount * rand.Next(0, 50) / 100;
                    v = false;
                }
                sTemp += par2amount * rand.Next(50, 100) / 100;
            }

            if (wTemp >= 5 * wizardry + 45)
            {
                wTemp = 0;
                wizardry += 1;
                owner.showMsg("Vous semblez mieux maitriser la puissance des arcanes.");
            }

            if (sTemp >= 5 * survival + 60)
            {
                sTemp = 0;
                survival += 1;
                owner.showMsg("Vous êtes désormais plus expérimenté dans le domaine de la survie.");
            }

            if (cTemp >= 5 * cheating + 45)
            {
                cTemp = 0;
                cheating += 1;
                owner.showMsg("Vous commencez à devenir un vrai roublard !");
            }

            return v;
        }

        public int getCheat()
        {
            return cheating;
        }

        public void upgradeCheat(float par1)
        {
            cTemp += par1 * rand.Next(50, 100) / 100;
            if (cTemp >= 5 * cheating + 45)
            {
                cTemp = 0;
                cheating += 1;
                owner.showMsg("Vous commencez à devenir un vrai roublard !");
            }
        }



    }
}
