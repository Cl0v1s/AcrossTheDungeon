using System;

using System.Drawing;

namespace DW
{
    [Serializable]
    class Biome
    {
        protected Stair stair;
        protected Room[] rooms;

        protected Random rand = new Random();
        protected int x;
        protected int y;
        protected int width;
        protected int height;

        protected Entity[] entities = new Entity[]
            {

            };


        //<summary>
        //Créer le biome normal
        //</summary>
        public Biome()
        {
        }

        //<summary>
        //applique les paramètre correspondant aux biome
        //</summary>
        //<param name="par1stair">Etage du donjon dans lequel est situé le biome</param>
        //<param name="par2x">position x du biome dans l'étage</param>
        //<param name="par3y">position y du biome dans l'étage</param>
        //<param name="par4width">largeur du biome dans l'étage</param>
        //<param name="par5height">hauteur du biome dans l'étage</param>
        public void set(Stair par1stair, int par2x, int par3y, int par4width, int par5height)
        {
            stair = par1stair;
            x = par2x;
            y = par3y;
            width = par4width;
            height = par5height;
        }

        //<summary>
        //Créer le biome normal
        //</summary>
        //<param name="par1stair">Etage du donjon dans lequel est situé le biome</param>
        //<param name="par2x">position x du biome dans l'étage</param>
        //<param name="par3y">position y du biome dans l'étage</param>
        //<param name="par4width">largeur du biome dans l'étage</param>
        //<param name="par5height">hauteur du biome dans l'étage</param>
        public Biome(Stair par1stair,int par2x,int par3y,int par4width,int par5height)
        {
            stair = par1stair;
            x = par2x;
            y = par3y;
            width = par4width;
            height = par5height;
        }

        //<summary>
        //vérifie si le biome contient le poitn situé aux coordonnées spécifiées
        //</summary>
        //<param name="par1x">position x du point à vérifier</param>
        //<param name="par2y">position y du point à vérifier</param>
        public bool contains(int par1x, int par2y)
        {
            if (par1x >= x && par1x <= x + width && par2y >= y && par2y <= y + height)
                return true;
            return false;
        }

        //<summary>
        //affecte les salles situées à l'intérieur du Biome
        //</summary>
        //<param name="par1">Tableau contenant la liste des salles contenues dans le biome</param>
        public void setRooms(Room[] par1)
        {
            rooms = par1;
        }

        //<summary>
        //retourne la hauteur du biome
        //</summary>
        public int getH()
        {
            return height;
        }

        //<summary>
        //retourne la largeur du biome
        //</summary>
        public int getW()
        {
            return width;
        }

        //<summary>
        //retourne la position x du biome
        //</summary>
        public int getX()
        {
            return x;
        }

        //<summary>
        //retourne la position y du biome
        //</summary>
        public int getY()
        {
            return y;
        }

        //<summary>
        //applique les spécificité du biome aux diverses salles de ce dernier
        //</summary>
        public virtual void apply()
        {
           
        }

        //<summary>
        //applique les spécificité du biome aux diverses salles de ce dernier
        //</summary>
        public virtual Entity[] applyEntities(int par1)
        {
            return null;
        }

        //<summary>
        //retourne une copie indépendante de l'objet courant
        //</summary>
        public Biome clone()
        {
            return (Biome)this.MemberwiseClone();
        }

        //<summary>
        //applique de l'eau dans la salle passée en argument
        //</summary>
        //<param name="par1room">salle à affecter</param>
        protected void applyWater(Room par1room)
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
            par1room.set(100, xp, yp);
            stair.set(100, par1room.getX() + xp, par1room.getY() + yp);
            for (int i = 0; i < rand.Next(5, 15); i++)
            {
                xp = rand.Next(1, par1room.getW() - 1);
                yp = rand.Next(1, par1room.getH() - 1);
                while (!(par1room.getMap()[xp - 1, yp] == 100 || par1room.getMap()[xp + 1, yp] == 100 || par1room.getMap()[xp, yp - 1] == 100 || par1room.getMap()[xp, yp + 1] == 100) || par1room.getMap()[xp, yp] != 1)
                {
                    if (tried >= 500)
                        return;
                    xp = rand.Next(1, par1room.getW() - 1);
                    yp = rand.Next(1, par1room.getH() - 1);
                    tried += 1;
                }
                par1room.set(100, xp, yp);
                stair.set(100, par1room.getX() + xp, par1room.getY() + yp);
            }
        }




    }
}
