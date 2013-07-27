using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DW
{
    [Serializable]
    public class Skills
    {
        private Player owner;
        private Random rand = new Random();
        private int wizardry = 1;
        private int survival = 1;
        private int cheating = 1;
        private int forge = 1;
        private float wTemp = 0;
        private float sTemp = 0;
        private float cTemp = 0;
        private float fTemp = 0;

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
                double r = rand.NextDouble();
                r = r * wizardry;
                if (r >= 0 && r <= wizardry * 0.25)
                {
                    wTemp += par2amount * rand.Next(0, 30) / 100;
                    v = false;
                }
                wTemp += par2amount * rand.Next(50, 100) / 100;
                if (wTemp >= 5 * wizardry + 30)
                {
                    wTemp = 0;
                    wizardry += 1;
                    owner.showMsg("Votre lien avec le monde des arcanes est visiblement plus fort désormais.");
                }
            }
            else if (par1skill == "cheating")
            {
                double r = rand.NextDouble();
                r = r * cheating;
                if (r >= 0 && r <= cheating * 0.25)
                {
                    cTemp += par2amount * rand.Next(0, 30) / 100;
                    v = false;
                }
                cTemp += par2amount * rand.Next(50, 100) / 100;
                if (cTemp >= 5 * cheating + 60)
                {
                    cTemp = 0;
                    cheating += 1;
                    owner.showMsg("Vous commencez à devenir un véritable expert serrurier !");
                }
            }
            else if (par1skill == "survival")
            {
                double r = rand.NextDouble();
                r = r * survival;
                if (r >= 0 && r <= survival * 0.25)
                {
                    sTemp += par2amount * rand.Next(0, 30) / 100;
                    v = false;
                }
                sTemp += par2amount * rand.Next(50, 100) / 100;
                if (sTemp >= 5 * survival + 30)
                {
                    sTemp = 0;
                    survival += 1;
                    owner.showMsg("Vous commencez à vous sentir vraiment aventurier.");
                }
            }
            else if (par1skill == "forge")
            {
                double r=rand.NextDouble();
                r = r * forge;
                if (r >=0 && r<=forge*0.25)
                {
                    fTemp += par2amount * rand.Next(0, 30) / 100;
                    v = false;
                }
                fTemp += par2amount * rand.Next(50, 100) / 100;
                if (fTemp >= 5 * forge + 30)
                {
                    fTemp = 0;
                    forge += 1;
                    owner.showMsg("Grace à l'expérience que vous avez acquise vous vous entez plus a l'aise avec les mineraux.");
                }
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
