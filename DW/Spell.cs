using System;
using System.Drawing;
using SdlDotNet.Graphics;

namespace DW
{
    [Serializable]
    public class Spell
    {
        public static Spell weaponAttack = new Spell(0,"Attaque", "Vous attaquez avec ce que vous avez dans les mains, tout simplement.", 0, Animation.Damage, -1, 50, 1, false);


        public static Spell[] list = new Spell[]
        {
            weaponAttack
        };


        private Surface icon;
        private int damage;
        private int realDamage;
        private int coolDown;
        private int coolDownFrame;
        private int range;
        private bool onAir;
        private Animation animation;
        public string name;
        public string description;
        private Surface shadow;
        public int id;

        //<summary>
        //créer un sort.
        //Les statistiques du joueur peuvent être utilisée comme dommage en utilisant le code suivant;
        // - -1 = force
        //à venir
        //</summary>
        public Spell(int par8id,string par0name,string par7description,int par1fileIndex,Animation par6animation, int par2damage, int par3cooldown, int par4range,bool par5onair)
        {
            id = par8id;
            icon = new Surface(30, 30);
            icon.Blit(new Surface("Data/images/GUI/Spells.png"), new Point(0, 0), new Rectangle(30 * par1fileIndex, 0, 30, 30));
            damage = par2damage;
            realDamage=damage;
            coolDown = par3cooldown;
            range = par4range;
            onAir = par5onair;
            animation = par6animation;
            name = par0name;
            description = par7description;
            shadow = new Surface(20, 20).Convert(Video.Screen);
            shadow.AlphaBlending = true;
            shadow.Fill(Color.Red);
            shadow.Alpha = 150;
            
        }

        public int useSpell(Entity par1caller,Entity par2victim)
        {
            if (damage < 0)
            {
                switch (damage)
                {
                    case -1:
                        damage = par1caller.force;
                        break;

                }
            }

            if (coolDownFrame <= 0)
            {
                if (isInRange(par1caller, par2victim))
                {
                    coolDownFrame = coolDown;
                    int d = damage;
                    damage = realDamage;
                    DW.render.addAnimation(animation,par2victim.getX(),par2victim.getY());
                    return d;
                }
                else
                    par1caller.showMsg("Cible hors de portée.");
            }
            return -1;
        }

        public void update(int par1x,int par2y)
        {
            if(coolDownFrame>=1)
                coolDownFrame -= 1;
            Video.Screen.Blit(icon, new Point(par1x, par2y));
            if (coolDownFrame > 0)
            {
                int c = coolDownFrame * 20 / coolDown;
                Video.Screen.Blit(shadow, new Point(par1x + 5, par2y + 5+c),new Rectangle(0,c,20,20-c));
            }



        }

        public bool isInRange(Entity par1caller, Entity par2victim)
        {
            int par1x = par2victim.getX();
            int par2y = par2victim.getY();
            int x = par1caller.getX();
            int y = par1caller.getY();
            if (Math.Abs(x - par1x) + Math.Abs(y - par2y) <= range)
            {
                int dx = Math.Abs(par1x - x);
                int dy = Math.Abs(par2y - y);
                int sx = 1;
                int rx = x;
                int ry = y;
                if (x > par1x)
                    sx = -1;
                int sy = 1;
                if (y > par2y)
                    sy = -1;
                int err = dx - dy;
                while (!((rx == par1x) && (ry == par2y)))
                {
                    int e2 = err << 1;
                    if (e2 > -dy)
                    {
                        err -= dy;
                        rx += sx;
                    }
                    if (e2 < dx)
                    {
                        err += dx;
                        ry += sy;
                    }
                    if (onAir==false && DW.player.canWalkOn(rx, ry) == false)
                    {
                        break;
                    }
                }
                if (rx != par1x || ry != par2y)
                    return false;
                return true;
            }
            return false;
        }
    }
}
