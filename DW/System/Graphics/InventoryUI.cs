using System;
using System.Drawing;
using System.Threading;

using SdlDotNet.Graphics;

namespace DW
{
    class InventoryUI
    {

        private Inventory inventory;
        private Surface background = new Surface("Data/images/GUI/InventoryUI_background.png");
        private bool opened = false;
        private bool active = false;

        public InventoryUI(Inventory par1)
        {
            inventory = par1;
        }

        public void update()
        {
            if (opened)
            {
                Video.Screen.Blit(background, new Point(70, 90));
                new Text("pixel.ttf", 30, 90, 100, "Inventaire",200,200,200).update();
            }
        }

        public void open()
        {
            if (opened == false)
                opened = true;
            else
                opened = false;
            Thread.Sleep(100);
        }
    }
}
