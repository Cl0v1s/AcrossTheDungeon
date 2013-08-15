using System;
using System.Drawing;

namespace DW
{
    class BiomeVillage : Biome
    {

        House[] houses;

        public BiomeVillage(Stair par1stair, int par2x, int par3y, int par4width, int par5height)
            : base(par1stair, par2x, par3y, par4width, par5height)
        {
            setBuildings();    
        }

        //<summary>
        //ENregistre les différentes formes de maisons disponibles
        //</summary>
        void setBuildings()
        {
            House.setShapes();
            houses = new House[House.shapes.Length];
        }

        public BiomeVillage()
            : base()
        {
            setBuildings();
        }

        //<summary>
        //Applique les pecificité de ce biome.
        //</summary>
        public override void apply()
        {
            Console.WriteLine("Generating Village...");
            applyBuildings();
        }

        //<summary>
        //Place les bâtiments sur la map
        //</summary>
        void applyBuildings()
        {
            for (int i = 0; i < rooms.Length; i++)
            {
                if (rooms[i] != null && rooms[i].width>=12 && rooms[i].y>=12)
                {
                    Console.WriteLine("Creating houses in Room " + i);
                    int nb = rand.Next(1, 10);
                    for (int r = 0; r < nb; r++)
                    {
                        int[,] build = House.shapes[rand.Next(0, House.shapes.Length)];
                        if (build == null || rooms[i].width - build.GetLength(0)<2 || rooms[i].height- build.GetLength(1)<2)
                            continue;
                        int x = rooms[i].x + rand.Next(2, rooms[i].width- build.GetLength(0));
                        int y = rooms[i].y + rand.Next(2, rooms[i].height- build.GetLength(1));
                        bool p = false;
                        /*
                        for (int u = 0; u < houses.Length; u++)
                        {
                                if (houses[u] != null && x + build.GetLength(0) >= houses[u].x && x <= houses[u].x + houses[u].width && y + build.GetLength(1) >= houses[u].y && y <= houses[u].y + houses[u].height)
                                {
                                    p = true;
                                    Console.WriteLine("House can't be put at " + x + ":" + y + " (" + build.GetLength(0) + "/" + build.GetLength(1) + ")");
                                    break;
                                }
                        }*/
                        if (p)
                            continue;
                        for (int u = 0; u < houses.Length; u++)
                        {
                            if (houses[u] == null)
                            {
                                houses[u] = new House(x, y, build.GetLength(0), build.GetLength(1),stair);
                                houses[u] = houses[u].fill(this, build);
                                break;
                            }
                        }
                        Console.WriteLine("House put in " + x + ":" + y + " (" + build.GetLength(0) + "/" + build.GetLength(1) + ")");
                    }
                }
            }
        }

        //<summary>
        //Retourne si le point pass" en paramètre se trouve dans une maison.
        //</summary>
        public bool isInHouse(Point par1)
        {
            for (int u = 0; u < houses.Length; u++)
            {
                if (houses[u] != null && houses[u].contains(par1))
                {
                    return true;
                }
            }
            return false;
        }

        //<summary>
        //Met à jour les maisons
        //</summary>
        public void update()
        {
            for (int i = 0; i < houses.Length; i++)
            {
                if (houses[i] != null)
                {
                    houses[i].update();
                }
            }
        }

    }
}
