using System;

namespace DW
{
    [Serializable]
    class Rock : Special
    {
        private Random rand = new Random();

        private ItemMineral[] possibleContent = new ItemMineral[]
        {
            ItemMineral.ItemMineralIron,
            ItemMineral.ItemMineralObsidian,
            ItemMineral.ItemRock,
        };

        private ItemMineral content;

        private int strenght;


        public Rock()
        {
            value = "rock";
            strenght = rand.Next(2, 20);
            selectContent();
        }

        /**
         * Affecte a content to the rock
         */
        public void selectContent()
        {
            int id = 0;
            int r = rand.Next(0, 100);
            if (r >= 0 && r <= 15)
                id = 1;
            else if (r > 15 && r <= 50)
                id = 0;
            else if (r > 50 && r <= 100)
                id = 2;


            content=(ItemMineral)possibleContent[id].clone();
        }

        /**
         * Allow the player to break the rock if he has a pickaxe in his hand
         * if the strenght of the rock is less (or equals) than 0, then the rock drop a loot
         */
        public override void interact(Entity par1)
        {
            if (par1 is Player && strenght <= 0 && ((Player)par1).getItemInHand()==Item.ItemPickAxe)
            {
                    stair.setSpecial(new ItemOnMap(content, stair, x, y), x, y);
                    par1.showMsg("Vous enfoncez d'un coups bref votre pioche dans le rocher, qui tombe en poussière.");
                    par1.showMsg("Seul un fragment de " + content.getName() + " subsiste sur le sol.");
                    if (rand.Next(0, 5) == 0)
                        stair.spawnItem(DW.Clone<Item>(Item.ItemDust), x, y);
                    int r=rand.Next(0, 7);
                    if (r>=0 && r<=2)
                        stair.spawnItem(DW.Clone<Item>(ItemMineral.ItemRock), x, y);
            }
            else if (par1 is Player &&  ((Player)par1).getItemInHand() == Item.ItemPickAxe)
            {
                    Player p = (Player)par1;
                    if (p.skills.tryAction("forge", 0.5F))
                    {
                        strenght -= 1;
                        par1.showMsg("Vous frappez avec vigueur le rocher, qui commence à s'effriter par endroits.");
                    }
                    else
                        par1.showMsg("Vous frappez maladroitement le rocher, qui ressort indemne de cette agression inutile.");
            }
            else
                par1.showMsg("Et vous comptiez faire ca avec vos ongles ?");
        }

        /*
         * Create a new rock by copying this rock and affect a new content
         */
        public override Special clone()
        {
            Rock r = (Rock)this.MemberwiseClone();
            r.selectContent();
            return (Special)r;
        }




    }
}
