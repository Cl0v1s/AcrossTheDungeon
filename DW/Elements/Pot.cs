using System;

using System.Drawing;

namespace DW
{
    [Serializable]
    class Pot : Special
    {
        private Random rand=new Random();

        private Item[] possibleContent = new Item[]
        {
            new Cookie()
        };

        private Inventory inventory;



        public Pot()
            : base()
        {
            value = "U";
            color = Color.Brown;
            setContent();
        }

        public void setContent()
        {
            inventory = new Inventory(this, 6);
            for (int i = 0; i < 6; i++)
            {
                if(rand.Next(0,10)==10)
                    inventory.addItem(possibleContent[rand.Next(0, possibleContent.Length)].clone());
            }
        }

        public override void interact(Entity par1)
        {
            stair = par1.getStair();
            for (int i = 0; i < inventory.getContents().Length; i++)
            {
                if (inventory.getContents()[i] != null)
                {
                    inventory.removeItem(i, true,par1);
                }
            }
            par1.showMsg("Vous remuez le vase et faites sortir quelques objets");
            if (inventory.isEmtpy()==false)
                par1.showMsg("Il semble qu'il reste quelques objets à l'intérieur...");
            else
                par1.showMsg("Il ne reste plus rien dans le vase.");
        }

        public override Special clone()
        {
            Pot clone=(Pot)this.MemberwiseClone();
            clone.setContent();
            return (Special)clone;
        }
    }
}
