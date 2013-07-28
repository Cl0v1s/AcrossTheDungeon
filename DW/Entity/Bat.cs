using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DW
{
    [Serializable]
    class Bat : Entity
    {
        public Bat()
        {
            inventory = new Inventory(this);
            name = "Chauve-Souris";
            regime = "carnivore";
            value = "V";

            life = 40;
            force = 13;
            endurance = 20;
            volonte = 0;
            agilite = 7;
            speed = 20;
            peur = -10;
            range = 3;

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
