using System;

using System.Drawing;

namespace DW
{
    [Serializable]
    class Pot : Special
    {
        public Pot()
            : base()
        {
            value = "U";
            color = Color.Brown;
        }

        public override void interact(Entity par1)
        {
            stair.setSpecial(null, x, y);
            drop();
        }

        private void drop()
        {

        }
    }
}
