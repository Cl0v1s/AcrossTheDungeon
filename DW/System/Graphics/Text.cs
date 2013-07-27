using System;
using System.IO;

using SdlDotNet.Core;
using SdlDotNet.Graphics;
using System.Drawing;

namespace DW
{

    public enum TypePos
    {
        Normal,
        Center
    }
    //<summary>affiche du texte à l'écran</summary>
    class Text
    {
        private SdlDotNet.Graphics.Font font;
        private Surface text;
        private int x;
        private int y;
        private String value;
        private TypePos pos;

        //<summary>
        //créer la surface sur laquelle sera affichée le texte
        //</summary>
        //<param name="par1file">fichier de la police</param>
        //<param name="par2size">la taille du texte</param>
        //<param name="par3x">la position x du texte dans la fenetre du jeu</param>
        //<param name="par4y">la position y du texte dans la fenetre du jeu</param>
        //<param name="par5text">le texte à afficher</param>
        //<param name="r">couleur rouge du texte</param>
        //<param name="v">couleur vert du texte</param>
        //<param name="b">couleur bleu du texte</param>
        public Text(String par1file,int par2size,int par3x,int par4y,String par5text,int r=255,int v=255,int b=255,TypePos par6type=TypePos.Normal)
        {
            pos = par6type;
            value = par5text;
            font = new SdlDotNet.Graphics.Font(Directory.GetCurrentDirectory()+"\\Data\\"+par1file, par2size);
            text = font.Render(par5text, Color.FromArgb(r,v,b));
            setPos(par3x, par4y);
        }

        //<summary>
        //change le texte contenu par la surface de texte
        //</summary>
        //<param name="par1">le texte à afficher</param>
        //<param name="r">quantité de rouge</param>
        //<param name="v">quantité de vert</param>
        //<param name="b">quantit" de bleu</param>
        public Surface changeText(String par1,int r=255,int v=255,int b=255)
        {
            text = font.Render(par1, Color.FromArgb(r, v, b));
            value = par1;
            return text;
        }

        //<summary>
        //retourne la position x du texte
        //</summary>
        public int getX()
        {
            return x;
        }

        //<summary>
        //retourne la position y du texte
        //</summary>
        public int getY()
        {
            return y;
        }

        //<summary>
        //retourne la valeur du texte
        //</summary>
        public string getText()
        {
            return value;
        }

        //<summary>
        //change la position initiale du texte
        //</summary>
        //<param name="par1x">position x du texte dans la fenetre du jeu</param>
        //<param name="par2y">position y du texte dans la fenetre du jeu</param>
        public void setPos(int par1x, int par2y)
        {
            if (pos == TypePos.Normal)
            {
                x = par1x;
                y = par2y;
            }
            else
            {
                    x = par1x - text.Width / 2;
                    y = par2y - text.Height / 2;
            }

        }

        //<summary>
        //affiche le texte à l'écran
        //</summary>
        public void update()
        {
            try
            {
                Video.Screen.Blit(text, new Rectangle(new Point(x, y), text.Size));
            }
            catch (Exception)
            { }
        }
    }
}
