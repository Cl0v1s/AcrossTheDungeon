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
            addItem(new Berry());
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
                string name = contents[par1].getName();
                if (par2let == true)
                {
                    if (DW.client == null)
                    {
                        if (owner.getStair().spawnItem(contents[par1], owner.getX(), owner.getY()))
                            contents[par1] = null;
                        else
                        {
                            owner.showMsg("Vous ne pouvez deposer un objet ici.");
                            return;
                        }
                    }
                    else
                    {
                        Packet.Send(new CommandPacket("spawnitem", new object[] { contents[par1], owner.getX(), owner.getY() }), DW.client.getServer());
                        while (true)
                        {
                            Packet e=Packet.Receive(DW.client.getServer());
                            if (e is DataPacket && ((DataPacket)e).get() is bool)
                            {
                                bool result = (bool)((DataPacket)e).get();
                                if (result)
                                    contents[par1] = null;
                                else
                                {
                                    owner.showMsg("Vous ne pouvez deposer un objet ici.");
                                    return;
                                }
                                break;
                            }
                        }
                    }
                }
                else
                    contents[par1] = null;
                if (owner is Player)
                    ((Player)owner).showMsg("L'objet " + name + " a été retiré du sac.");
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
