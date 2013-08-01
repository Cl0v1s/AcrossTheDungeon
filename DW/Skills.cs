using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DW
{
    [Serializable]
    public class Skills
    {
        private Player owner;
        private Random rand = new Random();
        /*[MANUEL]Tableau contenant tout les talents:
         * 0=battle
         * 1=wizardry
         * 2=survival
         * 3=cheating
         * 4=forge*/
        private int[] levels = new int[5];
        /*[MANUEL]Tableau contenant les noms des talents*/
        private string[] levelsName = new string[] 
        { 
            "combat", 
            "magie", 
            "survie", 
            "roublardise", 
            "forge" 
        };
        /*[AUTO]Tableau contenant l'xp par talent, même ordre que levels[]*/
        private float[] levelsTemp;

        public Skills(Player par1)
        {
            owner = par1;
            levelsTemp = new float[levels.Length];
            for (int i = 0; i < levels.Length; i++)
                levels[i] = 1;
            if (owner.getClass() == "Barbare")
                levels[0] = 5;
            else if (owner.getClass() == "Pretre")
                levels[1] = 5;
            else if (owner.getClass() == "Escroc")
                levels[3] = 5;
        }

        //<summary>
        //attente une action avec l'un des talents définis plus haut et le cas present fait evoluer le niveau dudit talent du joueur.
        //</summary>
        //<param name="par1skill">Le nom du talent a utiliser</param>
        //param name="par2amount">Le gain d'xp maximal en cas de reussite</param>
        public bool tryAction(string par1skill, float par2amount)
        {
            bool result=true;
            int index = nameToIndex(par1skill);
            if (index == -1)
                return false;
            int r = rand.Next(0, 100);
            if (r >= levels[index]+((-5/10)*levels[index]+5))
            {
                levelsTemp[index] += par2amount * rand.Next(0, 30) / 100;
                result = false;
            }
            else
            {
                levelsTemp[index] += par2amount * rand.Next(50, 100) / 100;
                if (levelsTemp[index] >= 5 * levels[index] + 30)
                {
                    levelsTemp[index] = 0;
                    levels[index] += 1;
                    owner.showMsg("Vous vous sentez plus a l'aise dans le domaine de la "+levelsName[index]);
                }
            }
            return result;
        }

        //<summary>
        //retourne le niveau du joueur du talent passé en paramètre
        //</summary>
        //<param name="par1">Le nom du talent à retourner</param>
        public int getTalentLevel(string par1)
        {
            int index = nameToIndex(par1);
            if (index == -1)
                return 0;
            else
                return levels[index];

        }

        //<summary>
        //retourne l'index du talent dans le tabelau levels en fonction du nom trouvé dans levelsName
        //</summary>
        //<param name="par1">le nom du talent a "traduire"</param>
        private int nameToIndex(string par1)
        {
            for (int i = 0; i < levelsName.Length; i++)
            {
                if (levelsName[i] == par1)
                {
                    return i;
                }
            }
            Console.WriteLine("Le talent " + par1 + " n'existe pas.");
            return -1;
        }
    }
}
