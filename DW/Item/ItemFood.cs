using System;

namespace DW
{
   /* [Serializable]
    class Cookie : ItemFood
    {
        public Cookie()
        {
            set("cookie", "Un cookie certainement abandonné par un humain...\nLe pauvre. Gardez le pour un enfant, \nils sont si gourmands.", 3, 10, -10);
        }
    }*/


    [Serializable]
    class ItemFood : Item
    {

        public static ItemFood Berry = new ItemFood("baie", "Une petite baie acide cueillie sur une plante du donjon.", 2,5,1);

        private int eatAmount;
        private int dryAmount;

        public ItemFood(string par1name, string par2desc, int par3price, int par4eat, int par5dry)
        {
            base.set(par1name, par2desc, par3price, "Manger");
            eatAmount = par4eat;
            dryAmount = par5dry;
        }

        protected void set(string par1name, string par2desc, int par3price, int par4eat,int par5dry)
        {
            base.set(par1name, par2desc, par3price,"Manger");
            eatAmount = par4eat;
            dryAmount = par5dry;
        }

        public override Item interact(Entity par1)
        {
            Console.WriteLine("t");
            float e = par1.getHungry();
            par1.setHungry(e + eatAmount);
            e = par1.getThrirst();
            par1.setThrirst(e + dryAmount);
            return null;
        }

    }
}
