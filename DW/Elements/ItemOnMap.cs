using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DW
{
    [Serializable]
    class ItemOnMap : Special
    {

        private Item item;

        public ItemOnMap(Item par1item, Stair par2stair, int par2x, int par3y)
            : base(par2stair)
        {
            item = par1item;
            value = par1item.getName();
            x = par2x;
            y = par3y;
        }


        public override bool canPass()
        {
            return true;
        }

        public override void interact(Entity par1)
        {
            if (par1.getInventory().addItem(item))
            {
                par1.getStair().removeSpecial(x, y);
            }
        }

    }
}
