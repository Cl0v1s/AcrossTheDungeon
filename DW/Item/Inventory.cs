using System;

namespace DW
{
    [Serializable]
    class Inventory
    {
        private Entity owner;
        private int size;
        private Item[] contents;

        public Inventory(Entity par1owner, int par2size=22)
        {
            owner = par1owner;
            size = par2size;
            contents = new Item[par2size];
        }

        public void setSlot(int par1index, Item par2item)
        {
            contents[par1index] = par2item;
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
                {
                    if(owner.getStair().spawnItem(contents[par1], owner.getX(), owner.getY()))
                        contents[par1] = null;
                    else
                        owner.showMsg("Vousne pouvez deposer un objet ici.");
                }
            }
        }

        public Entity getOwner()
        {
            return owner;
        }

        public Item[] getContents()
        {
            return contents;
        }

        public int getSize()
        {
            return size;
        }


    }
}
