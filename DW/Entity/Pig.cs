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
            value = "p";
            peur = 0;
            originalValue = value;
            color = Color.Pink;
            turn();
        }

        public override bool update(Entity[] par1)
        {
            others = par1;
            frame += 1;
            if (isSleeping == true)
            {
                if (frame <= 20)
                    value = "Z";
                else
                    value = originalValue;
                if (frame >= 40)
                    frame = 0;
            }
            else
            {
                if (frame <= 20)
                    value = "P";
                else
                    value = originalValue;
                if (frame >= 40)
                    frame = 0;
            }
            if (lifeTmp <= 0)
                dead = true;
            return dead;
        }
    }
}
