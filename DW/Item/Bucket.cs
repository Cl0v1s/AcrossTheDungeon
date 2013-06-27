using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DW
{
    class Bucket : Item
    {
        public Bucket()
            : base("Seau", "Un seau en fer robuste et léger.", 10, "Remplir")
        {
        }

        public override Item interact(Entity par1)
        {
            if (par1.isNear(100))
                return Item.ItemBucketWater;
            else if (par1.isNear(101))
                return Item.ItemBucketLava;
            else
                return this;
        }
    }

    class BucketWater : Item
    {
        public BucketWater()
            : base("Seau d'eau", "un seau en fer rempli d'eau", 11, "Vider")
        {
        }

        public override Item interact(Entity par1)
        {
            int[,] s = par1.getStair().getMap();
            int x = par1.getX();
            int y = par1.getY();
            if (s[x + 1, y] == 101)
                par1.getStair().set(1, x + 1, y);
            else if (s[x - 1, y] == 101)
                par1.getStair().set(1, x - 1, y);
            else if (s[x, y + 1] == 101)
                par1.getStair().set(1, x, y + 1);
            else if (s[x, y - 1] == 101)
                par1.getStair().set(1, x, y - 1);
            else
            {
                par1.showMsg("Vous vous renversez le seau sur la tête.");
                par1.showMsg("Vous vous sentez plus propre désormais.");
                par1.setSale(par1.getStat()[4] - 15);
                if (par1.getStat()[4] <= 0)
                    par1.setSale(0);
                return Item.ItemBucket;
            }
            par1.showMsg("Vous renversez le seau d'eau sur la lave qui se soldifie avec grand bruit.");
            return Item.ItemBucket;
        }

    }
}
