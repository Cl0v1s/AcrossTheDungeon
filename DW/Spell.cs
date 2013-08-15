using System;
using System.Drawing;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Sprites;
using System.Threading;

namespace DW
{
    [Serializable]
    public class Spell
    {
        public static Spell weaponAttack = new Spell(0,"Attaque", "Vous attaquez avec ce que vous avez dans les mains, tout simplement.", 0, Animation.weaponAttack, -1, 50, 1, false);


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
        public int targetStat = 0;
        AnimatedSprite target;
        Point targetPoint=Point.Empty;
        Entity caller = null;
        Entity victim = null;
        Random rand = new Random();

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
            AnimationCollection a = new AnimationCollection();
            SurfaceCollection e = new SurfaceCollection();
            e.Add("Data/images/GUI/Target.png", new Size(30, 30));
            a.Add(e);
            a.Delay = 150;
            target = new AnimatedSprite(a);
            target.Animate = true;
            
        }

        //<summary>
        //Affecte les effets du sort à la victim
        //</summary>
        public void useSpell(Entity par1caller)
        {
            Entity par2victim=null;
            caller = par1caller;
            if (targetStat == 0 && range > 1)
            {
                targetStat = 1;
                targetPoint = new Point(par1caller.x, par1caller.y);
                return;
            }
            else if (targetStat == 1 && range>1)
                return;
            else if (targetStat == 2 && range > 1)
            {
                if (victim == null)
                {
                    caller = null;
                    targetPoint = Point.Empty;
                    targetStat = 0;
                    victim = null;
                    Console.WriteLine("ya rien ici.");
                    return;
                }
                else
                {
                    par2victim = victim;
                    victim = null;
                }
            }
            else if (range <= 1)
            {
                par2victim=par1caller.getEntityInFront();
                if (par2victim == null)
                {
                    caller = null;
                    victim = null;
                    targetPoint = Point.Empty;
                    targetStat = 0;
                    return;
                }
            }

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
                    par2victim.setEnemy(par1caller);
                    par1caller.setEnemy(par2victim);
                    par1caller.lookTo(par2victim);
                    par2victim.lookTo(par1caller);
                    par2victim.WantFight = true;
                    int d = damage;
                    damage = realDamage;
                    if (d < 0)
                        return;
                    int atk = (int)(d * (par1caller.enduranceTmp * 100 / par1caller.endurance) / 100);
                    double cc = rand.NextDouble();
                    par1caller.enduranceTmp -= atk * cc * par1caller.enduranceTmp / 100;
                    cc = rand.NextDouble();
                    if (cc <= 1 / 280 * par1caller.agilite)
                        atk = (int)(atk * (1 + cc));
                    atk = atk * (1 - (par2victim.getAgilite() / 100));
                    par2victim.setLife(par2victim.getStat()[0] - atk);
                    if (par2victim.getStat()[0] <= 10 * par2victim.getLife() / 100)
                    {
                        par2victim.WantFight = false;
                        par2victim.setFear(10);
                    }
                    coolDownFrame = coolDown;
                    DW.render.animationManager.addAnimation(animation,par2victim.x,par2victim.y,par1caller.x,par1caller.y);
                    caller = null;
                    victim = null;
                    targetPoint = Point.Empty;
                    targetStat = 0;
                }
                else
                    par1caller.showMsg("Cible hors de portée.");
            }
            return;
        }

        //<summary>
        //Retourne les dommages infligés par le sort
        //</summary>
        public int getSpellDamage(Player par1caller)
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
            int d = damage;
            damage = realDamage;
            return d;
        }

        //<summary>
        //Affiche l'icon du sort à l'écran et met à jour l'etat de visée du sort.
        //</summary>
        public void update(int par1x,int par2y)
        {
            if (targetStat == 1)
            {
                Video.Screen.Blit(target,new Point(DW.render.x + targetPoint.X * 30, DW.render.y + targetPoint.Y * 30));
                if (DW.input.equals(SdlDotNet.Input.Key.LeftArrow) && isInRange(caller,targetPoint.X-1,targetPoint.Y))
                {
                    targetPoint.X -= 1;
                    Thread.Sleep(50);
                }
                else if (DW.input.equals(SdlDotNet.Input.Key.RightArrow) && isInRange(caller, targetPoint.X + 1, targetPoint.Y))
                {
                    targetPoint.X += 1;
                    Thread.Sleep(50);
                }
                else if (DW.input.equals(SdlDotNet.Input.Key.UpArrow) && isInRange(caller, targetPoint.X, targetPoint.Y - 1))
                {
                    targetPoint.Y -= 1;
                    Thread.Sleep(50);
                }
                else if (DW.input.equals(SdlDotNet.Input.Key.DownArrow) && isInRange(caller, targetPoint.X, targetPoint.Y + 1))
                {
                    targetPoint.Y += 1;
                    Thread.Sleep(50);
                }
                else if (DW.input.equals(SdlDotNet.Input.Key.Escape))
                {
                    targetPoint = Point.Empty;
                    caller = null;
                    targetStat = 0;
                }
                else if (DW.input.equals(SdlDotNet.Input.Key.Space) && caller.getStair().getEntityAt(targetPoint) != caller)
                {
                    victim=caller.getStair().getEntityAt(targetPoint);
                    targetStat = 2;
                }
            }
            if(coolDownFrame>=1)
                coolDownFrame -= 1;
            Video.Screen.Blit(icon, new Point(par1x, par2y));
            if (coolDownFrame > 0)
            {
                int c = coolDownFrame * 20 / coolDown;
                Video.Screen.Blit(shadow, new Point(par1x + 5, par2y + 5+c),new Rectangle(0,c,20,20-c));
            }
        }

        //<summary>
        //Vérifie si l'entité visée est hors de portée ou non.
        //</summary>
        bool isInRange(Entity par1caller, Entity par2victim)
        {
            int par1x = par2victim.x;
            int par2y = par2victim.y;
            int x = par1caller.x;
            int y = par1caller.y;
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


        //<summary>
        //Vérifie si l'entité visée est hors de portée ou non.
        //</summary>
        bool isInRange(Entity par1caller, int par2x,int par3y)
        {
            int par1x = par2x;
            int par2y = par3y;
            int x = par1caller.x;
            int y = par1caller.y;
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
                    if (onAir == false && DW.player.canWalkOn(rx, ry) == false)
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
