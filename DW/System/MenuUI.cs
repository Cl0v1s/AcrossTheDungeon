using System;
using System.Drawing;

using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Input;

namespace DW
{
    //<summary>
    //affichage et gestion d'une interface de choix
    //</summary>
    class MenuUI
    {
        private int index;
        private Text[] text = new Text[5];
        private int x;
        private int y;
        private int width;
        private int size;
        private int height;
        private int length=-1;
        private Text cursor;
        private bool choosed=false;
        private bool active;
        private int frame = 0;
        private int selection = -1;

        //<summary>
        //créer l'interface
        //</summary>
        //<param name="par1">Les différentes options pouvant être séléctionnées</param>
        //<param name="par2x">La position x de l'interface sur la fenetre de jeu</param>
        //<param name="par3y">La position y de l'interface sur la fenetre de jeu</param>
        //<param name="par4width">La largeur de l'interface</param>
        //<param name="par5height">la hauteur de l'interface</param>
        //<param name="par6size">optionnel, taille du texte par défaut: 20</param>
        public MenuUI(String[] par1,int par2x,int par3y,int par4width,int par5height,int par6size=20,bool par7active=true)
        {
          
            index = 0;
            cursor = new Text("pixel.ttf", par6size, x, (int)(y + 20 + par6size * index), ">");
            width = par4width;
            height = par5height;
            size = par6size;
            x = par2x;
            y = par3y;
            active = par7active;
            for (int i = 0; i < par1.Length; i++)
            {
                if (par1[i] != null)
                {
                    text[i] = new Text("pixel.ttf", par6size, x + 20, (int)(y + i * par6size), par1[i]);
                    length += 1;
                }

            }
            Events.KeyboardDown += new EventHandler<KeyboardEventArgs>(this.examine);
        }

        //<summary>
        //déruit l'interface
        //</summary>
        ~MenuUI()
        {

        }

        //<summary>
        //examine la touche enfoncée et agit en conséquence
        //</summary>
        private void examine(object sender, KeyboardEventArgs e)
        {
            if (active == true)
            {
                if (e.Key == Key.DownArrow)
                    index += 1;
                else if (e.Key == Key.UpArrow)
                    index -= 1;
                else if (e.Key == Key.KeypadEnter || e.Key==Key.Return)
                    choosed = true;
                if (index > length)
                    index = 0;
                else if (index < 0)
                    index = length;
            }

        }

        //<summary>
        //affiche l'interface à l'écran et retourne l'action effectuée si il y a lieu
        //</summary>
        public int update()
        {
            Video.Screen.Fill(new Rectangle(x, y, width, height), Color.Black);
            for (int i = 0; i < text.Length; i++)
            {
                if(text[i] != null)
                 text[i].update();
            }
            cursor.setPos(x, (int)(y + size * index));
            if (active == true)
            {
                frame += 1;
                if (frame > 20)
                    cursor.update();
                if (frame > 80)
                    frame = 0;
            }
            else
                cursor.update();
            if (choosed == true)
            {
                choosed = false;
                selection = index;
                return index;
            }
            return -1;
        }

        //<summary>
        //change l'etat d'activation du menu
        //</summary>
        public void activate()
        {
            if (active == true)
                active = false;
            else if (active == false)
                active = true;
        }

        //<summary>
        //change l'etat d'activation du menu
        //</summary>
        public void activate(bool par1)
        {
            active = par1;
        }

        //<summary>
        //change le texte affiché dans le menu
        //</summary>
        //<param name="par1">le texte à afficher</param>
        public void changeText(String[] par1)
        {
            length = -1;
            for (int i = 0; i < par1.Length; i++)
            {
                if (par1[i] != null)
                {
                    text[i] = new Text("pixel.ttf", size, x + 20, (int)(y + i * size), par1[i]);
                    length += 1;
                }

            }
        }

        //<summary>
        //retourne la séléction active
        //</summary>
        public String getCurrentSelectionText()
        {
            if (selection != -1)
                return text[selection].getText();
            else
                return null;
        }
    }
}
