using System;

using System.Drawing;

namespace DW
{
    [Serializable]
    public class Entity : Special
    {

        protected Random rand = new Random();
        protected int frame = 0;
        protected int tour = 0;
        protected bool dead = false;

        protected String name;
        protected String gender;
        protected String regime;
        protected String espece;

        protected int life;
        protected int lifeTmp;
        public int force;
        public int endurance;
        public double enduranceTmp;
        public int volonte;
        public int agilite;
        protected int range = 5;

        protected float faim = 0;
        protected float soif = 0;
        protected float sommeil = 0;
        protected float sale = 0;
        protected double peur = 0;
        protected int timer = 40;
        protected int speed = 40;
        protected Inventory inventory;

        protected Entity[] others;
        protected Entity worstEnemy = null;
        protected Entity bestFriend = null;
        protected Point objective = new Point(-1, -1);
        protected bool isSleeping = false;
        protected int sleepingTime = 0;
        protected bool isburning = false;
        protected int attempt = 0;
        public bool WantFight = false;


        public Entity(String par1name, int par3force, int par4endurance, int par5volonte, int par6agilite, Stair par7stair)
            : base(par7stair)
        {
            inventory = new Inventory(this);
            name = par1name;
            force = par3force;
            life = force * endurance + rand.Next(1, par1name.Length);
            lifeTmp = life;
            endurance = par4endurance;
            enduranceTmp = (double)endurance;
            volonte = par5volonte;
            agilite = par6agilite;
            regime = "carnivore";
            value = "E";
            originalValue = value;
            turn();
        }

        public void setSale(int par1)
        {
            sale = par1;
        }

        public Entity(Stair par1)
            : base(par1)
        {
        }

        public Entity()
        {
            lifeTmp = life;
            enduranceTmp = endurance;
            originalValue = value;
            turn();
        }

        public virtual void showMsg(string par1)
        {
        }

        public void setStair(Stair par1)
        {
            stair = par1;
        }

        public void time()
        {
            timer -= 1;
            if (timer <= 0)
            {
                turn();
                timer = speed;
            }
        }

        public virtual bool update(Entity[] par1)
        {
            others = par1;
            frame += 1;
            if (isSleeping == true)
            {
                if (frame <= 20)
                    value = "Z";
                else
                    value = originalValue;
                if (frame >= 40)
                    frame = 0;
            }
            if (isburning)
            {
                if (frame <= 20)
                    color = Color.FromArgb(150, 50, 50);
                else
                    color = Color.FromArgb(250, 50, 50);
            }
            if (lifeTmp <= 0)
            {
                dead = true;
                Console.WriteLine(this.getName() + " is dead");
            }
            return dead;
        }

        public virtual void turn()
        {
            if (stair != null)
            {
                tour += 1;
                faim += (float)5 / 432;
                sommeil += (float)5 / 288;
                soif += (float)1 / 18;
                sale += (float)5 / 864;
                if (isburning)
                    lifeTmp -= 1;
                if (enduranceTmp <= endurance - 0.5)
                    enduranceTmp += 0.5;
                if (isSleeping == false)
                {
                    if (worstEnemy != null && isNear(worstEnemy) == false)
                        WantFight = false;
                    if (WantFight == false)
                    {
                        survivalIA();
                        choiceIA();
                    }
                    EnvironmentEffect();

                }
                else if (tour >= sleepingTime)
                {
                    isSleeping = false;
                    sleepingTime = 0;
                }
            }
        }

        protected void survivalIA()
        {
            lookForOther();
            if (faim >= 75 - rand.Next(0, 15))
            {
                lookForFood();
                return;
            }
            else if (soif >= 75 - rand.Next(0, 15) || sale >= 75 - rand.Next(0, 15))
            {
                defineObjectiveFor(100);
                return;
            }
            else if (sommeil >= 75 - rand.Next(0, 15))
            {
                sleep();
                return;
            }
        }

        //<summary>
        //retourne la force présumé de l'entité
        //</summary>
        protected double getPower()
        {
            return (double)((force*(enduranceTmp*100/endurance)/100)/ (faim + soif + sommeil+1));
        }

