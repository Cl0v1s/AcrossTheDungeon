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
        private bool recentlyOpenned = false;


        public RecipeUI(Player par1owner, Special par2tool = null)
        {
            owner = par1owner;
            tool = par2tool;
        }

        //<summary>
        //si ouvert, affiche la fenetre d'assemblage
        //</summary>
        public void update()
        {
            if (opened)
            {
                inputUpdate();
                Video.Screen.Blit(background, new Point(70, 90));
                if(tool==null)
                    new Text("pixel.ttf", 30, 90, 100, "Assemblage", 200, 200, 200).update();
                else
                    new Text("pixel.ttf", 30, 90, 100, "Assemblage ("+tool.getName()+")", 200, 200, 200).update();
                for (int i = listIndex; i < listIndex+5; i++)
                {
                    if (possibleList[i] != null)
                    {
                        Sprite e=DW.render.getSprite(possibleList[i].getItemResults()[0].getName());
                        try
                        {
                            Video.Screen.Blit(e, new Point(120, 135 + i * 30));
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Ressource absente pour " + possibleList[i].getItemResults()[0].getName());
                        }
                        new Text("pixel.ttf", 25, 160, 140 + i * 30, possibleList[i].getName()).update();
                    }
                }
                new Text("pixel.ttf", 25, 100, 140 + index * 30, ">").update();
                if (inSelection)
                {
                    if (possibleList[index] != null)
                    {
                        if(canCraftSelected)
                            new Text("pixel.ttf", 27, 540, 100, "V",0,255,0).update();
                        new Text("pixel.ttf", 27, 300, 100, possibleList[index].getName()).update();
                        string d = possibleList[index].getDescription();
                        int part = d.Split("\n".ToCharArray()).Length;
                        for (int u = 0; u < part; u++)
                        {
                            new Text("pixel.ttf", 20, 300, 130 + u * 20, d.Split("\n".ToCharArray())[u]).update();
                        }
                        if(canCraftSelected==false)
                            new Text("pixel.ttf", 23, 300, 210, "Necessite:", 255, 0, 0).update();
                        else
                            new Text("pixel.ttf", 23, 300, 210, "Necessite:", 0, 255, 0).update();
                        d = possibleList[index].getNeeds();
                        part = d.Split(";".ToCharArray()).Length - 1;
                        for (int y = 0; y < part; y++)
                        {
                            Video.Screen.Blit(DW.render.getSprite(d.Split(";".ToCharArray())[y]),new Point(300+25*y,240));
                        }
                        if (canCraftSelected == false)
                            new Text("pixel.ttf", 25, 450, 350, "  Assembler", 100, 100, 100).update();
                        else
                            new Text("pixel.ttf", 25, 450, 350, "> Assembler").update();
                    }


                }
            }
        }

        //<summary>
        //retourne true si la recette spécifiée peux etre realisée.
        //</summary>
        //<param name="par1">la recette a tester</param>
        private bool canCraft(Recipe par1)
        {
            if (par1 != null)
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
                        owner.getInventory().removeItem(n[i], false,null,false);
                    }
                }
                for (int i = 0; i < r.Length; i++)
                {
                    if (r[i] != null)
                        owner.getInventory().addItem(r[i], false);
                }
                if (d >= n.Length)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        //<summary>
        //craft la recette specifiée
        //</summary>
        //<param name="par1">la recette a crafter</param>
        private void craft(Recipe par1)
        {
                if(par1 != null && canCraft(par1))
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

        //<summary>
        //actualise les entrées de touches
        //</summary>
        public void inputUpdate()
        {
            if (inSelection == false)
            {
                if (DW.input.equals(SdlDotNet.Input.Key.DownArrow))
                {
                    if (index < 5 && possibleList[index+1] != null)
                        index += 1;
                    else if (possibleList[index+1] != null)
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
                    Thread.Sleep(100);
                }
            }
            else
            {
                if (DW.input.equals(SdlDotNet.Input.Key.Escape))
                {
                    inSelection = false;
                    Thread.Sleep(100);
                }
                else if (DW.input.equals(SdlDotNet.Input.Key.KeypadEnter) || DW.input.equals(SdlDotNet.Input.Key.Return) && canCraftSelected)
                {
                    craft(possibleList[index]);
                    canCraftSelected=canCraft(possibleList[index]);
                    Thread.Sleep(100);
                }

            }
            if(recentlyOpenned)
            {
                inSelection=false;
                recentlyOpenned = false;
            }

        }

        //<summary>
        //ouvre la fenetre d'assemblage si fermé. Ferme si ouvert.
        //</summary>
        public void open()
        {
            if (opened == false)
            {
                possibleList = new Recipe[100];
                listIndex = 0;
                index = 0;
                inSelection=false;
                canCraftSelected = false;
                opened = true;
                recentlyOpenned = true;
                findPossible();
            }
            else
            {
                opened = false;
            }
            Thread.Sleep(100);
        }

        //<summary>
        //détermine quelles sont les recettes pouvant etres réalisé avec la configuration courante.
        //</summary>
        private void findPossible()
        {
            Recipe[] l=owner.getRecipes();
            int it = 0;
            for (int i = 0; i < l.Length; i++)
            {
                if (tool != null)
                {
                    if (l[i] != null && l[i].getTool() != null && l[i].getTool().GetType() == tool.GetType())
                    {
                        possibleList[it] = l[i];
                        it += 1;
                    }
                }
                else if (l[i] != null && l[i].getTool()==null)
                {
                        possibleList[it] = l[i];
                        it += 1;
                }
            }
        }

        //<summary>
        //paramètre la fenetre pour utiliser l'outil de craft spécifié
        //</summary>
        //<param name="par1">l'outil de craft à utiliser</param>
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
