using System;
using System.Drawing;
using System.Threading;

using SdlDotNet.Graphics.Sprites;
using SdlDotNet.Graphics;
using SdlDotNet.Input;




namespace DW
{
    [Serializable]
    class Player : Entity
    {
        protected String pclass;
        public Skills skills;
        private int stairId = -1;
        

        //<summary>
        //créer et gère le joueur coté serveur et les fonctions communes au joueur coté client
        //</summary>
        public Player(String par1name, String par2class, int par3force, int par4endurance, int par5volonte, int par6agilite)
            : base()
        {
            inventory = new Inventory(this);
            name = par1name;
            force = par3force;
            endurance = par4endurance;
            enduranceTmp = endurance;
            volonte = par5volonte;
            agilite = par6agilite;
            pclass = par2class;
            espece = "human";
            regime = "omnivore";
            value = "@";
            originalValue = "@";
            color = Color.Purple;
            life = force * endurance * rand.Next(1, par1name.Length);
            lifeTmp = life;
            DW.render.setUI(this);
            skills = new Skills(this);
        }

        //<summary>
        //retourne la classe du joueur
        //</summary>
        public string getClass()
        {
            return pclass;
        }


        //<summary>
        //affecte les statistiques du joueur en faisant évoluer le temps (1 tour = 10 min)
        //affecte les statistiques du joueur en fonction de l'environnenment
        //vérifie si le joueur est à prixmité d'un monstre
        //</summary>
        public override void turn()
        {
            tour += 1;
            faim += (float)5 / 432;
            sommeil += (float)5 / 288;
            soif += (float)1 / 18;
            sale += (float)5 / 864;
            if (isburning)
                lifeTmp -= 1;
            EnvironmentEffect();
        }

        //<summary>
        //permet au joueur de boire (seulement coté serveur)
        //</summary>
        public void lap()
        {
            if (isOn(100))
            {
                soif = soif - 25F;
                if (soif < 0)
                    soif = 0;
            }
        }


        //<summary>
        //affecte les statistiques du joueur en fonction de l'environnement
        //</summary>
        protected override void EnvironmentEffect()
        {
            if (isOn(3) == true)
            {
                lifeTmp -= 5;
                showMsg("Vous venez de marcher sur un piège !");
            }
            else if (isOn(101))
            {
                isburning = true;
                showMsg("Vous avez votre pieds dans un lac de lave...");
            }
        }

        //<summary>
        //tranfert le message envoyé au joueur au client ou au serveur selon la configuration de la connexion
        //</summary>
        public override void showMsg(string par1)
        {
            if (DW.client != null)
                DW.client.showMsg(par1);
            else if(DW.dungeon != null)
                DW.dungeon.showMsg(par1,this);
        }


        //<summary>
        //centre la caméra sur le joueur (coté serveur seulement)
        //</summary>
        public void setCanvas()
        {
            DW.render.move(x * -30 + 640 / 2, y * -30 + 480 / 2);
        }


        //<summary>
        //met à jour le joueur à l'écran (coté serveur seulement)
        //</summary>
        public new virtual bool update()
        {
            if (stair != null)
                stair.update();
            DW.render.renderEntityVision(this);
            if (isburning)
            {
                if (frame <= 20)
                    color = Color.FromArgb(150, 50, 50);
                else
                    color = Color.FromArgb(250, 50, 50);
            }
            if (lifeTmp <= 0)
                dead = true;
            return dead; 
        }


        public void interact()
        {
            if (isFighting == false)
            {
                Special[,] s = stair.getSpecial();
                if (s[x, y] != null)
                {
                    s[x, y].interact(this);
                    return;
                }
                else if (s[x - 1, y] != null)
                {
                    s[x - 1, y].interact(this);
                    return;
                }
                else if (s[x + 1, y] != null)
                {
                    s[x + 1, y].interact(this);
                    return;
                }
                else if (s[x, y - 1] != null)
                {
                    s[x, y - 1].interact(this);
                    return;
                }
                else if (s[x, y + 1] != null)
                {
                    s[x, y + 1].interact(this);
                    return;
                }
                
            }
        }


        //<summary>
        //fait bouger le joueur (coté serveur seulement)
        //</summary>
        public void move(int par1x, int par2y)
        {
            Entity[] e = stair.getEntities();
            for (int i = 0; i < e.Length; i++)
            {
                if (e[i] != null && !(e[i] is Player) && isNear(e[i]))
                {
                    fight(this, e[i]);
                    break;
                }
            }
            if (isFighting == false)
            {
                if (canWalkOn(x + par1x, y + par2y))
                {
                    x = x + par1x;
                    y = y + par2y;
                }
            }
            setCanvas();
            turn();
            Thread.Sleep(100);


        }


        public void changeStair(Stair par1stair, int par2id)
        {
            stair = par1stair;
            stairId = par2id;
        }


        public int getStairId()
        {
            return stairId;
        }








    }
}

