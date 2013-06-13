using System;

using System.Drawing;

namespace DW
{
    [Serializable]
    class Pig : Entity
    {
        public Pig()
        {
            name = "Cochon";
            life = 30;
            lifeTmp = life;
            force = 3;
            endurance = 1;
            volonte = 1;
            agilite = 1;
            regime = "herbivore";
            value = "P";
            peur = 0;
            originalValue = value;
            color = Color.Pink;
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
