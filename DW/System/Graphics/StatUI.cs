using System;
using System.Drawing;

using SdlDotNet.Graphics;
using SdlDotNet.Core;

namespace DW
{
    [Serializable]
    class StatUI
    {
        private Entity subject;
        private Surface background_left = new Surface("Data/images/GUI/StatUI_background.png");
        private Surface background_right = new Surface("Data/images/GUI/StatUI_background.png");
        private Surface jauge = new Surface("Data/images/GUI/jauge.png");
        private Surface Icons = new Surface("Data/images/Gui/Icon.png");
        private Surface itemInHand = null;
        private string lastItemInHand="";
        private Surface[] statsIcon = new Surface[4];
        private float frame;
        private Random rand = new Random();



        public StatUI(Entity par1)
        {
            subject = par1;
            itemInHand = new Surface(35, 35).Convert(Video.Screen);
            itemInHand.Blit(new Surface("Data/images/GUI/Slot.png"));
            itemInHand.CreateScaledSurface(6, 6, false);
        }

        public void setOwner(Player par1)
        {
            subject = par1;
        }
        

        public void update()
        {
            frame += (float)3.14/50;
            if (frame >= 100)
                frame = 0;
            Video.Screen.Blit(background_left,new Point(-440,400));
            Video.Screen.Blit(background_left, new Point(455, 400));
            new Text("pixel.ttf", 25, 20, 410, subject.getName(), 200, 200, 200).update();
            /*Life*/
            Video.Screen.Fill(new Rectangle(45, 447, subject.getStat()[0] * 98 / subject.getLife(), 16), Color.FromArgb(50, 250, 50));
            Video.Screen.Blit(jauge, new Point(45, 445));
            Video.Screen.Blit(Icons, new Point(22, 442), new Rectangle(0, 0, 30, 30));
            /*ItemInHand*/
            if (itemInHand != null)
                Video.Screen.Blit(itemInHand, new Point(475, 422));
            if (((Player)subject).getItemInHand() != null && lastItemInHand != ((Player)subject).getItemInHand().getName())
            {
                itemInHand = new Surface(35, 35).Convert(Video.Screen);
                itemInHand.Blit(new Surface("Data/images/GUI/Slot.png"));
                itemInHand.Blit(DW.render.getSprite(((Player)subject).getItemInHand().getName()),new Point(2,2));
                itemInHand.CreateScaledSurface(6, 6, false);
                lastItemInHand = ((Player)subject).getItemInHand().getName();
            }
            else if (((Player)subject).getItemInHand() == null)
            {
                itemInHand = new Surface(35, 35).Convert(Video.Screen);
                itemInHand.Blit(new Surface("Data/images/GUI/Slot.png"));
                itemInHand.CreateScaledSurface(6, 6, false);
            }
            renderSurvivalStat();
        }

        public void renderSurvivalStat()
        {
            manageStatIcon();
            int yd = 0;
            int xd = 0;
            for (int i = 1; i < 4; i++)
            {
                if (statsIcon[i] != null)
                {
                    if (subject.getStat()[i] >= 90)
                    {
                        int m = (int)(8 * Math.Sin((double)frame)*i/3);
                        Video.Screen.Blit(statsIcon[i], new Point(540 + 45 * xd, 415 + yd+m));
                    }
                    else
                        Video.Screen.Blit(statsIcon[i], new Point(540 + 45 * xd, 415 + yd));

                    xd += 1;
                    if (xd >= 2)
                    {
                        xd = 0;
                        yd += 35;
                    }
                }
            }
        }

        public void manageStatIcon()
        {
            for (int i = 1; i < 4; i++)
            {
                if (subject.getStat()[i] >= 70 && statsIcon[i] == null)
                {
                    Surface h = new Surface(30, 30).Convert(Video.Screen);
                    h.Fill(Color.Fuchsia);
                    h.Blit(Icons, new Point(0,0), new Rectangle(30*i, 0, 30, 30));
                    h.SourceColorKey = Color.Fuchsia;
                    statsIcon[i] = h;
                }
            }
        }


    }
}
