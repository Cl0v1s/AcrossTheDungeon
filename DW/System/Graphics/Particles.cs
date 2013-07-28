using System;
using System.Drawing;
using SdlDotNet.Graphics.Sprites;
using SdlDotNet.Graphics;

namespace DW
{
    public class Particles
    {
        private Particle[] particles;

        public Particles(Color par1,int par2x, int par3y)
        {
            Random rand = new Random();
            particles = new Particle[rand.Next(50, 150)];
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i] = new Particle(par1, par2x, par3y);
            }
        }

        public Particles update()
        {
            bool f=false;
            for (int i = 0; i < particles.Length; i++)
            {
                if (particles[i] != null)
                {
                    particles[i] = particles[i].update();
                    f = true;
                }
                    
            }
            if (f)
                return this;
            else
                return null;
        }
    }

    public class Particle
    {
        private Surface rectangle;
        private int x;
        private int y;
        private int addX;
        private int addY;
        private int alpha;
        private int alphaLoss;

        public Particle(Color par1source, int par2x, int par3y)
        {
            Random rand=new Random();
            rectangle = new Surface(rand.Next(2, 5), rand.Next(2, 5));
            rectangle = rectangle.Convert(Video.Screen);
            rectangle = rectangle.CreateRotatedSurface(rand.Next(0, 360));
            rectangle.AlphaBlending = true;
            alpha=rand.Next(50, 205);
            rectangle.Fill(par1source);
            alphaLoss = rand.Next(1, 25);
            rectangle.Alpha = (byte)alpha;
            x = par2x;
            y = par3y;
            addX = rand.Next(-2, 2);
            addY = rand.Next(-2, 2);
        }

        public Particle update()
        {
            alpha -= alphaLoss;
            if (alpha <= 0)
                return null;
            x += addX;
            y += addY;
            rectangle.Alpha = (byte)alpha;
            Video.Screen.Blit(rectangle, new Point(x, y));
            return this;
        }
    }
}
