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
        private Texture2D pixel;
        public bool Visible { get; set; }
        private Rectangle background;

        public Inventar(int width, int height, Vector2 position) : base(width, height, position)
        {
            inventarList = new UIList( 400, 300, position, Main.MainObject.GameManager.GameState.Player.Inventar );

            Add(inventarList);
            Visible = false;

            if ( pixel == null )
                pixel = Main.ContentManager.Load<Texture2D>( "images/pixelWhite" );

            background = new Rectangle((int) GetPosition().X, (int) GetPosition().Y, width, height);
            Add(inventarList);
        }


        public override void Draw( SpriteBatch sb )
        {
            sb.Draw( pixel, background, Color.Gray );
            base.Draw(sb);
        }
    }
}
