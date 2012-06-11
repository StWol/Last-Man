using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EVCS_Projekt.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EVCS_Projekt.GUI
{
    class Inventar : UIPanel
    {
        private UIList inventarList;
        public bool Visible { get; set; }

        public Inventar(int width, int height, Vector2 position) : base(width, height, position)
        {
            inventarList = new UIList(200,300,new Vector2(0,0), Main.MainObject.GameManager.GameState.Player.Inventar );
            Add(inventarList);
            Visible = false;
        }

    }
}
