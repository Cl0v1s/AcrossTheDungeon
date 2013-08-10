using System;
using System.Drawing;

namespace DW
{
    class BiomeVillage : Biome
    {

        int[][,] buildings;
        Rectangle[] houses;

        public BiomeVillage(Stair par1stair, int par2x, int par3y, int par4width, int par5height)
            : base(par1stair, par2x, par3y, par4width, par5height)
        {
            setBuildings();    
        }

        void setBuildings()
        {
            buildings = new int[2][,];
            buildings[0] = new int[,]
            {
                 {2,2,2,2},
                 {2,1,1,2},
                 {2,1,1,-1},
                 {2,1,1,2},
                 {2,2,2,2}
            };
            buildings[1] = new int[,]
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
            houses = new Rectangle[buildings.Length];
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
                    int nb = rand.Next(1, 10);
                    for (int r = 0; r < nb; r++)
                    {
                        int[,] build = buildings[rand.Next(0, buildings.Length)];
                        if (build == null || rooms[i].width - build.GetLength(0)<2 || rooms[i].height- build.GetLength(1)<2)
                            continue;
                        int x = rooms[i].x + rand.Next(2, rooms[i].width- build.GetLength(0));
                        int y = rooms[i].y + rand.Next(2, rooms[i].height- build.GetLength(1));
                        bool p = false;
                        for (int u = 0; u < houses.Length; u++)
                        {
                                if (houses[u] != Rectangle.Empty && x + build.GetLength(0) >= houses[u].X && x <= houses[u].X + houses[u].Width && y + build.GetLength(1) >= houses[u].Y && y <= houses[u].Y + houses[u].Height)
                                {
                                    p = true;
                                    Console.WriteLine("House can't be put at " + x + ":" + y + " (" + build.GetLength(0) + "/" + build.GetLength(1) + ")");
                                    break;
                                }
                        }
                        if (p)
                            continue;
                        for (int u = 0; u < houses.Length; u++)
                        {
                            if (houses[u] == Rectangle.Empty)
                            {
                                houses[u] = new Rectangle(x, y, build.GetLength(0), build.GetLength(1));
                                break;
                            }
                        }
                        try
                        {
                            for (int u = 0; u < build.GetLength(0); u++)
                            {
                                for (int o = 0; o < build.GetLength(1); o++)
                                {
                                    if (build[u, o] != -1)
                                        stair.map[x + u, y + o] = build[u, o];
                                    else
                                    {
                                        stair.map[x + u, y + o] = 1;
                                        Door d = new Door(stair.map, x + u, y + o);
                                        d.setPos(stair, x + u, y + o);
                                        stair.setSpecial(d, x + u, y + o);
                                    }
                                }
                            }
                        }
                        catch (Exception)
                        { }
                        Console.WriteLine("House put in " + x + ":" + y + " (" + build.GetLength(0) + "/" + build.GetLength(1) + ")");
                    }
                }
            }
        }
    }
}
