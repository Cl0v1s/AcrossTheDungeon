using System;
using System.IO;
using System.Collections.Generic;


using System.Drawing;
using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Sprites;


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

        public int[] id = new int[]
        {
            0,
            0,
            1,

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
        private Surface tileset = new Surface("Data/images/TileSet.png");
        private KeyValuePair<string, AnimatedSprite>[] spriteList = new KeyValuePair<string, AnimatedSprite>[500];

        protected AnimationCollection waterAnimation;
        protected AnimatedSprite water;

        private SdlDotNet.Graphics.Font font = new SdlDotNet.Graphics.Font(Directory.GetCurrentDirectory() + "\\Data\\" + "pixel.ttf", 30);


        public Render(int par1x, int par2y)
        {
            x = par1x;
            y = par2y;
            waterAnimation = new AnimationCollection();
            SurfaceCollection e = new SurfaceCollection();
            e.Add("Data/images/Water.png", new Size(30, 30));
            waterAnimation.Add(e);
            waterAnimation.Delay = 1200;
            water = new AnimatedSprite(waterAnimation);
            water.Animate = true;
            
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
        //Enregistre un sprite annimé pour l'objet spécifié
        //</summary>
        //<param name="par">objet clef dont le hash code va servir de clef pour le dictionnaire de sprites</param>
        //<param name="par2">Sprite annimé à associé avec l'objet spcécifié</param>
        public void registerSprite(Special par1, AnimatedSprite par2)
        {

            for (int i = 0; i < spriteList.Length; i++)
            {
                if (spriteList[i].Key==null)
                {
                    spriteList[i] = new KeyValuePair<string, AnimatedSprite>(par1.GetHashCode().ToString(), par2);
                    Console.WriteLine(par1.GetHashCode().ToString());
                    return;
                }
                else
                {
                    Console.WriteLine(par1.GetHashCode().ToString());
                    if (spriteList[i].Key == par1.GetHashCode().ToString())
                        return;
                }

            }
        }

        //<summary>
        //retourne le sprite animé associé à l'objet spécifié precedemment enregistré grace à la méthode registerSprite
        //</summary>
        //<param name="par1">objet associé au sprite animé</param>
        private AnimatedSprite getSprite(Special par1)
        {
            for (int i = 0; i < spriteList.Length; i++)
            {
                if (!spriteList[i].Equals(null))
                {
                    //Console.WriteLine(spriteList[i].Key);
                    if (spriteList[i].Key == par1.GetHashCode().ToString())
                    {
                        AnimatedSprite e = spriteList[i].Value;
                        switch (par1.getFace())
                        {
                            case "front":
                                if (e.Frame > 3)
                                    e.Frame = 0;
                                break;
                            case "left":
                                if (e.Frame < 8)
                                    e.Frame = 8;
                                if (e.Frame > 11)
                                    e.Frame = 8;
                                break;
                            case "right":
                                if (e.Frame < 4)
                                    e.Frame = 4;
                                if (e.Frame > 7)
                                    e.Frame = 4;
                                break;
                            case "back":
                                if (e.Frame < 12)
                                    e.Frame = 12;
                                if (e.Frame > 15)
                                    e.Frame = 12;
                                break;
                        }
                                return spriteList[i].Value;
                    }
                }
            }
            return null;
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
                            {
                                if (par1map[i, u] < id.Length)
                                {
                                    if (par1map[i, u] != 2)
                                        Video.Screen.Blit(tileset, new Point(x + i * 30, y + u * 30), new Rectangle(id[par1map[i, u]] * 30, 0, 30, 30));
                                    else
                                        connectionTile(par1map,i,u);
                                }
                                else
                                    Video.Screen.Blit(font.Render((string)value[par1map[i, u] * 2], (Color)value[par1map[i, u] * 2 + 1]), new Point(x + i * 30, y + u * 30));

                            }
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


        private void connectionTile(int[,] par1map, int par2x,int par3y)
        {
            int idToRender = par1map[par2x,par3y];
            if (par1map[par2x - 1, par3y] != idToRender && par1map[par2x + 1, par3y] != idToRender && par1map[par2x, par3y + 1] != idToRender)
                idToRender = id[idToRender];
            else if(par1map[par2x, par3y + 1] != idToRender)
                idToRender = id[idToRender] + 1;
            else
                idToRender = id[idToRender] + 2;
            Video.Screen.Blit(tileset, new Point(x + par2x * 30, y + par3y * 30), new Rectangle(idToRender * 30, 0, 30, 30));
        }


        //<summary>
        //affiche l'entité spécifiée
        //</summary>
        //<param name="par1">l'entité</param>
        private void renderEntityAt(Entity par1)
        {
            AnimatedSprite e=getSprite(par1);
            if(e != null)
                Video.Screen.Blit(e, new Point(x + par1.getX() * 30, y + par1.getY() * 30));
            else
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
            if (par1map[par2x, par3y] == 100)
            {
                if (par1map[par2x, par3y - 1] == 100 && water.Frame < 2)
                    water.Frame = 2;
                if (par1map[par2x, par3y - 1] != 100 && water.Frame > 1)
                    water.Frame = 0;

                Video.Screen.Blit(water, new Point(x + par2x * 30, y + par3y * 30));
            }
            else
            {
                try
                {
                    if (frame < 40 / 2)
                        Video.Screen.Blit(font.Render((string)animated[(par1map[par2x, par3y] - 100) * 4], (Color)animated[(par1map[par2x, par3y] - 100) * 4 + 1]), new Point(x + par2x * 30, y + par3y * 30));
                    else
                        Video.Screen.Blit(font.Render((string)animated[(par1map[par2x, par3y] - 100) * 4 + 2], (Color)animated[(par1map[par2x, par3y] - 100) * 4 + 3]), new Point(x + par2x * 30, y + par3y * 30));
                }
                catch (Exception)
                {


                }
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
                    if (par1map[i, u] != null)
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
                    AnimatedSprite e = getSprite(par1[i]);
                    if (e != null)
                        Video.Screen.Blit(e, new Point(x + par1[i].getX() * 30, y + par1[i].getY() * 30));
                    else
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

