using System;
using System.Drawing;

using SdlDotNet.Core;
using SdlDotNet.Graphics;

namespace DW
{
    //<summary>
    //affiche et gère le menu principal
    //</summary>
    class GameMenu
    {

        private MenuUI menu;
        private Text title;
        private Text error;

        public GameMenu(string par1=null)
        {
            menu = new MenuUI(new String[] { "Nouvelle marche", "Rejoindre une marche","Quitter" }, 320, 360,200,40);
            title = new Text("pixel.ttf", 60, 640/2, 50, "Dungeon Walker", 230, 230, 230,TypePos.Center);
            error = new Text("pixel.ttf", 20, 640 / 2, 90 + 100, "", 255, 0, 0,TypePos.Center);
            if (par1 != null)
            {
                error.changeText(par1, 255, 0, 0);
                error.setPos(640 / 2, 90 + 100);
            }
        }

        //<summary>
        //met à jour l'interface et vérifie le retour du menu de séléction
        //</summary>
        public void update()
        {
            title.update();
            error.update();
            int index = menu.update();
            if (index == 0)
            {
                DW.close(DW.gameMenu);
                DW.changeScene("EditorMenu", "Server");
            }
            else if (index == 1)
            {
                DW.close(DW.gameMenu);
                DW.changeScene("EditorMenu","IpMenu");

            }
            else if (index == 2)
                DW.endProgramm();
        }
    }
}
