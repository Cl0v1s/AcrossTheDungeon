﻿using System;
using System.Drawing;
using System.Threading;

using SdlDotNet.Input;


namespace DW
{
    [Serializable]
    class Player : Entity
    {
        public StatUI statUI;
        protected String pclass;
        public Skills skills;
        private int stairId = -1;

        //<summary>
        //créer et gère le joueur coté serveur et les fonctions communes au joueur coté client
        //</summary>
        public Player(String par1name, String par2class, int par3force, int par4endurance, int par5volonte, int par6agilite)
            : base()
        {
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
            statUI = new StatUI(this);
            skills = new Skills(this);
        }

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

            EnvironmentEffect();
            Entity[] e = stair.getEntities();
            for (int i = 0; i < e.Length; i++)
            {
                if (e[i] != null && !(e[i] is Player) && isNear(e[i]))
                {
                    fight(this, e[i]);
                    break;
                }
            }
        }

        //<summary>
        //réagit en fonction des touches préssées (seulement coté serveur, ceci étant géré par la class Client coté client
        //</summary>
        private void inputUpdate()
        {
            if (DW.input.equals(Key.UpArrow) == true)
            {
                move(0, -1);
            }
            else if (DW.input.equals(Key.DownArrow) == true)
            {
                move(0, 1);
            }
            else if (DW.input.equals(Key.RightArrow) == true)
            {
                move(1, 0);
            }
            else if (DW.input.equals(Key.LeftArrow) == true)
            {
                move(-1, 0);
            }
            else if (DW.input.equals(Key.KeypadEnter))
            {
                interact();
                Thread.Sleep(100);
            }
            else if (DW.input.equals(Key.L))
                lap();
        }

        //<summary>
        //permet au joueur de boire (seulement coté serveur)
        //</summary>
        private void lap()
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
                showMsg("Vous marchez sur un piège !");
            }
        }

        public void showMsg(string par1)
        {
            if (DW.client != null)
                DW.client.showMsg(par1);
            else
                DW.dungeon.showMsg(par1);
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
        public virtual bool update()
        {
            inputUpdate();
            if (stair != null)
                stair.update();
            //Fait avancer le joueur en fonction de la touche choisie
            //Note: L'axe est inversé !


            DW.render.renderEntityVision(this);
            statUI.update();
            if (lifeTmp <= 0)
                dead = true;
            return dead; 
        }

        public void interact()
        {
            if (isFighting == false)
            {
                Special[,] s = stair.getSpecial();
                if (s[x - 1, y] != null)
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
                turn();
                stair.roll();
                
            }
        }

        //<summary>
        //fait bouger le joueur (coté serveur seulement)
        //</summary>
        private void move(int par1x, int par2y)
        {
            if (isFighting == false)
            {
                if (canWalkOn(x + par1x, y + par2y))
                {
                    x = x + par1x;
                    y = y + par2y;
                }
            }
            turn();
            stair.roll();
            setCanvas();

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