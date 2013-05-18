using System;
using System.Threading;
using System.Text.RegularExpressions;

using SdlDotNet.Input;

namespace DW
{
    class IpMenu
    {
        private string ip;
        private TextInput input;
        private Text error;

        public IpMenu()
        {
            input = new TextInput(640 / 2 - 100, 90 + 75, 200, 25, 20, "", false,true);
            error = new Text("pixel.ttf", 20, 640 / 2 - 100-30, 90 + 100, "", 255, 0, 0);
            input.activate(true);
            Thread.Sleep(200);
        }

        
        //<summary>
        //affiche l'éditeur à l'écran et gère l'actualisation des composants
        //</summary>
        public void update()
        {
            input.update();
            if (DW.input.equals(Key.V))
            {
                if (Regex.Matches(input.getText(), ".", RegexOptions.IgnoreCase).Count!=12 && Regex.Matches(input.getText(),(string)".").Count==3)
                    error.changeText("Veuillez entrer une adresse ip valide.", 255, 0, 0);
                else
                DW.changeScene("Client", input.getText());
            }
            new Text("pixel.ttf", 20, 220, 410 - 150, "Appuyez sur V pour valider.").update();
            new Text("pixel.ttf", 40, 10, 0, "Connexion").update();
            new Text("pixel.ttf", 20, 10, 40, "Entrez l'adresse Ip local du serveur auquel vous souhaitez vous connecter.").update();
            error.update();
        }
    }
}
