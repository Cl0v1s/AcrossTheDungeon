using System;


using System.Drawing;

namespace DW
{
    [Serializable]
    class BiomeCave : Biome
    {


        private new Entity[] entities = new Entity[]
            {
                new Bat()
            };

        public BiomeCave()
            : base()
        {

        }

        public BiomeCave(Stair par1stair, int par2x, int par3y, int par4width, int par5height)
            : base(par1stair, par2x, par3y, par4width, par5height)
        { }

        //<summary>
        //applique les spécificité du biome aux diverses salles de ce dernier
        //</summary>
        public override void apply()
        {
            for (int i = 0; i < rooms.Length; i++)
            {
                if (rooms[i] != null)
                {
                    applyWater(rooms[i]);
                    applyGravel(rooms[i]);
                    applyLava(rooms[i]);
                }
            }
        }

        private void applyLava(Room par1room)
        {
            int tried = 0;
            int xp = rand.Next(1, par1room.getW() - 1);
            int yp = rand.Next(1, par1room.getH() - 1);
            while (par1room.getMap()[xp, yp] != 1)
            {
                if (tried >= 500)
                    return;
                xp = rand.Next(1, par1room.getW() - 1);
                yp = rand.Next(1, par1room.getH() - 1);
                tried += 1;
            }
            tried = 0;
            par1room.set(101, xp, yp);
            stair.set(101, par1room.getX() + xp, par1room.getY() + yp);
            for (int i = 0; i < rand.Next(5, 15); i++)
            {
                xp = rand.Next(1, par1room.getW() - 1);
                yp = rand.Next(1, par1room.getH() - 1);
                while (!(par1room.getMap()[xp - 1, yp] == 101 || par1room.getMap()[xp + 1, yp] == 101 || par1room.getMap()[xp, yp - 1] == 101 || par1room.getMap()[xp, yp + 1] == 101) || par1room.getMap()[xp, yp] != 1)
                {
                    if (tried >= 500)
                        return;
                    xp = rand.Next(1, par1room.getW() - 1);
                    yp = rand.Next(1, par1room.getH() - 1);
                    tried += 1;
                }
                par1room.set(101, xp, yp);
                stair.set(101, par1room.getX() + xp, par1room.getY() + yp);
            }
        }

        //<summary>
        //Pose des graviers dans la salle.
        //<summary>
        private void applyGravel(Room par2room)
        {
            int u = rand.Next(1, 10);
            int tried = 0;
            for (int i = 0; i < u; i++)
            {
                int xGravel = rand.Next(1, par2room.getW() - 1);
                int yGravel = rand.Next(1, par2room.getH() - 1);

                while (par2room.getMap()[xGravel, yGravel] != 1)
                {
                    if (tried >= 500)
                        return;
                    tried += 1;
                    xGravel = rand.Next(1, par2room.getW() - 1);
                    yGravel = rand.Next(1, par2room.getH() - 1);
                }

                par2room.set(5, xGravel, yGravel);
                stair.set(5, par2room.getX() + xGravel, par2room.getY() + yGravel);
            }
        }

        //<summary>
        //génère les entitées inhérantes au biome
        //</summary>
        //<param name="par1">nombre maximal d'entités à générer</param>
        public override Entity[] applyEntities(int par1)
        {
            if (par1 > 0)
            {
                Entity[] res = new Entity[par1];
                for (int i = 0; i < par1; i++)
                {
                    Point p = new Point(-1, -1);
                    p = stair.getFreeSpecialCase(new Rectangle(x,y,width,height));
                    int tried = 0;
                    while (contains(p.X, p.Y) == false)
                    {
                        tried += 1;
                        p = stair.getFreeSpecialCase();
                        if (tried >= 500)
                        {
                            Console.WriteLine("fail");
                            break;
                        }
                    }
                    if (p.X == -1 && p.Y == -1)
                        continue;
                    int ty = rand.Next(0, entities.Length);
                    res[i] = (Entity)(entities[ty].clone());
                    res[i].setPos(stair, p.X, p.Y);
                }
                return res;
            }
            else
            {
                return null;

            }
        }
    }
}