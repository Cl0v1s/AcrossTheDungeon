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
        };

        private ItemMineral content;

        private int strenght;


        public Rock()
        {
            value = "rock";
            strenght = rand.Next(2, 20);
            selectContent();
        }

        public void selectContent()
        {
            content=(ItemMineral)possibleContent[rand.Next(0,possibleContent.Length)].clone();
        }

        public override void interact(Entity par1)
        {
            if (strenght <= 0 && par1.getInventory().contains(Item.ItemPickAxe))
            {
                    stair.setSpecial(new ItemOnMap(content, stair, x, y), x, y);
                    par1.showMsg("Vous enfoncez d'un coups bref votre pioche dans le rocher, qui tombe en poussière.");
                    par1.showMsg("Seul un fragment de " + content.getName() + " subsiste sur le sol.");
                    if (rand.Next(0, 5) == 0)
                        stair.spawnItem(DW.Clone<Item>(Item.ItemDust), x, y);
                int r=rand.Next(0, 10);
                    if (r>=0 && r<=2)
                        stair.spawnItem(DW.Clone<Item>(Item.ItemRock), x, y);
            }
            else if (par1.getInventory().contains(Item.ItemPickAxe))
            {
                if (par1 is Player)
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
            }
            else
                par1.showMsg("Et vous comptiez faire ca avec vos ongles ?");
        }

        public override Special clone()
        {
            Rock r = (Rock)this.MemberwiseClone();
            r.selectContent();
            return (Special)r;
        }




    }
}
