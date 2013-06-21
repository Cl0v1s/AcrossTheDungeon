using System;

using System.Drawing;

namespace DW
{
    [Serializable]
    class BiomeGarden : Biome
    {

        private new Entity[] entities = new Entity[]
            {
                new Pig()
            };

        private object[] specialCase = new object[]
        {
            new Plant(),
        };


        //<summary>
        //Créer le biome Jardin
        //</summary>
        public BiomeGarden()
            : base()
        {
           
        }

        //<summary>
        //Créer le biome jardin
        //</summary>
        //<param name="par1stair">Etage du donjon dans lequel est situé le biome</param>
        //<param name="par2x">position x du biome dans l'étage</param>
        //<param name="par3y">position y du biome dans l'étage</param>
        //<param name="par4width">largeur du biome dans l'étage</param>
        //<param name="par5height">hauteur du biome dans l'étage</param>
        public BiomeGarden(Stair par1stair, int par2x, int par3y, int par4width, int par5height)
            : base(par1stair, par2x, par3y, par4width, par5height)
        {

        }

        //<summary>
        //applique les spécificité du biome aux diverses salles de ce dernier
        //</summary>
        public override void apply()
        {
            for (int i = 0; i < rooms.Length; i++)
            {
                if (rooms[i] != null)
                {
                   applyWeed(rooms[i]);
                   applySpecialCase(rooms[i]);
                   applyWater(rooms[i]);
                }
            }            
        }

        //<summary>
        //applique de l'herbe dans la salle passée en argument
        //</summary>
        //<param name="par1">salle à affecter</param>
        private void applyWeed(Room par1)
        {
            Point c = new Point(par1.getW() / 2, par1.getH() / 2);
            for (int i = 1; i < par1.getW() - 1; i++)
            {
                for (int u = 1; u < par1.getH() - 1; u++)
                {
                    if (rand.Next(0, 50) != 1 && par1.getMap()[i,u] == 1)
                    {
                        int h = Math.Abs(c.X - i) + Math.Abs(c.Y - u);
                        int j=0;
                        if(c.X-i<0)
                            j = Math.Abs(par1.getW() - i);
                        else 
                            j=Math.Abs(0-i);
                        if(c.Y-u<0)
                            j+=Math.Abs(par1.getH()-u);
                        else 
                            j+=Math.Abs(0-u);
                        int d = (h - j)*20;
                        if (d < 0)
                            d = 0;
                        int r = rand.Next(0, d);
                        if ( r ==0 )
                        {
                            par1.set(6, i, u);
                            stair.set(6, par1.getX() + i, par1.getY() + u);
                        }
                    }
                }
            }
        }

        //<summary>
        //applique les spécificité du biome à la salle de ce dernier passée en argument
        //</summary>
        //<param name="par2room">salle à affecter</param>
        public void applySpecialCase(Room par2room)
        {
            int w = par2room.getW();
            int h = par2room.getH();
            for (int i = 1; i < w - 1; i++)
            {
                for (int u = 1; u < h - 1; u++)
                {
                    if (par2room.getMap()[i, u] != 2 && rand.Next(0, 50) == 1)
                    {
                        int r = rand.Next(0, specialCase.Length);
                        try
                        {
                            stair.set((int)specialCase[r], par2room.getX() + i, par2room.getY() + u);
                            par2room.set((int)specialCase[r], i, u);
                        }
                        catch (System.InvalidCastException)
                        {
                            Special s = (((Special)specialCase[r]).clone());
                            s.setPos(stair,par2room.getX() + i, par2room.getY() + u);
                            stair.setSpecial(s, par2room.getX() + i, par2room.getY() + u);
                            par2room.setSpecial(s, i, u);
                        }
                        catch (System.IndexOutOfRangeException) { }
                    }

                }
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
                    p = stair.getFreeSpecialCase(new Rectangle(x, y, width, height));
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
