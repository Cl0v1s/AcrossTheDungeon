using System;



namespace DW
{
    [Serializable]
    class ItemStick : Item
    {
        public ItemStick()
        {
            set("Bâton", "Un bâton long et fin...", 4);
        }
    }

    [Serializable]
    class ItemIronIngot : Item
    {
        public ItemIronIngot()
        {
            set("Lingot de fer", "Un lingot de fer.", 10);
        }
    }

    [Serializable]
    class ItemCoal : Item
    {
        public ItemCoal()
        {
            set("Charbon", "Un minerais de charbon. Hé, Marie, on aura de quoi se chauffer ce soir !", 6);
        }
    }

    [Serializable]
    class ItemGlass : Item
    {
        public ItemGlass()
        {
            set("Verre brute", "Il brille de milles feux, mais attention ! Il est coupant...", 2);
        }
    }

    [Serializable]
    class ItemRefinedGlass : Item
    {
        public ItemRefinedGlass()
        {
            set("Verre raffiné", "Fin prêt à l'emploi", 5);
        }
    }

    [Serializable]
    class ItemEmerald : Item
    {
        public ItemEmerald()
        {
            set("Emeraude", "Prête à être incrustée dans un bâton.", 4);
        }
    }

    [Serializable]
    class ItemRuby : Item
    {
        public ItemRuby()
        {
            set("Ruby", "Une pierre rouge écarlate. On dirait un coeur... de pierre...", 4);
        }
    }

    [Serializable]
    class ItemObsidian : Item
    {
        public ItemObsidian()
        {
            set("Obsidienne", "Originellement forgé par les titans, l'obsidienne est maintenant un matériaux noir et utilisé pour tout et par tous.", 4);
        }
    }


    [Serializable]
    class ItemCloviscieux : Item
    {
        public ItemCloviscieux()
        {
            set("Cloviscieux", "(Cloviscium raffiné) Ce matériau est utiles dans la conception des objets tous plus noir les uns que les autres...", 4);
        }
    }

    [Serializable]
    class ItemWood : Item
    {
        public ItemWood()
        {
            set("Bois", "Comme le bûcheron Kébécoué que vous êtes, un tronc de plus ou de moins ne vous fait pas peur.", 4);
        }
    }

    [Serializable]
    class ItemPearl : Item
    {
        public ItemPearl()
        {
            set("Perle", "Une perle ou un poème difficilement accessible, aurait dit Frenciz Ponche", 4);
        }
    }



    [Serializable]
    class ItemLexertium : Item
    {
        public ItemLexertium()
        {
            set("Lexertium", "Utiles pour diverses potions et autres engins de torture: délie toutes les langues !", 4);
        }
    }

    [Serializable]
    class ItemEyebat : Item
    {
        public ItemEyebat()
        {
            set("Oeil de chauve-souris", "Très ragoutant, dirait votre belle mère...", 4);
        }
    }

    [Serializable]
    class ItemPigtail : Item
    {
        public ItemPigtail()
        {
            set("Queue de cochon", "Sa forme et sa couleur ne vous fait rien présager de bon lorsque vous la trouvez dans votre potion ou votre caleçon.", 4);
        }
    }

    [Serializable]
    class ItemKrystal : Item
    {
        public ItemKrystal()
        {
            set("Krystal", "Pierre extrêmement rare. Lorsque vous la tenez, une sensation de malaise vous prend et vous entendez comme un cri lointain qui vous appelle à l'aide.", 4);
        }
    }

    [Serializable]
    class ItemWheed : Item
    {
        public ItemWheed()
        {
            set("Herbe", "Un vers elfique vous revient en mémoire: 'SMOK OUID EVRI DAI !'", 4);
        }
    }

    [Serializable]
    class ItemWool : Item
    {
        public ItemWool()
        {
            set("Laine", "En Avaeryl, ne te découpe pas de fil... dans le pull de ton frère...", 4);
        }
    }

    [Serializable]
    class ItemTenebrion : Item
    {
        public ItemTenebrion()
        {
            set("Tenebrion", "Une herbe qui rampe dans le plus noir des donjons... Qui rampe ?", 4);
        }
    }

    [Serializable]
    class ItemAelion : Item
    {
        public ItemAelion()
        {
            set("Aelion", "Obtenu en chauffant très fort du tenebrion... Il me semble l'avoir entendu crier...", 4);
        }
    }

    [Serializable]
    class ItemMint : Item
    {
        public ItemMint()
        {
            set("Menthe", "Lorsque vous l'écrasez sous votre pied, elle embaume la salle.", 4);
        }
    }

    [Serializable]
    class ItemRock : Item
    {
        public ItemRock()
        {
            set("Roche", "Un banal cailloux qui roule au creux de votre main... ROCK'N ROLL !", 4);
        }
    }

    [Serializable]
    class ItemDust : Item
    {
        public ItemDust()
        {
            set("Poussière fine", "Le cailloux à du tomber... en poussière...", 4);
        }
    }

    [Serializable]
    class ItemIvoire : Item
    {
        public ItemIvoire()
        {
            set("Ivoire", "Cet ivoire provient peut être d'une défense de mammouth? Ou des dents d'un aventurier...", 4);
        }
    }

    [Serializable]
    class ItemRock_Polis : Item
    {
        public ItemRock_Polis()
        {
            set("Roche polis", "'Lorsqu'un cailloux devient une sphère, il ne se transforme pas nécessairement en planète' Guide du voyageur intratemporel", 4);
        }
    }

    [Serializable]
    class ItemPickAxe : Item
    {
        public ItemPickAxe()
        {
            set("Pioche", "What's your's is Mine.Craft.", 4);
        }
    }






















    [Serializable]
    class Item
    {
        private string name;
        private string description;
        private string action;
        private int price;

        //<summary>
        //créer un nouvel item
        //</summary>
        //<param name="par1name">nom de l'objet</param>
        //<param name="par2desc">descritpion de l'objet</param>
        //<param name="par3price">prix de vente de l'objet</param>
        public Item(string par1name, string par2desc, int par3price,string par4action=null)
        {
            name = par1name;
            description = par2desc;
            price = par3price;
            action = par4action;

        }

        public Item()
        {

        }

        protected virtual void set(string par1name, string par2desc, int par3price,string par4action=null)
        {
            name = par1name;
            description = par2desc;
            price = par3price;
            action = par4action;
        }

        //<summary>
        //execute la fonction de l'objet
        //</summary>
        //<param name="par1">entité à affecter</param>
        public virtual Item interact(Entity par1)
        {
            return this;
        }

        public string getName()
        {
            return name;
        }

        public string getDescription()
        {
            return description;
        }

        public string getAction()
        {
            return action;
        }

        public virtual Item clone()
        {
            return (Item)this.MemberwiseClone();
        }

    }
}
