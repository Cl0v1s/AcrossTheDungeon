using System;

namespace DW
{
    [Serializable]
    class ItemMineralIron : ItemMineral
    {
        public ItemMineralIron()
        {
            set("Minerais de fer", "Un minerais de fer trouvé dans une cavité. Il y aura de quoi fer !", 6);
        }
    }


    [Serializable]
    class ItemMineralObsidian : ItemMineral
    {
        public ItemMineralObsidian()
        {
            set("Minerais d'obsidienne", "'Raffines-moi', semble dire cette pierre noire comme ce donjon...", 4);
        }
    }


    [Serializable]
    class ItemMineralLexertium : ItemMineral
    {
        public ItemMineralLexertium()
        {
            set("Minerais de Lexertium", "Ce minerais à des propriétés urticantes fort malvenues.", 4);
        }
    }

    [Serializable]
    class ItemMineralCloviscium : ItemMineral
    {
        public ItemMineralCloviscium()
        {
            set("Minerais de Cloviscium", "Ce minerais à été nommé en hommage au Dieu du donjon ténébreux.", 4);
        }
    }





    [Serializable]
    class ItemMineral : Item
    {
    }
}
