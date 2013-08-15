using System;
using System.Drawing;

namespace DW
{
    class Pnj : Entity
    {
        House house;

        public static string[] PnjName = new string[]
        {

        };

        public Pnj(House par1)
        {
            house = par1;
            inventory = new Inventory(this);
            name = "Pnj";
            regime = "herbivore";
            value = "Pnj";

            life = 150;
            force = 20;
            endurance = 20;
            volonte = 20;
            agilite = 10;
            speed = 20;
            peur = 0;
            range = 5;

            enduranceTmp = endurance;
            lifeTmp = life;
            originalValue = value;
            turn();
        }

        //<summary>
        //Fait tourner le pnj à la manière des pnjs de pokémon
        //</summary>
        void play()
        {
            int r = rand.Next(0, 6);
            if (r == 1)
                face = "front";
            else if (r == 2)
                face = "back";
            else if (r == 3)
                face = "left";
            else if (r == 4)
                face = "right";
        }

        //<summary>
        //Intelligence artificielle chargée de faire des choix.
        //</summary>
        protected override void choiceIA()
        {
            if(house.contains(new Point(x,y)))
            {
                play();
                return;
            }
            if (worstEnemy != null)
            {
                if (peur > 0 && canSee(worstEnemy.x, worstEnemy.y))
                {
                    escapeFrom(worstEnemy.x, worstEnemy.y);
                    return;
                }
                else if (canSee(worstEnemy.x, worstEnemy.y))
                {
                    moveTo(worstEnemy.x, worstEnemy.y);
                    return;
                }
            }
            if (objective.X != -1 && objective.Y != -1)
            {
                attempt += 1;
                if (attempt >= 10)
                {
                    attempt = 0;
                    objective = new Point(-1, -1);
                    return;
                }
                moveTo(objective.X, objective.Y);
                return;
            }
            else if (bestFriend != null && canSee(bestFriend.x, bestFriend.y) && peur>0)
            {
                moveTo(bestFriend.x, bestFriend.y);
                return;
            }
            Point p = stair.getFreeSpecialCase();
            objective = p;
            moveTo(objective.X, objective.Y);
            return;
        }
    }
}
