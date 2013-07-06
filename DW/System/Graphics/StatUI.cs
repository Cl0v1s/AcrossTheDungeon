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
            int dec = 1;
            Video.Screen.Blit(background_left,new Point(-440,400));
            Video.Screen.Blit(background_left, new Point(455, 400));
            new Text("pixel.ttf", 25, 20, 410, subject.getName(), 200, 200, 200).update();
            /*Life*/
            Video.Screen.Fill(new Rectangle(45, 447, subject.getStat()[0] * 98 / subject.getLife(), 16), Color.FromArgb(50, 250, 50));
            Video.Screen.Blit(jauge, new Point(45, 445));
            Video.Screen.Blit(Icons, new Point(22, 442), new Rectangle(0, 0, 30, 30));
            /*Hungry*/
            if (subject.getStat()[1] >= 70)
            {
                Video.Screen.Blit(Icons, new Point(100*dec+65, 442), new Rectangle(30, 0, 30, 30));
                new Text("pixel.ttf",20,100*dec+65+35,447,(subject.getStat()[1]*-1+100).ToString()).update();
                dec += 1;
            }
            /*thrirst*/
            if (subject.getStat()[2] >= 70)
            {
                Video.Screen.Blit(Icons, new Point(100*dec+65, 442), new Rectangle(60, 0, 30, 30));
                new Text("pixel.ttf", 20, 100*dec+65 + 35, 447, (subject.getStat()[2] * -1 + 100).ToString()).update();
                dec += 1;
            }
            /*sleep*/
            if (subject.getStat()[3] >= 70)
            {
                Video.Screen.Blit(Icons, new Point(100*dec+65, 442), new Rectangle(90, 0, 30, 30));
                new Text("pixel.ttf", 20, 100*dec+65 + 35, 447, (subject.getStat()[3] * -1 + 100).ToString()).update();
                dec += 1;
            }
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
        }
    }
}
