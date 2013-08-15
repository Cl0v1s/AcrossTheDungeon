using System;
using System.Drawing;

namespace DW
{
    class House
    {
        public int x;
        public int y;
        public int width;
        public int height;
        Stair stair;
        Random rand = new Random();

        public static int[][,] shapes;

        public House(int par1x, int par2y, int par3width, int par4height,Stair par5stair)
        {
            x = par1x;
            y = par2y;
            width = par3width;
            height = par4height;
            stair = par5stair;
        }

        //<summary>
        //Enregistre les différentes formes de maison possibles
        //</summary>
        public static void setShapes()
        {
            shapes = new int[2][,];
            shapes[0] = new int[,]
            {
                 {2,2,2,2},
                 {2,1,1,2},
                 {2,1,1,-1},
                 {2,1,1,2},
                 {2,2,2,2}
            };
            shapes[1] = new int[,]
            {
                 {2,2,2,2},
                 {2,1,1,2},
                 {2,1,1,-1},
                 {2,1,1,2},
                 {2,1,1,2},
                 {2,1,1,2},
                 {2,1,1,2},
                 {2,2,2,2}
            };
        }

        //<summary>
        //Remplit et definit la salle avec des elements décoratifs et des PNJ
        //</summary>
        public House fill(BiomeVillage par1parent,int[,] par2shape)
        {
            for (int u = 0; u < par2shape.GetLength(0); u++)
            {
                for (int o = 0; o < par2shape.GetLength(1); o++)
                {
                    if (par2shape[u, o] != -1 && stair.map[u + x, y + o + 1] == 2)
                    {
                        Console.WriteLine("House aborted !");
                        return null;
                    }
                    if (!Entity.canWalkOn(x + u, y + o, stair))
                    {
                        Console.WriteLine("House aborted !");
                        return null;
                    }
                }
            }
            for (int u = 0; u < par2shape.GetLength(0); u++)
            {
                for (int o = 0; o < par2shape.GetLength(1); o++)
                {
                        if (par2shape[u, o] != -1)
                            stair.map[x + u, y + o] = par2shape[u, o];
                        else
                        {
                            stair.map[x + u, y + o] = 1;
                            Door d = new Door(stair.map, x + u, y + o);
                            d.setPos(stair, x + u, y + o);
                            stair.setSpecial(d, x + u, y + o);
                        }
                    }
            }

            int b = rand.Next(1, 3);
            for (int i = 0; i < b; i++)
            {
                Point z = getFreeCase();
                if (z != Point.Empty)
                {
                    Pnj p = new Pnj(this);
                    p.setPos(stair, z.X, z.Y);
                    stair.putEntity(p);
                    Console.WriteLine("Pnj generated in " + z);
                }
            }
            return this;
        }

        //<summary>
        //Efface les contours et le contenu de la salle
        //</summary>
        void clean(int[,] par1)
        {
            for (int u = 0; u < par1.GetLength(0); u++)
            {
                for (int o = 0; o < par1.GetLength(1); o++)
                {
                        stair.map[x + u, y + o] = 1;
                        stair.setSpecial(null, x + u, y + o);
                }
            }
            Console.WriteLine("House deleted !");
        }

        public void update()
        {

        }
        
        //<summary>
        //Retourne si le point passé en paramètre est situé dans la maison
        //</summary>
        public bool contains(Point par1)
        {
            if (par1.X >= x && par1.X <= x + width && par1.Y >= y && par1.Y <= y + height)
                return true;
            else
                return false;
        }


        //<summary>
        //Retourne une case vide dans la maison.
        //</summary>
        Point getFreeCase()
        {
            for (int i = 0; i < width; i++)
            {
                for (int u = 0; u < height; u++)
                {
                    if (Entity.canWalkOn(i + x, u + y, stair))
                        return new Point(i+x, u+y);
                }
            }
            return Point.Empty;
        }
    }
}
