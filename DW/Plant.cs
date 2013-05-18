using System;

using System.Drawing;

namespace DW
{
    [Serializable]
    class Plant : Special
    {

        private int age = 0;
        private Random rand = new Random();
        private int grow = 0;
        private int limit;

        //<summary>
        //créer une plante pouvant être recoltée
        //</summary>
        public Plant()
            : base()
        {
            age = rand.Next(0, 3);
            value = ",";
            color = Color.DarkGreen;
            limit = rand.Next(0, age * 50);
        }

        //<summary>
        //gère l'evolution de la plante 
        //</summary>
        public override void update()
        {
            if(grow>5000+limit)
            {
                age += 1;
                grow = 0;
                limit = rand.Next(0, age * 50);
            }
            if (age == 0)
                value = ",";
            else if (age == 1)
                value = "\"";
            else if (age == 2)
                value = "!";
            else
                value = "T";
        }

        //<summary>
        //permet au joueur d'interragir avec les plantes et notamment de les recolter.
        //</summary>
        public override void interact(Player par1)
        {
            if (age > 2)
            {

            }
        }

    }
}
