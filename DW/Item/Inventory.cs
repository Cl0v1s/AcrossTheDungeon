using System;

namespace DW
{
    [Serializable]
    class Inventory
    {
        private Special owner;
        private int size;
        private Item[] contents;

        public Inventory(Special par1owner, int par2size=22)
        {
            owner = par1owner;
            size = par2size;
            contents = new Item[par2size];
            addItem(Item.ItemBucket);
            addItem(ItemFood.ItemPickAxe);
        }

        //<summary>
        //teste si 'linventaire est vide ou non
        //</summary>
        public bool isEmtpy()
        {
            for (int i = 0; i < contents.Length; i++)
            {
                if (contents[i] != null)
                    return false;
            }
            return true;
        }

        //<summary>
        //remplit un emplacement de l'inventaire avec l'objet specifié à l'index spécifié
        //</summary>
        //<param name="par1index">index du slot</param>
        //<param name="par2item">item à ajouter dans le slot spécifié</param>
        public void setSlot(int par1index, Item par2item)
        {
            contents[par1index] = par2item;
        }

        //<summary>
        //ajoute l'item specifié à l'inventaire
        //</summary>
        //<param name="par1">Item à ajouter</param>
        public bool addItem(Item par1,bool par2talk=true)
        {
            if (par1 != null)
            {
                for (int i = 0; i < size; i++)
                {
                    if (contents[i] == null)
                    {
                        contents[i] = par1;
                        if (owner is Entity)
                            contents[i].onAddingInInventory((Entity)owner);
                        if (owner is Player && par2talk)
                            ((Player)owner).showMsg("Vous placez l'objet " + par1.getName() + " dans votre sac.");
                        return true;
                    }
                }
                if (owner is Player && par2talk)
                    ((Player)owner).showMsg("Votre sac est trop lourd, vous ne pouvez emporter plus d'objets");
                return false;
            }
            return false;
        }

        //<summary>
        //supprime un objet de l'inventaire en le posant au sol si specifié et optionnellement à l'emplacement de l'entité specifiée.
        //</summary>
        //<param name="par1">Item à supprimer</param>
        //<param name="par2let">bool indiquant si l'objet doit etre placé au sol ou non</param>
        //<param name="par3target">entité sur laquelle l'objet doit spawner</param>
        public void removeItem(Item par1,bool par2let,Entity par3target=null,bool par4talk=true)
        {
            for (int i = 0; i < size; i++)
            {
                if (contents[i] != null)
                {
                    if (contents[i].getName() == par1.getName())
                    {
                        removeItem(i, par2let, par3target, par4talk);
                        return;
                    }
                }
            }
        }

        //<summary>
        //supprime un objet de l'inventaire en le posant au sol si specifié et optionnellement à l'emplacement de l'entité specifiée.
        //</summary>
        //<param name="par1">index de l'Item à supprimer</param>
        //<param name="par2let">bool indiquant si l'objet doit etre placé au sol ou non</param>
        //<param name="par3target">entité sur laquelle l'objet doit spawner</param>
        public void removeItem(int par1, bool par2let,Entity par3target=null,bool par4talk=true)
        {
            if (contents[par1] != null)
            {
                string name = contents[par1].getName();
                if (par2let == true)
                {
                    if (DW.client == null)
                    {
                        if (par3target == null)
                        {
                            if (owner.getStair().spawnItem(contents[par1], owner.getX(), owner.getY()))
                                contents[par1] = null;
                            else
                            {
                                if (owner is Entity && par4talk)
                                    ((Entity)owner).showMsg("Vous ne pouvez deposer un objet ici.");
                                return;
                            }
                        }
                        else
                        {
                            if (owner.getStair().spawnItem(contents[par1], par3target.getX(), par3target.getY()))
                                contents[par1] = null;
                            else
                            {
                                if (owner is Entity && par4talk)
                                    ((Entity)owner).showMsg("Vous ne pouvez deposer un objet ici.");
                                return;
                            }
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
                            if(owner is Entity)
                                ((Entity)owner).showMsg("Vous ne pouvez deposer un objet ici.");
                                    return;
                                }
                                break;
                            }
                        }
                    }
                }
                else
                    contents[par1] = null;
                if (owner is Player && par4talk==true)
                    ((Player)owner).showMsg("L'objet " + name + " a été retiré du sac.");
            }


        }

        //<summary>
        //retourne le propriétaire de l'inventaire
        //</summary>
        public Special getOwner()
        {
            return owner;
        }

        //<summary>
        //retourne le contenu de l'inventaire
        //</summary>
        public Item[] getContents()
        {
            return contents;
        }

        //<summary>
        //retourne la taille/le nombre de slot de l'inventaire
        //</summary>
        public int getSize()
        {
            return size;
        }

        //<summary>
        //teste si l'inevtnaire contient l'objet spécifié
        //</summary>
        public bool contains(Item par1)
        {
            for (int i = 0; i < contents.Length; i++)
            {
                if (contents[i] != null)
                {
                    if (contents[i].getName() == par1.getName())
                        return true;
                }
            }
            return false;
        }

        public int howMany(Item par1)
        {
            return howMany(par1.getName());
        }

        public int howMany(string par1)
        {
            int n = 0;
            for (int i = 0; i < contents.Length; i++)
            {
                if (contents[i] != null && contents[i].getName() == par1)
                    n += 1;
            }
            return n;
        }


    }
}