        //<summary>
        //l'entité active cherche d'autres entitées et interagit avec elles en fonction de son taux de peur
        //peur=0 : indiférence
        //peur<0 : agressivité
        //peur>0 : fuite
        //</summary>
        protected void lookForOther()
        {
            if (peur != 0)
            {
                double a = -1;
                double f = -1;
                if (others != null)
                {
                    for (int i = 0; i < others.Length; i++)
                    {
                        if (others[i] != null && canSee(others[i].getX(), others[i].getY()) == true)
                        {
                            if (peur < 0 && others[i].getSpecies() != getSpecies())
                            {
                                if (a == -1 || a < others[i].getPower())
                                {
                                    a = others[i].getPower();
                                    worstEnemy = others[i];
                                }
                            }
                            else if (others[i].getSpecies() == getSpecies())
                            {
                                if (f == -1 || f < others[i].getPower())
                                {
                                    f = others[i].getPower();
                                    bestFriend = others[i];
                                }
                            }
                        }
                    }
                    if (worstEnemy != null && worstEnemy.getPower() > getPower())
                    {
                        peur = peur + (worstEnemy.getPower()-getPower())/10;
                        Console.WriteLine("peur : " + peur);
                    }
                }
            }
        }

        protected void choiceIA()
        {
            if (worstEnemy != null)
            {
                if (peur > 0 && canSee(worstEnemy.getX(), worstEnemy.getY()))
                {
                    escapeFrom(worstEnemy.getX(), worstEnemy.getY());
                    return;
                }
                else if (canSee(worstEnemy.getX(), worstEnemy.getY()))
                {
                    moveTo(worstEnemy.getX(), worstEnemy.getY());
                    return;
                }
            }
            if (objective.X != -1 && objective.Y != -1)
            {
                attempt += 1;
                if (attempt >= 10)
                {
                    attempt = 0;
                    objective = new Point(-1, -1);
                    return;
                }
                moveTo(objective.X, objective.Y);
                return;
            }
            else if (bestFriend != null && canSee(bestFriend.getX(), bestFriend.getY()) && peur>0)
            {
                moveTo(bestFriend.getX(), bestFriend.getY());
                return;
            }

            Point p = stair.getFreeSpecialCase();
            objective = p;
            moveTo(objective.X, objective.Y);
            return;


        }

        protected void moveTo(int par1x, int par2y)
        {
            int poids = -1;
            Point goodnode = new Point();
            Point[] nodes = new Point[4];
            if (x - 1 <= stair.getW() && x - 1 >= 0 && y <= stair.getH() && y >= 0)
                nodes[0] = new Point(x - 1, y);
            else
                nodes[0] = new Point(-1, -1);
            if (x + 1 <= stair.getW() && x + 1 >= 0 && y <= stair.getH() && y >= 0)
                nodes[1] = new Point(x + 1, y);
            else
                nodes[1] = new Point(-1, -1);
            if (x <= stair.getW() && x >= 0 && y - 1 <= stair.getH() && y - 1 >= 0)
                nodes[2] = new Point(x, y - 1);
            else
                nodes[2] = new Point(-1, -1);
            if (x <= stair.getW() && x >= 0 && y + 1 <= stair.getH() && y + 1 >= 0)
                nodes[3] = new Point(x, y + 1);
            else
                nodes[3] = new Point(-1, -1);
            for (int p = 0; p < 4; p++)
            {
                if (nodes[p].X != -1 && nodes[p].Y != -1)
                {
                    Point node = nodes[p];
                    int h = Math.Abs(par1x - node.X) + Math.Abs(par2y - node.Y);
                    if ((poids == -1 || h < poids) && canWalkOn(node.X, node.Y))
                    {
                        poids = h;
                        goodnode = node;
                    }
                }
            }
            if (nodes[0].Equals(goodnode))
                face = "left";
            else if (nodes[1].Equals(goodnode))
                face = "right";
            else if (nodes[2].Equals(goodnode))
                face = "back";
            else if (nodes[3].Equals(goodnode))
                face = "front";
            x = goodnode.X;
            y = goodnode.Y;
            if (x == objective.X && y == objective.Y)
                objective = new Point(-1, -1);
        }

        protected void escapeFrom(int par1x, int par2y)
        {
            if (par1x > x && stair.getMap()[x - 1, y] == 1)
                x -= 1;
            else if (par1x < x && stair.getMap()[x + 1, y] == 1)
                x += 1;
            else if (par2y > y && stair.getMap()[x, y - 1] == 1)
                y -= 1;
            else if (par2y < y && stair.getMap()[x, y + 1] == 1)
                y += 1;
        }

