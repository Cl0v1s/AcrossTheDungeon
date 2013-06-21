using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DW
{
    [Serializable]
    class Berry : Item
    {
        public Berry()
        {
            set("baie", "Une petite baie acide cueillie sur une plante du donjon.", 2);
        }
    }

    class Stick : Item
    {
        public Stick()
        {
            set("Bâton", "Un bâton long et fin...", 4);
        }
    }
    class IronIngot : Item
    {
        public IronIngot()
        {
            set("Lingot de fer", "Un lingot de fer.", 10);
        }
    }
    class IronMineral : Item
    {
        public IronMineral()
        {
            set("Minerais de fer", "Un minerais de fer trouvé dans une cavité. Il y aura de quoi fer !", 6);
        }
    }
    class Coal : Item
    {
        public Coal()
        {
            set("Charbon", "Un minerais de charbon. Hé, Marie, on aura de quoi se chauffer ce soir !", 6);
        }
    }
    class Glass : Item
    {
        public Glass()
        {
            set("Verre brute", "Il brille de milles feux, mais attention ! Il esxt coupant...", 2);
        }
    }
    class RefinedGlass : Item
    {
        public RefinedGlass()
        {
            set("Verre raffiné", "Fin prêt à l'emploi", 5);
        }
    }
    class Emerald : Item
    {
        public Emerald()
        {
            set("Emeraude", "Prête à être incrustée dans un bâton.", 4);
        }
    }
    class Ruby : Item
    {
        public Ruby()
        {
            set("Ruby", "Une pierre rouge écarlate. On dirait un coeur... de pierre...", 4);
        }
    }
    class Obsidian : Item
    {
        public Obsidian()
        {
            set("Obsidienne", "Originellement forgé par les titans, l'obsidienne est maintenant un matériaux noir et utilisé pour tout et par tous.", 4);
        }
    }
    class ObsidianMineral : Item
    {
        public ObsidianMineral()
        {
            set("Minerais d'obsidienne", "'Raffines-moi', semble dire cette pierre noire comme ce donjon...", 4);
        }
    }
    class Cloviscium : Item
    {
        public Cloviscium()
        {
            set("Minerais de Cloviscium", "Ce minerais à été nommé en hommage au Dieu du donjon ténébreux.", 4);
        }
    }
    class Cloviscieux : Item
    {
        public Cloviscieux()
        {
            set("Cloviscieux", "(Cloviscium raffiné) Ce matériau est utiles dans la conception des objets tous plus noir les uns que les autres...", 4);
        }
    }
    class Wood : Item
    {
        public Wood()
        {
            set("Bois", "Comme le bûcheron Kébécoué que vous êtes, un tronc de plus ou de moins ne vous fait pas peur.", 4);
        }
    }
    class Pearl : Item
    {
        public Pearl()
        {
            set("Perle", "Une perle ou un poème difficilement accessible, aurait dit Frenciz Ponche", 4);
        }
    }
    class LexertiumMineral : Item
    {
        public LexertiumMineral()
        {
            set("Minerais de Lexertium", "Ce minerais à des propriétés urticantes fort malvenues.", 4);
        }
    }
    class Lexertium : Item
    {
        public Lexertium()
        {
            set("Lexertium", "Utiles pour diverses potions et autres engins de torture: délie toutes les langues !", 4);
        }
    }
    class Eyebat : Item
    {
        public Eyebat()
        {
            set("Oeil de chauve-souris", "Très ragoutant, dirait ma belle mère...", 4);
        }
    }
    class Pigtail : Item
    {
        public Pigtail()
        {
            set("Queue de cochon", "Sa forme et sa couleur ne vous fait rien présager de bon lorsque vous la trouvez dans votre potion.", 4);
        }
    }
    class Krystal : Item
    {
        public Krystal()
        {
            set("Krystal", "Pierre extrêmement rare. Lorsque vous la tenez, une sensation de malaise vous prend et vous entendez comme un cri lointain qui vous appelle à l'aide.", 4);
        }
    }
    class Wheed : Item
    {
        public Wheed()
        {
            set("Herbe", "Un vers elfique vous revient en mémoire: 'SMOK OUID EVRI DAI !'", 4);
        }
    }
    class Wool : Item
    {
        public Wool()
        {
            set("Laine", "En Avaeryl, ne te découpe pas de fil... dans le pull de ton frère...", 4);
        }
    }
    class Tenebrion : Item
    {
        public Tenebrion()
        {
            set("Tenebrion", "Une herbe qui rampe dans le plus noir des donjons... Qui rampe ?", 4);
        }
    }
    class Aelion : Item
    {
        public Aelion()
        {
            set("Aelion", "Obtenu en chauffant très fort du tenebrion... Il me semble l'avoir entendu crier...", 4);
        }
    }
    class Mint : Item
    {
        public Mint()
        {
            set("Menthe", "Lorsque vous l'écrasez sous votre pied, elle embaume la salle.", 4);
        }
    }
    class Rock : Item
    {
        public Rock()
        {
            set("Roche", "Un banal cailloux qui roule au creux de votre main... ROCK'N ROLL !", 4);
        }
    }
    class Dust : Item
    {
        public Dust()
        {
            set("Poussière fine", "Le cailloux à du tomber... en poussière...", 4);
        }
    }
    class Ivoire : Item
    {
        public Ivoire()
        {
            set("Ivoire", "Cet ivoire provient peut être d'une défense de mammouth? Ou des dents d'un aventurier...", 4);
        }
    }
    class Rock_Polis : Item
    {
        public Rock_Polis()
        {
            set("Roche polis", "'Lorsqu'un cailloux devient une sphère, il ne se transforme pas nécessairement en planète' Guide du voyageur intratemporel", 4);
        }
    }
}
