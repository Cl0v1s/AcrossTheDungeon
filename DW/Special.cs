using System;

using System.Drawing;
using SdlDotNet.Graphics.Sprites;


namespace DW
{
    [Serializable]
    class Special
    {
        protected Stair stair;
        protected int x;
        protected int y;
        protected string value="";
        protected string originalValue = "";
        protected Color color=Color.Gray;
        protected Sprite sprite;

        public Special(Stair par1stair)
        {
            stair = par1stair;
        }

        public Special()
        { }

        public string getChar()
        {
            return value;
        }

        public Color getColor()
        {
            return color;
        }

        public void setPos(Stair par3stair,int par1x, int par2y)
        {
            stair = par3stair;
            x = par1x;
            y = par2y;
        }

        public void setPos(int par1x, int par2y)
        {
            x = par1x;
            y = par2y;
        }

        public int getX()
        {
            return x;
        }

        public int getY()
        {
            return y;
        }

        public Stair getStair()
        {
            return stair;
        }

        public virtual Special clone()
        {
            return (Special)this.MemberwiseClone();
        }

        public virtual bool canPass()
        {
            return false;
        }

        public virtual void interact(Player par1)
        {

        }

        public virtual void update()
        {

        }
    }
}
