using System;

using System.Drawing;

namespace DW
{
    [Serializable]
    class Plant : Special
    {
        public Plant()
            : base()
        {
            value = "T";
            color = Color.DarkGreen;
        }

    }
}
