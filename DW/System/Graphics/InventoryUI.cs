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
        private Surface slot = new Surface("Data/images/GUI/Slot.png");
        private Surface selector = new Surface(new Size(32, 32));
        private Surface Icon = new Surface(new Size(30, 30));
        private bool opened = false;
        private int index;
        private int xIndex;
        private int yIndex;
        private bool inSelection = false;
        private int SelectionIndex = 0;
        private Item[] contents;

        public InventoryUI(Inventory par1)
        {
            inventory = par1;
            index=0;
            contents = inventory.getContents();
            Icon = Icon.Convert(Video.Screen);
            selector.Fill(Color.FromArgb(0, 255, 0));
        }

        public void update()
        {
            if (opened)
            {
                Video.Screen.Blit(background, new Point(70, 90));
                new Text("pixel.ttf", 30, 90, 100, "Inventaire",200,200,200).update();
                updateSelector();
                int y = 0;
                int x = 0;
                for (int i = 0; i < inventory.getSize(); i++)
                {
                    if (x == 11)
                    {
                        y += 1;
                        x = 0;
                    }
                    Video.Screen.Blit(slot, new Point(105 + (40 * x), 145 + 40 * y));
                    if (contents[i] != null)
                        Video.Screen.Blit(DW.render.getSprite(contents[i].getName()), new Point(107 + (40 * x), 147 + 40 * y));
                    x += 1;
                }
                if (inSelection)
                {
                    if (contents[index] != null)
                    {
                        Video.Screen.Blit(Icon, new Point(90, 240));
                        string d = contents[index].getDescription();
                        int part = d.Length / 55;
                        if (part <= 1)
                            new Text("pixel.ttf", 20, 90 + 60, 262, contents[index].getDescription()).update();
                        else if (part > 1)
                        {
                            int yu = 262 - (part * 25 / 2) + 25 / 2;
                            for (int i = 0; i < part; i++)
                            {
                                new Text("pixel.ttf", 20, 90 + 60, yu + i * 25, d.Substring(i * 55, 55)).update();
                            }
                        }
                        if (contents[index].getAction() == null)
                            new Text("pixel.ttf", 20, 100, 350, "Utiliser", 150, 150, 150).update();
                        else
                            new Text("pixel.ttf", 20, 100, 350, contents[index].getAction()).update();
                        new Text("pixel.ttf", 20, 490, 350, "Lacher").update();
                        new Text("pixel.ttf", 20, 90 + (SelectionIndex * 390), 350, ">").update();
                    }
                    else
                        inSelection = false;
                }
            }
        }

        private void updateSelector()
        {
            if (inSelection == false)
            {
                if (DW.input.equals(SdlDotNet.Input.Key.Escape))
                    opened=false;
                else if (DW.input.equals(SdlDotNet.Input.Key.RightArrow) && index < inventory.getSize() - 1)
                {
                    index += 1;
                    xIndex += 1;
                    if (xIndex == 11)
                    {
                        yIndex += 1;
                        xIndex = 0;
                    }
                    Thread.Sleep(50);
                }
                else if (DW.input.equals(SdlDotNet.Input.Key.LeftArrow) && index > 0)
                {
                    index -= 1;
                    xIndex -= 1;
                    if (xIndex == -1)
                    {
                        yIndex -= 1;
                        xIndex = 10;
                    }
                    Thread.Sleep(50);
                }
                else if (DW.input.equals(SdlDotNet.Input.Key.KeypadEnter) || DW.input.equals(SdlDotNet.Input.Key.Return))
                {
                    if (contents[index] != null)
                    {
                        Icon = new Surface(new Size(30, 30)).Convert(Video.Screen);
                        Icon.Fill(Color.Fuchsia);
                        Icon.Blit(DW.render.getSprite(contents[index].getName()), new Point(0, 0));
                        Icon = Icon.CreateScaledSurface(2D, false);
                        Icon.SourceColorKey = Color.Fuchsia;
                        string[] i;
                        if (contents[index].getAction() == null)
                            i = new string[] { "Lacher" };
                        else
                            i = new string[] { contents[index].getAction(), "Lacher" };
                        inSelection = true;
                        Thread.Sleep(100);
                    }
                }
            }
            else
            {
                if (DW.input.equals(SdlDotNet.Input.Key.Escape))
                    inSelection = false;
                else if (DW.input.equals(SdlDotNet.Input.Key.LeftArrow))
                    SelectionIndex = 0;
                else if (DW.input.equals(SdlDotNet.Input.Key.RightArrow))
                    SelectionIndex = 1;
                else if (DW.input.equals(SdlDotNet.Input.Key.KeypadEnter) || DW.input.equals(SdlDotNet.Input.Key.Return))
                {
                    if (SelectionIndex == 0)
                        inventory.setSlot(index,contents[index].interact(inventory.getOwner()));
                    else
                    {
                        inventory.removeItem(index, true);
                        contents = inventory.getContents();
                    }
                }
            }

            Video.Screen.Blit(selector, new Point(107 + 40 * xIndex, 147 + 25 * yIndex));
        }

        public void open()
        {
            if (opened == false)
                opened = true;
            else
                opened = false;
            Thread.Sleep(100);
        }

        public bool isOpenned()
        {
            return opened;
        }
    }
}