        protected virtual void EnvironmentEffect()
        {
            if(isNear(worstEnemy))
            {
                fight(worstEnemy);
            }
            if (isNear(4) == true && regime != "carnivore")
                faim = 0;
            else if (isNear(100) == true)
                soif = 0;
            if (isOn(3) == true)
                lifeTmp -= 5;
            else if (isOn(101))
                isburning = true;

        }

        public void setLife(int par1)
        {
            lifeTmp = par1;
        }

        //<summary>
        //inflige des degats à l'unité attaquée et paramètre son comportement en cas de danger de mort
        //</summary>
        //<param name="par2victim">l'entité victime de l'attaque</param>
        public virtual void fight(Entity par1victim)
        {
            if (peur == 0)
                peur = -5;
            setEnemy(par1victim);
            par1victim.setEnemy(this);
            lookTo(par1victim);
            par1victim.lookTo(this);
            WantFight = true;
            par1victim.WantFight = true;
            int atk=(int)(force*(enduranceTmp*100/endurance)/100);
            double cc = rand.NextDouble();
            enduranceTmp -= atk * cc * enduranceTmp / 100;
            cc=rand.NextDouble();
            if (cc <= 1 / 280 * agilite)
                atk = (int)(atk * (1 + cc));
            atk = atk * (1 - (par1victim.getAgilite() / 100));
            par1victim.setLife(par1victim.getStat()[0] - atk);
            if (par1victim.getStat()[0] <= 10 * par1victim.getLife() / 100)
            {
                par1victim.WantFight = false;
                WantFight = false;
                par1victim.setFear(10);
            }
            
        }

        public void setFear(int par1)
        {
            peur = par1;
        }

        public int getAgilite()
        {
            return agilite;
        }

        public string getName()
        {
            return name;
        }

        public void lookTo(Entity par1)
        {
            lookTo(par1.getX(), par1.getY());
        }

        public void lookTo(int par1x, int par2y)
        {
            if (x < par1x)
                setFace("right");
            else if (x > par1x)
                setFace("left");
            else if (y < par2y)
                setFace("front");
            else
                setFace("back");

        }

        public void setEnemy(Entity par1)
        {
            worstEnemy = par1;
        }

        protected bool isOn(int par1)
        {
            if (stair != null && stair.getMap()[x, y] == par1)
                return true;
            return false;
        }

        public bool isNear(Special par1)
        {
            try
            {
                if (stair != null && par1 != null)
                {
                    if (stair.getSpecial()[x + 1, y].GetType() == par1.GetType() || stair.getSpecial()[x - 1, y].GetType() == par1.GetType() || stair.getSpecial()[x, y + 1].GetType() == par1.GetType() || stair.getSpecial()[x, y - 1].GetType() == par1.GetType())
                        return true;
                }
            }
            catch (Exception) { }
            return false;

        }

        public bool isNear(Entity par1)
        {
            try
            {
                if (par1 != null)
                {
                    if (par1.getX() == getX() - 1 && par1.getY() == getY() || par1.getX() == getX() + 1 && par1.getY() == getY() || par1.getY() == getY() - 1 && par1.getX() == getX() || par1.getY() == getY() + 1 && par1.getX() == getX() || par1.getX() == getX() && par1.getY() == getY())
                        return true;
                }
            }
            catch (Exception) { }
            return false;

        }

