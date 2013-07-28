using System;

using System.Drawing;

namespace DW
{
    [Serializable]
    class Pig : Entity
    {
        public Pig()
        {
            inventory = new Inventory(this);
            name = "Cochon";
            regime = "herbivore";
            value = "P";

            life = 50;
            force = 20;
            endurance = 10;
            volonte = 5;
            agilite = 1;
            speed = 50;
            peur = 0;
            range = 5;

            enduranceTmp = endurance;
            lifeTmp = life;
            originalValue = value;
            turn();
        }

        public override bool update(Entity[] par1)
        {
            others = par1;
            if (lifeTmp <= 0)
                dead = true;
            return dead;
        }
    }
}
