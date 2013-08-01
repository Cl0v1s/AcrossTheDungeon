using System;
using System.Drawing;
using SdlDotNet.Input;
using SdlDotNet.Graphics;

namespace DW
{
    class SpellsUI
    {
        private Spell[] list = new Spell[4];
        private Player owner;
        private int x;
        private int y;
        private double enduranceB;
        private double enduranceOld;

        public SpellsUI(Player par3owner,int par1x, int par2y,Spell[] par4list)
        {
            owner = par3owner;
            x = par1x;
            y = par2y;
            for (int i = 0; i < par4list.Length; i++)
            {
                if (i < list.Length)
                    list[i] = par4list[i];
            }

        }

        //<summary>
        //ajoute un sort à la liste de sorts connus par le joueur.
        //</summary>
        //<param name="par1">Le sort à ajouter</param>
        public bool addSpell(Spell par1)
        {
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i] == null)
                {
                    list[i] = par1;
                    return true;
                }
            }
            return false;
        }

        //<summary>
        //Retire un sort de la liste de sorts connus par le joueur.
        //</summary>
        //<param name="par1">Le sort à retirer</param>
        public bool removeSpell(Spell par1)
        {
            string r = par1.name;
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i] != null && list[i].name == r)
                {
                    list[i] = null;
                    return true;
                }
            }
            return false;
        }

        public void update()
        {
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i] != null)
                    list[i].update(x + i * 40, y);
            }
            if (enduranceOld == 0)
            {
                enduranceOld = DW.player.enduranceTmp;
                enduranceB = DW.player.enduranceTmp;
            }
            if (enduranceOld != DW.player.enduranceTmp)
            {
                if (enduranceB > DW.player.enduranceTmp)
                {
                    enduranceB -= 0.5;
                    if (enduranceB <= DW.player.enduranceTmp)
                        enduranceOld = DW.player.enduranceTmp;
                }
                else
                {
                    enduranceB += 0.5;
                    if (enduranceB >= DW.player.enduranceTmp)
                        enduranceOld = DW.player.enduranceTmp;
                }

            }
            int l=(int)(enduranceB*150/DW.player.endurance);
            Video.Screen.Fill(new Rectangle(640/2+19-l/2, y + 35, l, 10), Color.DarkCyan);
            
        }

        public Spell getSpell(Key par1key)
        {
            try
            {
                if (par1key == Key.Space && list[0] != null)
                    return list[0];
                else if (par1key == Key.V && list[1] != null)
                    return list[1];
                else if (par1key == Key.B && list[2] != null)
                    return list[2];
                else if (par1key == Key.N && list[2] != null)
                    return list[3];
                else
                    return null;
            }
            catch (System.IndexOutOfRangeException)
            {
                return null;
            }
        }
    }
}
