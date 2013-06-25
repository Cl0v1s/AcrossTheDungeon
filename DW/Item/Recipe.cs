using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DW
{
    [Serializable]
    class Recipe
    {

        public static Recipe RecipeIronIngot = new Recipe("Verre poli", "Du verre parfaitement \nLisse et transparent.", new Item[] { ItemMineral.ItemGlass, ItemMineral.ItemDust }, new Item[] { ItemMineral.ItemRefinedGlass });







        private string name;
        private string description;
        private Item[] needs;
        private Item[] results;
        private Special tool;

        public Recipe(string par1name, string par2description, Item[] par3items, Item[] par4result,Special par5tool=null)
        {
            name = par1name;
            description = par2description;
            needs = par3items;
            results = par4result;
            tool=par5tool;
        }

        public string getName()
        {
            return name;
        }

        public string getDescription()
        {
            return description;
        }

        public string getNeeds()
        {
            string s = "";
            for (int i = 0; i < needs.Length; i++)
            {
                if(needs[i] != null)
                    s = s + needs[i].getName() + ";";
            }
            return s;
        }

        public string getResults()
        {
            string s = "";
            for (int i = 0; i < results.Length; i++)
            {
                if (results[i] != null)
                    s = s + needs[i].getName() +";";
            }
            return s;
        }

        public Item[] getItemNeeds()
        {
            return needs;
        }

        public Item[] getItemResults()
        {
            return results;
        }

        public Special getTool()
        {
            return tool;
        }

    }
}