        public bool isNear(int par1)
        {
            try
            {
                if (stair != null)
                {
                    if (stair.getMap()[x + 1, y] == par1 || stair.getMap()[x - 1, y] == par1 || stair.getMap()[x, y + 1] == par1 || stair.getMap()[x, y - 1] == par1 || stair.getMap()[x, y] == par1)
                        return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }

        protected void lookForFood()
        {
            if (regime == "carnivore")
            {
                Entity[] e = stair.getEntities();
                int poids = -1;
                for (int i = 0; i < e.Length; i++)
                {
                    if (e[i] != null)
                    {
                        int h = Math.Abs(x - e[i].getX()) + Math.Abs(y - e[i].getY());
                        if ((h < poids || poids == -1) && e[i].getSpecies() != getSpecies() && canSee(e[i].getX(), e[i].getY()) == true)
                        {
                            poids = h;
                            worstEnemy = e[i];
                        }
                    }
                }
            }
            else
                defineObjectiveFor(6);
        }

        protected void defineObjectiveFor(int par1case)
        {
            for (int i = (int)(x - range / 2); i < (int)(x + range / 2); i++)
            {
                for (int u = (int)(y - range / 2); u < (int)(y + range / 2); u++)
                {
                    try
                    {
                        if (stair.getMap()[i, u] == par1case && canSee(i, u))
                        {
                            objective = new Point(i, u);
                        }
                    }
                    catch (Exception)
                    { }
                }
            }
        }

        protected void sleep(int par1time = 2880)
        {
            tour = 0;
            frame = 0;
            sleepingTime = par1time;
            isSleeping = true;
        }

        public bool isInRange(int par1x, int par2y)
        {
            if (Math.Abs(x - par1x) + Math.Abs(y - par2y) <= range)
            {
                return true;
            }
            return false;

        }

        public int getRange()
        {
            return range;
        }

        public bool canSee(int par1x, int par2y)
        {
            if (Math.Abs(x - par1x) + Math.Abs(y - par2y) <= range)
            {
                int dx = Math.Abs(par1x - x);
                int dy = Math.Abs(par2y - y);
                int sx = 1;
                int rx = x;
                int ry = y;
                if (x > par1x)
                    sx = -1;
                int sy = 1;
                if (y > par2y)
                    sy = -1;
                int err = dx - dy;
                while (!((rx == par1x) && (ry == par2y)))
                {
                    int e2 = err << 1;
                    if (e2 > -dy)
                    {
                        err -= dy;
                        rx += sx;
                    }
                    if (e2 < dx)
                    {
                        err += dx;
                        ry += sy;
                    }
                    if (canWalkOn(rx, ry) == false)
                    {
                        // Console.WriteLine("bloqué");
                        break;
                    }
                }
                //Console.WriteLine("vu");
                if (rx != par1x || ry != par2y)
                    return false;
                return true;
            }
            //Console.WriteLine("hors champs de vision");
            return false;
        }

        public void kill()
        {
            dead = true;
        }

        public string getSpecies()
        {
            return espece;
        }

        //<summary>
        //retourne si l'entitée peut marcher sur la case située aux coordonnées spécifiées
        //</summary>
        public bool canWalkOn(int par1x, int par2y)
        {
            try
            {
                if (stair != null && stair.getMap()[par1x, par2y] == 1 || stair.getMap()[par1x, par2y] == 7 || stair.getMap()[par1x, par2y] == 100 || stair.getMap()[par1x, par2y] == 6 || stair.getMap()[par1x, par2y] == 3 || stair.getMap()[par1x, par2y] == 5 || stair.getMap()[par1x, par2y] == 101)
                {
                    if (stair != null && stair.getSpecial()[par1x, par2y] != null)
                        return stair.getSpecial()[par1x, par2y].canPass();
                    else
                    {
                        Entity[] e = stair.getEntities();
                        for (int i = 0; i < e.Length; i++)
                        {
                            if (e[i] != null && e[i].getX() == par1x && e[i].getY() == par2y)
                                return false;
                        }
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            { return true; }
        }

        //<summary>
        //retourne si l'entitée peut marcher sur la case située aux coordonnées spécifiées
        //</summary>
        public static bool canWalkOn(int par1x, int par2y,Stair stair)
        {

            try
            {
                if (stair != null && stair.getMap()[par1x, par2y] == 1 || stair.getMap()[par1x, par2y] == 7 || stair.getMap()[par1x, par2y] == 100 || stair.getMap()[par1x, par2y] == 4 || stair.getMap()[par1x, par2y] == 3 || stair.getMap()[par1x, par2y] == 5)
                {
                    
                    if (stair != null && stair.getSpecial()[par1x, par2y] != null)
                        return stair.getSpecial()[par1x, par2y].canPass();
                    else
                    {
                        Entity[] e = stair.getEntities();
                        for (int i = 0; i < e.Length; i++)
                        {
                            if (e[i] != null && e[i].getX() == par1x && e[i].getY() == par2y)
                                return false;
                        }
                        return true;
                    }
                }
                {
                    
                    return false;
                }
            }
            catch (Exception)
            { return true; }
        }

        public int[] getStat()
        {
            int[] value = new int[5];
            value[0] = lifeTmp;
            value[1] = (int)faim;
            value[2] = (int)soif;
            value[3] = (int)sommeil;
            value[4] = (int)sale;
            return value;

        }

        public int getLife()
        {
            return life;
        }

        public Inventory getInventory()
        {
            return inventory;
        }

        public void setHungry(float par1)
        {
            faim = par1;
        }

        public float getHungry()
        {
            return faim;
        }

        public void setThrirst(float par1)
        {
            soif = par1;
        }

        public float getThrirst()
        {
            return soif;
        }
    }
}