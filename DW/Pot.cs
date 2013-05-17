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
    }
}
