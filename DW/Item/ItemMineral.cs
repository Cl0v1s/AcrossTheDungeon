using System;

namespace DW
{
/*
    [Serializable]
    class ItemCoal : Item
    {
        public ItemCoal()
        {
            set("Charbon", "Un minerais de charbon. \nHé, Marie, on aura de quoi se chauffer ce soir !", 6);
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
    }*/





    [Serializable]
    class ItemMineral : Item
    {
        public static ItemMineral ItemMineralIron=new ItemMineral("Minerais de fer", "Un minerais de fer trouvé dans une cavité.\n Il y aura de quoi fer !", 6);
        public static ItemMineral ItemMineralObsidian=new ItemMineral("Minerais d'obsidienne", "'Raffines-moi',\n semble dire cette pierre noire comme ce donjon...", 4);

        public ItemMineral(string par1name, string par2desc, int par3price,string par4action=null): base(par1name,par2desc,par3price,par4action=null)
        {

        }

    }
}
