using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DW
{
    [Serializable]
    class Berry : Item
    {
        public Berry()
        {
            set("baie", "Une petite baie acide cueillie sur une plante du donjon.", 2);
        }
    }
}
