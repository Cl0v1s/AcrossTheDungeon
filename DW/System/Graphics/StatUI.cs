using System;
using System.Drawing;

using SdlDotNet.Graphics;
using SdlDotNet.Core;

namespace DW
{
    [Serializable]
    class StatUI
    {
        Entity subject;



        public StatUI(Entity par1)
        {
            subject = par1;
        }
        

        public void update()
        {
            Video.Screen.Fill(new Rectangle(0, 430, 640, 50),Color.Black);
            Video.Screen.Fill(new Rectangle(20,470,100,10),Color.DarkGray);
            Video.Screen.Fill(new Rectangle(20, 470, subject.getStat()[0]*100/subject.getLife(), 10), Color.FromArgb(50,255,50));
            Video.Screen.Fill(new Rectangle(140, 470, 100, 10), Color.DarkGray);
            Video.Screen.Fill(new Rectangle(140, 470, subject.getStat()[1] * -1 + 100, 10), Color.FromArgb(255,50,50));
            Video.Screen.Fill(new Rectangle(260, 470, 100, 10), Color.DarkGray);
            Video.Screen.Fill(new Rectangle(260, 470, subject.getStat()[2] * -1 + 100, 10), Color.FromArgb(50, 50, 255));
            Video.Screen.Fill(new Rectangle(380, 470, 100, 10), Color.DarkGray);
            Video.Screen.Fill(new Rectangle(380, 470, subject.getStat()[3] * -1 + 100, 10), Color.FromArgb(50, 50, 150));
            Video.Screen.Fill(new Rectangle(500, 470, 100, 10), Color.DarkGray);
            Video.Screen.Fill(new Rectangle(500, 470, subject.getStat()[4] * -1 + 100, 10), Color.FromArgb(250, 250, 250));
            
        }
    }
}
