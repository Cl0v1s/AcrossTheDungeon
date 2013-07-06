using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DW
{
    [Serializable]
    class Recipe
    {

        public static Recipe RecipeRefinedGlass = new Recipe("Verre poli", "Du verre parfaitement \nLisse et transparent.", new Item[] { ItemMineral.ItemGlass, ItemMineral.ItemDust }, new Item[] { ItemMineral.ItemRefinedGlass });
        public static Recipe RecipeForge = new Recipe("Forge", "Un kit complet pour monter\nun four fait de pierre chauffe\n grace a de la lave.\n Vous pourrez forger des outils !", new Item[] { ItemMineral.ItemRock, ItemMineral.ItemRock, ItemMineral.ItemRock, ItemMineral.ItemRock, Item.ItemBucketLava }, new Item[] { Item.ItemForge });
        public static Recipe RecipeGlass = new Recipe("Verre Brut", "Du verre grossier, coupant.\nInutilisable en l'état.", new Item[] { Item.ItemChaux, Item.itemSilice }, new Item[] { Item.ItemGlass }, new Forge());
        public static Recipe RecipeSilice = new Recipe("Silice", "En tamisant du sable vous obtiendez\nfacilement de la silice.", new Item[] { Item.ItemBucketSand }, new Item[] { Item.itemSilice, Item.itemSilice,Item.ItemBucket });
        public static Recipe RecipeChaux = new Recipe("Chaux", "Une poudre blanche. Element de base\nde la maconnerie.", new Item[] { Item.ItemBucketSand, ItemMineral.ItemDust }, new Item[] { Item.ItemChaux, Item.ItemBucket }, new Forge());




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
