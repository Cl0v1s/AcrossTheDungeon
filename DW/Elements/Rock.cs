using System;

namespace DW
{
    class Rock : Special
    {
        private Random rand = new Random();

        private ItemMineral[] possibleContent = new ItemMineral[]
        {
            new ItemMineralIron(),
            new ItemMineralObsidian(),
        };

        private ItemMineral content;


        public Rock()
        {
            value = "rock";
            selectContent();
        }

        public void selectContent()
        {
            content=(ItemMineral)possibleContent[rand.Next(0,possibleContent.Length)].clone();
        }

        public override void interact(Entity par1)
        {
            if (par1.getInventory().contains(new ItemPickAxe()))
            {
                stair.setSpecial(new ItemOnMap(content, stair, x, y), x, y);
                par1.showMsg("Vous enfoncez d'un coups bref votre pioche dans le rocher, qui tombe en poussière.");
                par1.showMsg("Seul un fragment de " + content.getName() + " subsiste sur le sol.");
                if (rand.Next(0, 5) == 0)
                    stair.spawnItem(new ItemDust(), x, y);
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
