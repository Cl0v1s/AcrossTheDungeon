using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Input;

namespace DW
{
    //<summary>
    //affiche et gère une entrée de texte
    //</summary>
    class TextInput
    {
        private Text renderText;
        private string text;
        private string oldText;
        private int x;
        private int y;
        private int height;
        private int width;
        private int maxText;
        private bool active;
        private bool number;

        //<summary>
        //constructeur de l'entrée de texte
        //</summary>
        //<param name="par1x">position x de l'entrée</param>
        //<param name="par2y">position y de l'entrée</param>
        //<param name="par3width">largeur de l'entrée</param>
        //<param name="par4height">hauteur de l'entrée</param>
        //<param name="par6maxText">longueur maximimum de la chaine de caractères</param>
        //<param name="par5text">texte par défaut</param>
        //<param name="par7active">true si l'entrée de texte est active(chaque touche préssée entrainera l'édition du texte de l'entrée)</param> 
        public TextInput(int par1x, int par2y, int par3width, int par4height,int par6maxText=100,string par5text="",bool par7active=true,bool par8number=false)
        {
            x = par1x;
            y = par2y;
            width = par3width;
            height = par4height;
            text = par5text;
            oldText = text;
            maxText = par6maxText;
            active = par7active;
            number = par8number;
            renderText = new Text("pixel.ttf", 20, x + 2, y + 2, text,0,0,0);
            Events.KeyboardDown += new EventHandler<KeyboardEventArgs>(this.examine);
        }

        //<summary>
        //examine la touche pressée et agit en conséquence
        //</summary>
        public void examine(object sender, KeyboardEventArgs e)
        {
            if (text.Length < maxText && active == true || (string)e.KeyboardCharacter == "backspace")
            {
                if (number == false)
                {
                    if ((e.Scancode >= 16 && e.Scancode <= 25) || (e.Scancode >= 30 && e.Scancode <= 39) || (e.Scancode >= 44 && e.Scancode <= 49))
                    {
                        if ((string)e.KeyboardCharacter == "q")
                        {
                            this.text = this.text + "a";
                            return;
                        }
                        else if ((string)e.KeyboardCharacter == "w")
                        {
                            this.text = this.text + "z";
                            return;
                        }
                        else if ((string)e.KeyboardCharacter == ";")
                        {
                            this.text = this.text + "m";
                            return;
                        }
                        else if ((string)e.KeyboardCharacter == "a")
                        {
                            this.text = this.text + "q";
                            return;
                        }
                        else if ((string)e.KeyboardCharacter == "z")
                        {
                            this.text = this.text + "w";
                            return;
                        }
                        else if ((string)e.KeyboardCharacter == "m")
                            return;

                        this.text = this.text + (string)e.KeyboardCharacter;
                    }
                    else if ((string)e.KeyboardCharacter == "space")
                    {
                        this.text = this.text + " ";
                    }
                }
                else
                {
                    if (e.Scancode >= 2 && e.Scancode <= 10)
                    {
                        this.text = this.text + (e.Scancode - 1);
                    }
                    else if (e.Scancode == 51)
                        this.text = this.text + ".";

                }
                
                
                if ((string)e.KeyboardCharacter == "backspace")
                {
                    if (text.Length >= 1)
                        this.text = this.text.Remove(this.text.Length - 1);
                }
            }
        }

        //<summary>
        //affiche l'entrée de texte à l'écran et gère la mise à jour
        //</summary>
        public void update()
        {
            Color color;
            if (active == true)
                color = Color.WhiteSmoke;
            else
                color = Color.DimGray;
            Video.Screen.Fill(new Rectangle(x, y, width, height),color);
            if (text != oldText)
            {
                renderText.changeText(text,0,0,0);
                oldText = text;
            }
            renderText.update();
        }

        //<summary>
        //change l'état d'activation de l'entrée
        //</summary>
        public void activate()
        {
            if (active == false)
                active = true;
            else if (active == true)
                active = false;
        }

        //<summary>
        //règle l'état d'activation de l'entrée en fonction du paramètre passé
        //</summary>
        //<param name="par1">true pour activé, false pour désactivé</param>
        public void activate(bool par1)
        {
            active = par1;
        }

        //<summary>
        //récupère le contenu de l'entrée de texte
        //</summary>
        public String getText()
        {
            return text;
        }


        //<summary>
        //efface tout le texte contenu dans l'entrée de texte
        //</summary>
        public void reset()
        {
            text = "";
            update();
        }
    }
}
