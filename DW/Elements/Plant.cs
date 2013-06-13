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
                Console.WriteLine("grow");
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

        //<summary>
        //regle l'age de la plante 
        //</summary>
        public void setAge(int par1)
        {
            age = par1;
        }

        //<summary>
        //clone la plante en changeant son etat d'evolution afin de ne pas obtenir de plantes homogènes
        //</summary>
        public override Special clone()
        {
            Plant s = (Plant)this.MemberwiseClone();
            s.setAge(rand.Next(0, 3));
            return (Special)s;
        }

        public override bool canPass()
        {
            if (age < 2)
                return true;
            return false;
        }

    }
}
