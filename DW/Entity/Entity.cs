using System;

using System.Drawing;

namespace DW
{
    [Serializable]
    class Entity : Special
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
        protected int force;
        protected int endurance;
        protected double enduranceTmp;
        protected int volonte;
        protected int agilite;
        protected int range = 5;

        protected float faim = 89;
        protected float soif = 89;
        protected float sommeil = 89;
        protected float sale = 89;
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
        protected bool isFighting = false;
        protected bool isburning = false;
        protected int attempt = 0;


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
                    if (isFighting == false)
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

        public bool isInFight()
        {
            return isFighting;
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

        protected double getPower()
        {
            return (double)((force * agilite) / (faim + soif + sommeil));
        }

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
                        peur = peur + (worstEnemy.getPower() - getPower());
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
                isFighting = true;
                fight(this, worstEnemy);
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

        public void fight(Entity par1cause, Entity par2victim)
        {
            par1cause.setFighting(true);
            par2victim.setFighting(true);
            par1cause.atk(par2victim);
            par1cause.setEnemy(par2victim);
            par2victim.setEnemy(par1cause);
            par1cause.lookTo(par2victim);
            par2victim.lookTo(par1cause);
            if (par2victim.getStat()[0] <= 0 || par1cause.isNear(par2victim) == false)
            {
                par1cause.setFighting(false);
                return;
            }
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
            if (stair.getMap()[x, y] == par1)
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

        public int atk(Entity par1)
        {
            int atk = 0;
            if (enduranceTmp > 0)
            {
                atk = (int)(force * (1 + (rand.Next(0, 5) / 100)) + enduranceTmp);
                enduranceTmp -= atk / 50;
                if (rand.Next(agilite, 100) == 100)
                    atk = atk * 2;
                if (par1.def(atk) <= 0)
                    isFighting = false;
            }
            return atk;

        }

        public int def(int par1)
        {
            int def = 0;
            if (enduranceTmp > 0)
            {
                def = (int)((enduranceTmp + 1.5 * enduranceTmp) + volonte);
                if (rand.Next(agilite, 100) == 100)
                    def = def * 2;
            }
            lifeTmp -= par1 * (1 - def / 100);
            return lifeTmp;
        }

        public void kill()
        {
            dead = true;
        }

        public string getSpecies()
        {
            return espece;
        }

        public void setFighting(bool par1)
        {
            isFighting = par1;
        }

        public string getName()
        {
            return name;
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