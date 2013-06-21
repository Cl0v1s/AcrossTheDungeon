using System;



namespace DW
{























    [Serializable]
    class Item
    {
        private string name;
        private string description;
        private string action;
        private int price;

        //<summary>
        //créer un nouvel item
        //</summary>
        //<param name="par1name">nom de l'objet</param>
        //<param name="par2desc">descritpion de l'objet</param>
        //<param name="par3price">prix de vente de l'objet</param>
        public Item(string par1name, string par2desc, int par3price,string par4action=null)
        {
            name = par1name;
            description = par2desc;
            price = par3price;
            action = par4action;

        }

        public Item()
        {

        }

        protected virtual void set(string par1name, string par2desc, int par3price,string par4action=null)
        {
            name = par1name;
            description = par2desc;
            price = par3price;
            action = par4action;
        }

        //<summary>
        //execute la fonction de l'objet
        //</summary>
        //<param name="par1">entité à affecter</param>
        public virtual Item interact(Entity par1)
        {
            return this;
        }

        public string getName()
        {
            return name;
        }

        public string getDescription()
        {
            return description;
        }

        public string getAction()
        {
            return action;
        }

        public virtual Item clone()
        {
            return (Item)this.MemberwiseClone();
        }

    }
}
