using System;
using System.Threading;
using System.Text.RegularExpressions;

using SdlDotNet.Input;

namespace DW
{
    class IpMenu
    {
        private TextInput input;
        private Text error;

        public IpMenu()
        {
            input = new TextInput(640 / 2 - 100, 90 + 75, 200, 25, 20, "", false,true);
            error = new Text("pixel.ttf", 20, 640 / 2 - 100-30, 95 + 100, "", 255, 0, 0,TypePos.Center);
            input.activate(true);
            Thread.Sleep(200);
        }

        
        //<summary>
        //affiche l'éditeur à l'écran et gère l'actualisation des composants
        //</summary>
        public void update()
        {
            input.update();
            if (input.getText().Length != 12 || input.getText().Split(((string)".").ToCharArray()).Length != 4)
            {
                error.setPos(640 / 2, 95 + 110);
                error.changeText("Veuillez entrer une adresse ip valide.", 255, 0, 0);
            }
            else
            {
                error.changeText("L'adresse ip saisie est valide.", 0, 255, 0);
                error.setPos(640 / 2, 95 + 110);
            }
            if (DW.input.equals(Key.V))
            {

                if (input.getText().Length != 12 || input.getText().Split(((string)".").ToCharArray()).Length != 4)
                {
                    error.changeText("Veuillez entrer une adresse ip valide.", 255, 0, 0);
                }
                else
                {
                    DW.close(DW.ipMenu);
                    DW.changeScene("EditorMenu", "Client:"+input.getText());
                    return;
                }
            }
            new Text("pixel.ttf", 20, 640/2, 410 - 50, "Appuyez sur V pour valider.",255,255,255,TypePos.Center).update();
            new Text("pixel.ttf", 40, 10, 0, "Connexion").update();
            new Text("pixel.ttf", 20, 10, 40, "Entrez l'adresse Ip locale du serveur auquel vous souhaitez vous connecter.").update();
            new Text("pixel.ttf", 20, 640/2, 210+40, "L'adresse ip locale du serveur auquel vous voulez vous connecter",150,150,150,TypePos.Center).update();
            new Text("pixel.ttf", 20, 640 / 2, 230+40, "s'affiche dans le chat du serveur lors de son lancement.", 150, 150, 150, TypePos.Center).update();
            new Text("pixel.ttf", 20, 640 / 2, 250+40, "Vous pouvez toujours égalemment l'obtenir en lançant l'invite de commande", 150, 150, 150, TypePos.Center).update();
            new Text("pixel.ttf", 20, 640 / 2, 270+40, "Windows et en entrant la commande 'ipconfig'.", 150, 150, 150, TypePos.Center).update();
            error.update();
        }
    }
}
