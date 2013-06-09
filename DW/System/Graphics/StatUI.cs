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
        private Surface background = new Surface("Data/images/GUI/StatUI_background.png");
        private Surface jauge = new Surface("Data/images/GUI/jauge.png");
        private Surface Icons = new Surface("Data/images/Gui/Icon.png");




        public StatUI(Entity par1)
        {
            subject = par1;
        }

        public void setOwner(Player par1)
        {
            subject = par1;
        }
        

        public void update()
        {
            int dec = 1;
            Video.Screen.Blit(background,new Point(0,400));
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
            
            /*
            Video.Screen.Fill(new Rectangle(20,470,100,10),Color.DarkGray);
            Video.Screen.Fill(new Rectangle(20, 470, subject.getStat()[0]*100/subject.getLife(), 10), Color.FromArgb(50,255,50));
            Video.Screen.Fill(new Rectangle(140, 470, 100, 10), Color.DarkGray);
            Video.Screen.Fill(new Rectangle(140, 470, subject.getStat()[1] * -1 + 100, 10), Color.FromArgb(255,50,50));
            Video.Screen.Fill(new Rectangle(260, 470, 100, 10), Color.DarkGray);
            Video.Screen.Fill(new Rectangle(260, 470, subject.getStat()[2] * -1 + 100, 10), Color.FromArgb(50, 50, 255));
            Video.Screen.Fill(new Rectangle(380, 470, 100, 10), Color.DarkGray);
            Video.Screen.Fill(new Rectangle(380, 470, subject.getStat()[3] * -1 + 100, 10), Color.FromArgb(50, 50, 150));
            Video.Screen.Fill(new Rectangle(500, 470, 100, 10), Color.DarkGray);
            Video.Screen.Fill(new Rectangle(500, 470, subject.getStat()[4] * -1 + 100, 10), Color.FromArgb(250, 250, 250));*/
            
        }
    }
}
