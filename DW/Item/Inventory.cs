using System;

namespace DW
{
    class Inventory
    {
        private Entity owner;
        private int size;
        private Item[] contents;

        public Inventory(Entity par1owner, int par2size=16)
        {
            owner = par1owner;
            size = par2size;
            contents = new Item[par2size];
        }

        public bool addItem(Item par1)
        {
            for (int i = 0; i < size; i++)
            {
                if (contents[i] == null)
                {
                    contents[i] = par1;
                    if (owner is Player)
                        ((Player)owner).showMsg("Vous placez l'objet " + par1.getName() + " dans votre sac.");
                    return true;
                }
            }
            if (owner is Player)
                ((Player)owner).showMsg("Votre sac est trop lourd, vous ne pouvez emporter plus d'objets");
            return false;
        }

        public void removeItem(Item par1,bool par2let)
        {
            for (int i = 0; i < size; i++)
            {
                if (contents[i].getName() == par1.getName())
                {
                    removeItem(i, par2let);
                    return;
                }
            }
        }

        public void removeItem(int par1, bool par2let)
        {
            if (contents[par1] != null)
            {
                if (owner is Player)
                    ((Player)owner).showMsg("L'objet " + contents[par1].getName() + " a été retiré du sac.");
                if (par2let == true)
                    owner.getStair().spawnItem(contents[par1], owner.getX(), owner.getY());
                contents[par1] = null;
            }
        }

        public Entity getOwner()
        {
            return owner;
        }


    }
}
