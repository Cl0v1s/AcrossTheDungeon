using System;

using System.Drawing;

namespace DW
{
    [Serializable]
    class Chest : Special
    {
        public Chest()
            : base()
        {
            value = "=";
            color = Color.Chocolate;
        }
    }
}
