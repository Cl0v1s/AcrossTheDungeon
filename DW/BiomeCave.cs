using System;


using System.Drawing;

namespace DW
{
    [Serializable]
    class BiomeCave : Biome
    {
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
                    applyGravel(rooms[i]);
                    applyWater(rooms[i]);
                }
            }
        }

        //<summary>
        //Pose des graviers dans la salle.
        //<summary>
        private void applyGravel(Room par2room)
        {
            int u=rand.Next(1,10);
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


    }
}