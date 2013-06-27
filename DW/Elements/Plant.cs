using System;

using System.Drawing;

namespace DW
{
    [Serializable]
    class Plant : Special
    {

        private int age = 0;
        private Random rand = new Random();
        private float grow = 0;
        private int limit;
        private int life = 1;

        //<summary>
        //créer une plante pouvant être recoltée
        //</summary>
        public Plant()
            : base()
        {
            life = rand.Next(1, 5);
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
            grow += (float)rand.NextDouble()*2;
            if(grow>5000+limit)
            {
                age += 1;
                grow = 0;
                limit = rand.Next(0, age * 50);
                life += rand.Next(1, 5);
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
        public override void interact(Entity par1)
        {
            if (age == 3)
            {
                int n=rand.Next(0,2);
                for(int i=0;i<n;i++)
                {
                    stair.spawnItem(ItemFood.Berry,x,y);
                }
                age = 2;
                return;
            }
            if (age >= 2 && par1.getInventory().contains(Item.ItemAxe))
            {
                if (((Player)par1).skills.tryAction("survival", 2F))
                {
                    life -= 1;
                    par1.showMsg("Vous brandissez bravement votre hache au dessus de votre tete ");
                    par1.showMsg("et entaillez profondemment le végétal.");
                }
                else
                {
                    par1.showMsg("Votre hache vous echappe misérablement des mains alors");
                    par1.showMsg("que vous alliez attaquer l'arbre.");
                }
                if (life <= 0)
                {
                    par1.showMsg("Dans un grand fracas, l'arbre fini par tomber au sol.");
                    int n = rand.Next(0, 2);
                    for (int i = 0; i < n; i++)
                    {
                        stair.spawnItem(Item.ItemWood, x, y);
                    }
                    stair.setSpecial(null, x, y);
                }
            }
            else
                par1.showMsg("Oui, c'est un arbre.");
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
    }
}
