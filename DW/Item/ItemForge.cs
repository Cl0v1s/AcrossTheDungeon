using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DW
{
    [Serializable]
    class ItemForge : Item
    {
        public ItemForge()
            : base("Forge", "Une forge faite de pierre et chauffe a la lave\nqui vous permettra de vous créer\n de nombreux outils.", 20, "Installer")
        {

        }

        public override Item interact(Entity par1)
        {
            int[,] s = par1.getStair().getMap();
            Special[,] u = par1.getStair().getSpecial();
            int x = par1.getX();
            int y = par1.getY();
            if(Entity.canWalkOn(x,y+1,par1.getStair()) && par1.getFace()=="front")
                par1.getStair().setSpecial(new Forge(),x,y+1);
            else if (Entity.canWalkOn(x, y - 1, par1.getStair()) && par1.getFace() == "back")
                par1.getStair().setSpecial(new Forge(), x, y - 1);
            else if (Entity.canWalkOn(x + 1, y, par1.getStair()) && par1.getFace() == "right")
                par1.getStair().setSpecial(new Forge(), x + 1, y);
            else if (Entity.canWalkOn(x - 1, y, par1.getStair()) && par1.getFace() == "left")
                par1.getStair().setSpecial(new Forge(), x - 1, y);
            else
            {
                par1.showMsg("Vous ne pouvez pas installer votre forge ici.");
                return this;
            }
            return null;
        }
    }
}
