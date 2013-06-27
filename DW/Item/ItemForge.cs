using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DW
{
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
            if(s[x,y+1]==1 && u[x,y+1]==null)
                par1.getStair().setSpecial(new Forge(),x,y+1);
            else if (s[x, y - 1] == 1 && u[x, y - 1] == null)
                par1.getStair().setSpecial(new Forge(), x, y - 1);
            else if (s[x + 1, y] == 1 && u[x + 1, y] == null)
                par1.getStair().setSpecial(new Forge(), x + 1, y);
            else if (s[x - 1,y] == 1 && u[x - 1, y] == null)
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
