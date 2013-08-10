using System;

using System.Drawing;

namespace DW
{
    [Serializable]
    class Room
    {
        private Stair stair;
        private Random rand = new Random(DateTime.Now.Millisecond);
        public int width;
        public int height;
        public int x;
        public int y;
        public int[,] map;
        private Special[,] special;
        public Point[] doors;
        private bool[] doorChecked;
        public bool ended = false;


        public Room(Stair par1stair)
        {
            stair = par1stair;
            width = rand.Next(5, 15);
            height = rand.Next(5, 15);
            x = rand.Next(0, stair.getW());
            y = rand.Next(0, stair.getH());
        }

        public bool check(Room[] par1rooms)
        {
            for (int o = 0; o < par1rooms.Length; o++)
            {
                if (par1rooms[o] != null && !par1rooms[o].Equals(this))
                {
                    Room r = par1rooms[o];
                    if ((x + width >= r.getX() && x <= r.getX() + r.getW() && y + height > r.getY() && y <= r.getY() + r.getH()) || x + width >= stair.getW() || y + height >= stair.getH())
                    {
                        return false;
                    }
                }
            }
            createMap();
            return true;
        }

        private void createMap()
        {
            map = new int[width + 1, height + 1];
            special = new Special[width + 1, height + 1];
            for (int i = 0; i < width; i++)
            {
                for (int u = 0; u < height; u++)
                {
                    map[i, u] = 1;
                }
            }
            for (int i = 0; i < width + 1; i++)
            {
                map[i, 0] = 2;
                map[i, height] = 2;
            }
            for (int i = 0; i < height + 1; i++)
            {
                map[0, i] = 2;
                map[width, i] = 2;
            }
            doors = new Point[20];
            doorChecked = new bool[doors.Length];
        }

        //<summary>
        //Créer une porte et retourne sa position.
        //</summary>
        public Point createDoor()
        {
                int pos = rand.Next(1, 4);
                int r;
                Point p;
                switch (pos)
                {
                    case 1:
                        r = rand.Next(1, width - 1);
                        p = new Point(r, 0);
                        break;
                    case 2:
                        r = rand.Next(1, width - 1);
                        p = new Point(r, height);
                        break;
                    case 3:
                        r = rand.Next(1, height - 1);
                        map[0, r] = 3;
                        p = new Point(0, r);
                        break;
                    case 4:
                        r = rand.Next(1, height - 1);
                        p = new Point(width, r);
                        break;
                    default:
                        goto case 1;
                }
                for (int i = 0; i < doors.Length; i++)
                {
                    if (doors[i] == Point.Empty)
                    {
                        doors[i] = p;
                        return p;
                    }
                }
                return p;
        }

        public bool contains(int par1x, int par2y)
        {
            if (par1x >= x && par1x <= x + width && par2y >= y && par2y <= y + height)
                return true;
            return false;
        }

        public void checkDoor(int par1)
        {
            doorChecked[par1] = true;
        }

        public bool isDoorChecked(int par1)
        {
            try
            {
                return doorChecked[par1];
            }
            catch (Exception)
            {
                return true;
            }
        }

        public Point[] getDoors()
        {
            return doors;
        }

        public int getX()
        {
            return x;
        }

        public int getY()
        {
            return y;
        }

        public int getW()
        {
            return width;
        }

        public int getH()
        {
            return height;
        }

        public int[,] getMap()
        {
            return map;
        }

        public void set(int par1case, int par2x, int par3y)
        {
            try
            {
                map[par2x, par3y] = par1case;
            }
            catch (Exception)
            { }
        }

        public void setSpecial(Special par1case, int par2x, int par3y)
        {
            special[par2x, par3y] = par1case;
        }

        public Special[,] getSpecial()
        {
            return special;
        }

        public Point getCaseInStairPos(int par1x, int par2y)
        {
            return new Point(x + par1x, y + par2y);
        }

        //<summary>
        //Retourne si tout les portes de la salle courante ont deja été reliées
        //</summary>
        public bool allDoorsChecked()
        {
            for (int i = 0; i < doors.Length; i++)
            {
                if (!isDoorChecked(i))
                    return false;
            }
            return true;
        }



    }
}
