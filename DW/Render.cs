using System;
using System.IO;

using System.Drawing;
using SdlDotNet.Core;
using SdlDotNet.Graphics;

namespace DW
{
    class Render
    {
        /* Valeurs des différents ID contenus par les cases
         * 0-vide
         * 1-sol donjon
         * 2-mur donjon
         * 3-piège donjon
         * 4-herbe
         * 5-gravier
         * 
         * 100-eau
         */
        public object[] value = new object[]{
            "",Color.Firebrick,
            ".",Color.DarkSlateGray,
            "H",Color.DarkGray,
            ",",Color.DarkSlateGray,
            "'",Color.Green,
            "*",Color.LightGray,
            
        };

        public object[] animated = new object[]{
            "_",Color.Blue,
            "-",Color.Blue,
            "_",Color.Red,
            "-",Color.Red,

        };

        private int frame;
        private int x;
        private int y;
       
        private SdlDotNet.Graphics.Font font = new SdlDotNet.Graphics.Font(Directory.GetCurrentDirectory()+"\\Data\\"+"pixel.ttf", 30);

        public Render(int par1x,int par2y)
        {
            x = par1x;
            y = par2y;
        }

        //<summary>
        //met à jour le compteur de frame pour les annimations
        //</summary>
        public void update()
        {
            frame += 1;
            if (frame > 40)
                frame = 0;
        }

        //<summary>
        //affiche la map entière dans la fenetre
        //</summary>
        //<param name="par1map">la map à afficher</param>
        //<param name="par2width">la largeur de la map à afficher</param>
        //<param name="par3height">la hauteur de la map à afficher</param>
        public void renderMap(int[,] par1map, int par2width, int par3height)
        {
            for (int i = 0; i < par2width; i++)
            {
                for (int u = 0; u < par3height; u++)
                {

                    if (par1map[i, u] * 2 <= value.Length && par1map[i, u] * 2 + 1 <= value.Length && par1map[i, u] < 100)
                        Video.Screen.Blit(font.Render((string)value[par1map[i, u] * 2], (Color)value[par1map[i, u] * 2 + 1]), new Point(x + i * 30, y + u * 30));
                    else
                        renderAnimated(par1map, i, u);

                }
            }
        }

        //<summary>
        //affiche la map et les entitésdans le périmètre de vision de l'entité spécifiée
        //</summary>
        //<param name="par1">l'entité</param>
        public void renderEntityVision(Entity par1)
        {
            if (par1.getStair() != null)
            {
                int[,] par1map = par1.getStair().getMap();
                Special[,] specials = par1.getStair().getSpecial();
                for (int i = par1.getX() - par1.getRange(); i < par1.getX() + par1.getRange(); i++)
                {
                    for (int u = par1.getY() - par1.getRange(); u < par1.getY() + par1.getRange(); u++)
                    {
                        if (u >= 0 && i >= 0 && i <= par1.getStair().getW() && u <= par1.getStair().getH() && par1.canSee(i, u))
                        {
                            if (par1map[i, u] * 2 <= value.Length && par1map[i, u] * 2 + 1 <= value.Length && par1map[i, u] < 100)
                                Video.Screen.Blit(font.Render((string)value[par1map[i, u] * 2], (Color)value[par1map[i, u] * 2 + 1]), new Point(x + i * 30, y + u * 30));
                            else
                                renderAnimated(par1map, i, u);
                            if (specials[i, u] != null)
                                renderSpecialAt(specials, i, u);
                        }

                    }
                }
                Entity[] ent = par1.getStair().getEntities();
                for (int i = 0; i < ent.Length; i++)
                {
                    if (ent[i] != null)
                    {
                        int xe = ent[i].getX();
                        int ye = ent[i].getY();
                        if (par1.canSee(xe, ye))
                            renderEntityAt(ent[i]);
                    }
                }
            }
        }

        //<summary>
        //affiche l'entité spécifiée
        //</summary>
        //<param name="par1">l'entité</param>
        private void renderEntityAt(Entity par1)
        {
            Video.Screen.Blit(font.Render(par1.getChar(), par1.getColor()), new Point(x + par1.getX() * 30, y + par1.getY() * 30));
        }

        //<summary>
        //affiche la case contenant un objet spécial situé aux coordonées spécicifiées
        //</summary>
        //<param name="par1map">la liste des objets spéciaux contenant l'objet à afficher</param>
        //<param name="par2x">la position x de l'objet à afficher</param>
        //<param name="par3y">la position y de l'objet à afficher</param>
        private void renderSpecialAt(Special[,] par1map, int par2x, int par3y)
        {
            if (par1map[par2x, par3y] != null)
                Video.Screen.Blit(font.Render((string)par1map[par2x, par3y].getChar(), (Color)par1map[par2x, par3y].getColor()), new Point(x + par2x * 30, y + par3y * 30));
        }

        private void renderAnimated(int[,] par1map, int par2x, int par3y)
        {
            try
            {
                if (frame < 40 / 2)
                    Video.Screen.Blit(font.Render((string)animated[(par1map[par2x, par3y] - 100)*4], (Color)animated[(par1map[par2x, par3y] - 100)*4 + 1]), new Point(x + par2x * 30, y + par3y * 30));
                else
                    Video.Screen.Blit(font.Render((string)animated[(par1map[par2x, par3y] - 100)*4 + 2], (Color)animated[(par1map[par2x, par3y] - 100)*4 + 3]), new Point(x + par2x * 30, y + par3y * 30));
            }
            catch (Exception)
            {

            }
        }

        //<summary>
        //affiche tout les objets spéciaux de la map
        //</summary>
        //<param name="par1map">la liste des objets spéciaux à afficher</param>
        //<param name="par2width">la largeur de la liste des objets spéciaux à afficher</param>
        //<param name="par3height">la hauteur de la liste des objets spéciaux à afficher</param>
        public void renderSpecial(Special[,] par1map, int par2width, int par3height)
        {
            for (int i = 0; i < par2width; i++)
            {
                for (int u = 0; u < par3height; u++)
                {
                    if(par1map[i, u] != null)
                        Video.Screen.Blit(font.Render((string)par1map[i, u].getChar(), (Color)par1map[i, u].getColor()), new Point(x + i * 30, y + u * 30));
                }
            }
        }

        //<summary>
        //affiche tout les entités de la liste spécifiée
        //</summary>
        //<param name="par1">la liste des objets entités à afficher</param>
        public void renderEntity(Entity[] par1)
        {
            for (int i = 0; i < par1.Length; i++)
            {
                if (par1[i] != null)
                {
                    Video.Screen.Blit(font.Render(par1[i].getChar(), par1[i].getColor()), new Point(x + par1[i].getX() * 30, y + par1[i].getY() * 30));
                }
            }
        }

        //<summary>
        //Déplace la caméra aux coordonées spécifiées
        //</summary>
        //<param name="par1x">la pisition x</param>
        //<param name="par2y">la pisition y</param>
        public void move(int par1x, int par2y)
        {
            x = par1x;
            y = par2y;
        }

        //<summary>
        //retourne la psoition x de la caméra
        //</summary>
        public int getX()
        {
            return x;
        }

        //<summary>
        //retourne la psotion y de la caméra
        //</summary>
        public int getY()
        {
            return y;
        }
    }
}
