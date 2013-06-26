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
            ItemFood.Berry,
            Item.ItemPickAxe,
            Item.ItemAxe,
            Item.ItemDust
        };
        private Inventory inventory;

        public Pot()
            : base()
        {
            value = "U";
            color = Color.Brown;
            setContent();
        }

        //<summary>
        //ajoute des objets provenant de la liste d'objets possible à l'inventaire du pot
        //</summary>
        public void setContent()
        {
            inventory = new Inventory(this, 6);
            for (int i = 0; i < 6; i++)
            {
                if (rand.Next(0, 5) == 0)
                {
                    inventory.addItem(possibleContent[rand.Next(0, possibleContent.Length)].clone());
                }
            }
        }

        //<summary>
        //permet au joueur d'interragir avec le pot et d'en sortir les objets
        //</summary>
        //<param name="par1">l'entité interragissant avec le pot</param>
        public override void interact(Entity par1)
        {
            stair = par1.getStair();
            for (int i = 0; i < inventory.getContents().Length; i++)
            {
                if (inventory.getContents()[i] != null)
                {
                    inventory.removeItem(i, true);
                }
            }
            par1.showMsg("Vous remuez le vase et faites sortir quelques objets");
            if (inventory.isEmtpy()==false)
                par1.showMsg("Il semble qu'il reste quelques objets à l'intérieur...");
            else
                par1.showMsg("Il ne reste plus rien dans le vase.");
        }

        //<summary>
        //permet d'obtenir un nouveau pot à partir de cette instance, avec un contenue différent
        //</summary>
        public override Special clone()
        {
            Pot clone=(Pot)this.MemberwiseClone();
            clone.setContent();
            return (Special)clone;
        }
    }
}
