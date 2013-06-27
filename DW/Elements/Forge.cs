using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DW
{
    [Serializable]
    class Forge : Special
    {
        public Forge()
        {
            value = "Forge";
        }

        public override void interact(Entity par1)
        {
            DW.render.setRecipe(this);
            DW.render.openRecipe();
        }
    }
}
