using System;

using SdlDotNet.Core;
using SdlDotNet.Input;

namespace DW
{
    //<summary>
    //affiche et gère l'éditeur de personnage
    //</summary>
    class EditorMenu
    {
        private int points = 10;

        private int endurance;
        private int force;
        private int volonte;
        private int agilite;
        private int enduranceB;
        private int forceB;
        private int volonteB;
        private int agiliteB;

        private int index = 0;
        private string callback;

        private String race = "Barbare";
        private String[] raceList = new String[] { "Barbare", "Escroc", "Pretre" };
        private MenuUI raceUI;
        private MenuUI statUI;
        private TextInput nameIN;

        public EditorMenu(string par1)
        {
            callback = par1;
            raceUI = new MenuUI(raceList, 640 / 2 - 100, 200 + 50, 100, 100);
            raceUI.activate(false);
            statUI = new MenuUI(new String[] { "Force:" + force + "+" + forceB, "Endurance:" + endurance + "+" + enduranceB, "Volonte:" + volonte + "+" + volonteB, "Agilite:" + agilite + "+" + agiliteB, "Reset" }, 640 / 2, 200 + 50, 100, 100, 20, false);
            statUI.activate(false);
            nameIN = new TextInput(640 / 2 - 100, 90 + 50, 200, 25, 20, "", false);
            nameIN.activate(true);
            Events.KeyboardDown += new EventHandler<KeyboardEventArgs>(this.examine);
        }

        //<summary>
        //examine l'event appuie de touche et agit en conséquence
        //</summary>
        public void examine(object send, KeyboardEventArgs e)
        {
            if (e.Key == Key.LeftArrow)
                index -= 1;
            else if (e.Key == Key.RightArrow)
                index += 1;

            if (index == 0)
            {
                raceUI.activate(false);
                statUI.activate(false);
                nameIN.activate(true);
            }
            if (index == 1)
            {
                raceUI.activate(true);
                statUI.activate(false);
                nameIN.activate(false);
            }
            else if (index == 2)
            {
                raceUI.activate(false);
                statUI.activate(true);
                nameIN.activate(false);
            }

            if (index > 2)
                index = 0;

            if (index < 0)
                index = 2;

            if (e.Key == Key.V && index != 0)
            {
                Events.KeyboardDown -= new EventHandler<KeyboardEventArgs>(this.examine);
                DW.close(DW.editorMenu);
                if (callback == "Server")
                {
                    DW.setPlayer(new Player(nameIN.getText(), raceUI.getCurrentSelectionText(), force + forceB, endurance + enduranceB, volonte + volonteB, agilite + agiliteB));
                    DW.changeScene(callback);
                }
                else if (callback.Contains(":"))
                {
                    DW.setPlayer(new OtherPlayer(nameIN.getText(), raceUI.getCurrentSelectionText(), force + forceB, endurance + enduranceB, volonte + volonteB, agilite + agiliteB));
                    string[] s = callback.Split(":".ToCharArray());
                    DW.changeScene(s[0],s[1]);
                }
                
            }

        }

        //<summary>
        //affiche l'éditeur à l'écran et gère l'actualisation des composants
        //</summary>
        public void update()
        {
            nameIN.update();
            int raceValue = raceUI.update();
            if (raceValue != -1)
                race = raceList[raceValue];
            if (race == raceList[0])
            {
                endurance = 5;
                force = 10;
                volonte = 7;
                agilite = 0;
            }
            else if (race == raceList[1])
            {
                endurance = 1;
                force = 5;
                volonte = 7;
                agilite = 10;
            }
            else if (race == raceList[2])
            {
                endurance = 5;
                force = 1;
                volonte = 10;
                agilite = 7;
            }
            new Text("pixel.ttf", 20, 640 / 2 - 120, 120 + 50, "Force:" + (force + forceB) + " Endurance:" + (endurance + enduranceB) + " volonte:" + (volonte + volonteB) + " agilite:" + (agilite + agiliteB), 0, 255, 0).update();
            int statValue = statUI.update();
            if (points >= 1)
            {
                if (statValue != -1)
                    points -= 1;
                if (statValue == 0)
                    forceB += 1;
                else if (statValue == 1)
                    enduranceB += 1;
                else if (statValue == 2)
                    volonteB += 1;
                else if (statValue == 3)
                    agiliteB += 1;
            }
            if (statValue == 4)
            {
                forceB = 0;
                enduranceB = 0;
                volonteB = 0;
                agiliteB = 0;
                points = 10;
            }
            statUI.changeText(new String[] { "Force:" + force + "+" + forceB, "Endurance:" + endurance + "+" + enduranceB, "Volonte:" + volonte + "+" + volonteB, "Agilite:" + agilite + "+" + agiliteB, "Reset" });
            new Text("pixel.ttf", 20, 640 / 2 - 100, 150 + 50, "Points de capacité restant:" + points, 150, 150, 150).update();
            new Text("pixel.ttf", 20, 640 / 2 - 100, 60 + 50, "Nom du personnage:", 255, 255, 255).update();
            new Text("pixel.ttf", 20, 30, 410 - 50, "Appuyez sur V pour valider/Appuyez sur les fleches dir. pour changer d'entrée.").update();
            new Text("pixel.ttf", 40, 10, 0, "Edition du personnage").update();
            new Text("pixel.ttf", 20, 10, 40, "Veuillez nommer votre personnage, choisir sa specialite et affecter vos points bonus.").update();
        }
    }
}