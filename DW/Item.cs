using System;



namespace DW
{
    class Item
    {
        private string name;
        private string description;
        private int price;

        //<summary>
        //créer un nouvel item
        //</summary>
        //<param name="par1name">nom de l'objet</param>
        //<param name="par2desc">descritpion de l'objet</param>
        //<param name="par3price">prix de vente de l'objet</param>
        public Item(string par1name, string par2desc, int par3price)
        {
            name = par1name;
            description = par2desc;
            price = par3price;
        }

        //<summary>
        //execute la fonction de l'objet
        //</summary>
        //<param name="par1">entité à affecter</param>
        public virtual void interact(Entity par1)
        {

        }

    }
}
