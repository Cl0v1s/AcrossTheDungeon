using System;
using System.IO;

using SdlDotNet.Core;
using SdlDotNet.Graphics;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace DW
{
    class DW
    {

        public static Input input = new Input();
        public static Render render = new Render(0, 0);
        public static EditorMenu editorMenu;
        public static GameMenu gameMenu = new GameMenu();
        public static Server dungeon;
        public static Client client;
        public static String Scene;
        public static Player player;

        /*
         * Point d'entré du compileur
         */
        public static void Main(string[] args)
        {
            DW App = new DW();
        }

        /*
         * Constructeur appelé lors du lancement du programme
         */
        public DW()
        {
            Scene = "GameMenu";
            Video.WindowCaption = "Dungeon Walker";
            //on créer la sortie vidéo
            Video.SetVideoMode(640, 480, false);
            //on ajoute un evenement lors du clique sur fermer
            Events.Quit += new EventHandler<QuitEventArgs>(DW.endProgramm);
            //on actualise à chaque tick
            Events.Tick += new EventHandler<TickEventArgs>(this.update);
            //on affiche la fenetre 
            Events.Run();

        }

        /*
         * Destructeur appelé lors de la fermeture du jeu
         */
        ~DW()
        {

        }

        /*
         * Actualisation du programme et de la sortie vidéo
         */
        private void update(object sender, TickEventArgs e)
        {
            //on efface l'écran
            Video.Screen.Fill(Color.Black);
            render.update();
            if (Scene == "GameMenu")
                gameMenu.update();
            else if (Scene == "EditorMenu")
                editorMenu.update();
            else if (Scene == "Server")
                dungeon.update();
            else if (Scene == "Client")
                client.update();
            //on met à jour l'écran
            Video.Screen.Update();
        }

        public static void changeScene(String par1, string par2 = null)
        {

            if (par1 == "Server")
                dungeon = new Server(true);
            else if (par1 == "EditorMenu" && par2 != null)
                editorMenu = new EditorMenu(par2);
            else if (par1 == "Client")
                client = new Client("192.168.1.19");
            Scene = par1;
        }

        public static void close(object o)
        {
            o = null;
        }

        /*
         * Fermeture de la fenetre
         */
        public static void endProgramm(object sender, QuitEventArgs e)
        {
            //ferme la fenetre
            Events.QuitApplication();
        }

        public static void endProgramm()
        {
            //ferme la fenetre
            Events.QuitApplication();
        }

        public static void setPlayer(Player par1)
        {
            player = par1;
        }

        public static byte[] objectToByte(object par1)
        {
            if (par1 == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, par1);
            ms.Seek(0, SeekOrigin.Begin);
            return ms.ToArray();
        }

        public static object byteToObject(byte[] _ByteArray)
        {
            try
            {
                // convert byte array to memory stream
                System.IO.MemoryStream _MemoryStream = new System.IO.MemoryStream(_ByteArray);

                // create new BinaryFormatter
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter _BinaryFormatter
                            = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                // set memory stream position to starting point
                _MemoryStream.Position = 0;

                // Deserializes a stream into an object graph and return as a object.
                return _BinaryFormatter.Deserialize(_MemoryStream);
            }
            catch (Exception _Exception)
            {
                // Error
                Console.WriteLine(_Exception.ToString());
            }

            // Error occured, return null
            return null;
        }

    }

}