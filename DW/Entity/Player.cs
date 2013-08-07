using System;
using System.Drawing;
using System.Threading;

using SdlDotNet.Graphics.Sprites;
using SdlDotNet.Graphics;
using SdlDotNet.Input;




namespace DW
{
    [Serializable]
    public class Player : Entity
    {

        protected int recoveryAmount = 100;
        protected String pclass;
        public Skills skills;
        private int stairId = -1;
        protected Recipe[] recipeList = new Recipe[100];
        protected Item itemInHand;
        protected Item lastItemInHand;
        

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
            life = force * endurance * rand.Next(1, par1name.Length);
            
            pclass = par2class;
            espece = "human";
            regime = "omnivore";
            value = "@";
            originalValue = "@";
            color = Color.Purple;
            lifeTmp = life;
            DW.render.setUI(this);
            skills = new Skills(this);
            learnRecipe(Recipe.RecipeForge);
            learnRecipe(Recipe.RecipeSilice);
            learnRecipe(Recipe.RecipeChaux);
            learnRecipe(Recipe.RecipeGlass);
        }

        public void setItemInHand(Item par1)
        {
            itemInHand = par1;
        }

        public bool itemInHandChanged()
        {
            if (itemInHand != lastItemInHand)
            {
                lastItemInHand = itemInHand;
                return true;
            }
            return false;
        }

        public Item getItemInHand()
        {
            return itemInHand;
        }

        //<summary>
        //retourne la classe du joueur
        //</summary>
        public string getClass()
        {
            return pclass;
        }

        public Recipe[] getRecipes()
        {
            return recipeList;
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
            else if(DW.server != null)
                DW.server.showMsg(par1,this);
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
            timer -= 1;
            if (enduranceTmp < endurance && timer<=0)
            {
                enduranceTmp += 1;
                timer = recoveryAmount;
            }
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

        /**
         * Allow the player to interact with his environment
         */
        public void interact()
        {
                if (itemInHand != null)
                    itemInHand = itemInHand.interact(this);
                else
                {
                    if (getFace() == "front" && getStair().getSpecial()[getX(), getY() + 1] != null)
                        getStair().getSpecial()[getX(), getY() + 1].interact(this);
                    else if (getFace() == "back" && getStair().getSpecial()[getX(), getY() - 1] != null)
                        getStair().getSpecial()[getX(), getY() - 1].interact(this);
                    else if (getFace() == "left" && getStair().getSpecial()[getX() - 1, getY()] != null)
                        getStair().getSpecial()[getX() - 1, getY()].interact(this);
                    else if (getFace() == "right" && getStair().getSpecial()[getX() + 1, getY()] !=null)
                        getStair().getSpecial()[getX() + 1, getY()].interact(this);
                }
        }

        //<summary>
        //Permet au joueur d'apprendre une recette
        //</summary>
        //<param name="par1">Recette à apprendre</param>
        public void learnRecipe(Recipe par1)
        {
            for (int i = 0; i < recipeList.Length; i++)
            {
                if (recipeList[i] == null)
                {
                    recipeList[i] = par1;
                    return;
                }   
            }
        }


        //<summary>
        //fait bouger le joueur (coté serveur seulement)
        //</summary>
        public void move(int par1x, int par2y)
        {
                if (canWalkOn(x + par1x, y + par2y))
                {
                    x = x + par1x;
                    y = y + par2y;
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

