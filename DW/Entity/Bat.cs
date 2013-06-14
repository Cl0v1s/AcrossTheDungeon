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
            life = 50;
            lifeTmp = life;
            force = 2;
            endurance = 2;
            speed = 20;
            volonte = 0;
            agilite = 7;
            regime = "carnivore";
            range = 3;
            value = "V";
            peur = -10;
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
