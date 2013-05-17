using System;

using System.Drawing;

namespace DW
{
    [Serializable]
    class Door : Special
    {
        public Door()
            : base()
        {
            value = "|";
            color = Color.Chocolate;
        }
    }
}
