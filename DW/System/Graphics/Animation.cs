using System;

using System.Drawing;
using SdlDotNet.Graphics.Sprites;
using SdlDotNet.Graphics;

namespace DW
{
    public class Animation
    {

        public static Animation Damage=new Animation(0,"Damage.png",100,300,true);
        public static Animation weaponAttack = new Animation(1, "Sword.png", 20, 300, false);

        public static Animation[] list = new Animation[]
        {
            Damage,
            weaponAttack,
        };

        private Surface toRender;
        private AnimatedSprite animation;
        private Point destination=Point.Empty;
        private int speed;
        private int duration;
        private int x;
        private int y;
        private double angle;
        private bool adaptAngle;
        public int id;

        public Animation(int par5id,string par1file,int par3speed, int par2duration, bool par4adaptAngle=false)
        {
            AnimationCollection a = new AnimationCollection();
            SurfaceCollection e = new SurfaceCollection();
            e.Add("Data/images/Animations/"+par1file, new Size(30, 30));
            a.Add(e);
            a.Delay = par3speed;
            animation = new AnimatedSprite(a);
            animation.AlphaBlending = true;
            speed = par3speed;
            duration = par2duration;
            toRender = new Surface(30, 30).Convert(Video.Screen);
            adaptAngle = par4adaptAngle;
            id = par5id;
        }

        //<summary>
        //Lance l'annimation depuis le point donnée et incline l'annimation selon les points de l'objectif
        //</summary>
        //<param name="par3xf">Coordonné x de la source</summary>
        //<param name="par4yf">Coordonné y de la source</summary>        
        public void start(int par1x, int par2y, int par3xf,int par4yf)
        {
            animation.Animate = true;
            animation.Frame = 0;
            x = par1x;
            y = par2y;
            if (adaptAngle)
            {

                if (par3xf == x && par4yf > y)
                    angle = 0;
                else if (par4yf == y && par3xf < x)
                    angle = 90;
                else if (par3xf == x && par4yf < y)
                    angle = 180;
                else if (par4yf == y && par3xf > x)
                    angle = 270;
            }
        }

        public void moveTo(int par1x, int par2y)
        {
            destination = new Point(par1x, par2y);
        }

        public Animation update()
        {
            if (destination != Point.Empty)
            {
                if (x < destination.X)
                    x += 1;
                else
                    x -= 1;
                if (y < destination.Y)
                    y += 1;
                else
                    y -= 1;
            }
            if (speed * animation.Frame >= duration)
                return null;
            if (angle != 0)
            {
                toRender = new Surface(30, 30);
                toRender.Fill(Color.Black);
                toRender.Blit(animation);
                toRender = toRender.CreateRotatedSurface((int)angle);
                toRender.SourceColorKey = Color.Black;
                Video.Screen.Blit(toRender, new Point(DW.render.x + x * 30, DW.render.y + y * 30));
                Console.WriteLine(angle);
            }
            else
                Video.Screen.Blit(animation, new Point(DW.render.x + x * 30, DW.render.y + y * 30));
            return this;
        }

        public Animation clone()
        {
            return (Animation)this.MemberwiseClone();
        }



    }
}
