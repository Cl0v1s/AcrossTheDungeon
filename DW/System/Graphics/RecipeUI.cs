using System;
using System.Drawing;
using System.Threading;

using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Sprites;

namespace DW
{
    class RecipeUI
    {
        private Player owner;
        private Special tool;
        private bool opened = false;
        private Surface background = new Surface("Data/images/GUI/InventoryUI_background.png");
        private Recipe[] possibleList = new Recipe[100];
        private int listIndex = 0;
        private int index = 0;
        private bool inSelection=false;
        private bool canCraftSelected = false;


        public RecipeUI(Player par1owner, Special par2tool = null)
        {
            owner = par1owner;
            tool = par2tool;
        }

        public void update()
        {
            if (opened)
            {
                inputUpdate();
                Video.Screen.Blit(background, new Point(70, 90));
                new Text("pixel.ttf", 30, 90, 100, "Assemblage", 200, 200, 200).update();
                for (int i = listIndex; i < listIndex+5; i++)
                {
                    if (possibleList[i] != null)
                    {
                        Sprite e=DW.render.getSprite(possibleList[i].getItemResults()[0].getName());
                        Video.Screen.Blit(e, new Point(110, 140 + i * 30));
                        new Text("pixel.ttf", 25, 140, 140 + i * 30, possibleList[i].getName()).update();
                    }
                }
                new Text("pixel.ttf", 25, 100, 140 + index * 30, ">").update();
                if (inSelection)
                {
                    new Text("pixel.ttf", 27, 300, 100, possibleList[index].getName(),0,255,0).update();
                    string d = possibleList[index].getDescription();
                    int part=d.Split("\n".ToCharArray()).Length;
                    for (int u = 0; u < part; u++)
                    {
                        new Text("pixel.ttf", 20, 300, 130 + u * 20, d.Split("\n".ToCharArray())[u]).update();
                    }
                    new Text("pixel.ttf", 23, 300, 210, "Necessite:", 255, 0, 0).update();
                    d = possibleList[index].getNeeds();
                    part = d.Split(";".ToCharArray()).Length - 1;
                    for (int y = 0; y < part; y++)
                    {
                        new Text("pixel.ttf", 20, 300, 240+y*18, (d.Split(";".ToCharArray())[y]+" > x"+owner.getInventory().howMany(d.Split(";".ToCharArray())[y]))).update();
                    }
                    if(canCraftSelected==false)
                        new Text("pixel.ttf", 25, 450, 350, "  Assembler", 100, 100, 100).update();
                    else
                        new Text("pixel.ttf", 25, 450, 350, "> Assembler", 0, 255, 0).update();


                }
            }
        }

        private bool canCraft(Recipe par1)
        {
            Item[] n = par1.getItemNeeds();
            Item[] r = new Item[n.Length];
            int d = 0;
            for (int i = 0; i < n.Length; i++)
            {
                if (owner.getInventory().contains(n[i]))
                {
                    d += 1;
                    r[i] = n[i];
                    owner.getInventory().removeItem(n[i], false);
                }
            }
            for (int i = 0; i < r.Length; i++)
            {
                if (r[i] != null)
                    owner.getInventory().addItem(r[i],false);
            }
            if (d >= n.Length)
                return true;
            else
                return false;
        }

        private void craft(Recipe par1)
        {
                if(canCraft(par1))
                {
                    Item[] n = par1.getItemNeeds();
                    for (int i = 0; i < n.Length; i++)
                    {
                        owner.getInventory().removeItem(n[i], false);
                    }
                    n=par1.getItemResults();
                    for(int i=0;i<n.Length;i++)
                    {
                        owner.getInventory().addItem(n[i]);
                    }
                    owner.showMsg("Vous avez réalisé la recette " + par1.getName() + " avec vos petites mains !");
                }
                else
                    owner.showMsg("Vous ne disposez pas des ressources necessaires pour assembler cette recette.");

        }

        public void inputUpdate()
        {
            if (inSelection == false)
            {
                if (DW.input.equals(SdlDotNet.Input.Key.DownArrow))
                {
                    if (index < 5 && possibleList[index] != null)
                        index += 1;
                    else if (possibleList[index] != null)
                    {
                        listIndex += 1;
                        index -= 1;
                    }
                    Thread.Sleep(10);
                }
                else if (DW.input.equals(SdlDotNet.Input.Key.UpArrow))
                {
                    if (index > 0)
                        index -= 1;
                    else if (listIndex >= 1)
                    {
                        listIndex -= 1;
                        index += 1;
                    }
                    Thread.Sleep(10);
                }
                else if (DW.input.equals(SdlDotNet.Input.Key.Escape))
                    opened = false;
                else if (DW.input.equals(SdlDotNet.Input.Key.KeypadEnter) || DW.input.equals(SdlDotNet.Input.Key.Return))
                {
                    inSelection = true;
                    canCraftSelected = canCraft(possibleList[index]);
                }
            }
            else
            {
                if (DW.input.equals(SdlDotNet.Input.Key.Escape))
                {
                    inSelection = false;
                    Thread.Sleep(100);
                }
            }

        }

        //<summary>
        //ouvre la fenetre d'assemblage si fermé. Ferme si ouvert.
        //</summary>
        public void open()
        {
            if (opened == false)
            {
                opened = true;
                findPossible();
            }
            else
                opened = false;
            Thread.Sleep(100);
        }

        private void findPossible()
        {
            Recipe[] l=owner.getRecipes();
            for (int i = 0; i < l.Length; i++)
            {
                if (tool != null)
                {
                    if (l[i] != null && l[i].getTool().GetType() == tool.GetType())
                    {
                        possibleList[i] = l[i];
                    }
                }
                else if (l[i] != null)
                {
                        possibleList[i] = l[i];
                }
            }
        }

        public void setTool(Special par1)
        {
            tool = par1;
        }

        //<summary>
        //retourne si la fenètre d'assemblage est ouverte ou fermée.
        //</summary>
        public bool isOpenned()
        {
            return opened;
        }

    }
}
