using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DW
{
    class AnimationManager
    {
        public Animation[] list = new Animation[50];

        public AnimationManager()
        {
        }

        //<summary>
        //Affiche la totalité des animations en cours à l'écran.
        //</summary>
        public void update()
        {
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i] != null)
                    list[i] = list[i].update();
            }
        }

        //<summary>
        //Ajoute une annimation à la liste des animations courantes.
        //</summary>
        public void addAnimation(Animation par0, int par1x, int par2y, int par3xto,int par4yto)
        {
            Animation a = par0.clone();
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i] == null)
                {
                    list[i] = a;
                    list[i].start(par1x, par2y,par3xto,par4yto);
                    return;
                }
            }
        }


    }
}
