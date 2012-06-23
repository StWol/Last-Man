using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using EVCS_Projekt.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EVCS_Projekt.UI
{
    class UIInfoPanel : UIPanel
    {
        private Item item;
        string group = "";
        string name = "";
        string desc = "";

        public Item Item
        {
            get { return item; }
            set { item = value; }
        }



        public UIInfoPanel( int width, int height, Vector2 position )
            : base( width, height, position )
        {


        }


        public override void Draw( Microsoft.Xna.Framework.Graphics.SpriteBatch sb )
        {
            int x = ( int ) ( GetPosition().X );
            int y = ( int ) ( GetPosition().Y );

            sb.Draw( Main.ContentManager.Load<Texture2D>( "images/pixelWhite" ), GetBoundingBox(), Color.White );

            if ( item != null )
            {

                group = item.Group.ToString();
                name = item.Name;
                desc = item.Description;
            }
            else
            {
                group = "";
                name = "";
                desc = "";
            }
            int lineHeight = 30;
            int padding = 20;

            sb.DrawString( UIButton.FONT_DEFAULT, name, new Vector2( padding + x, padding + y ), Color.Black );
            sb.DrawString( UIButton.FONT_DEFAULT, desc, new Vector2( padding + x, padding + y + lineHeight ), Color.Black );
            sb.DrawString( UIButton.FONT_DEFAULT, group, new Vector2( padding + x, padding + y + lineHeight * 2 ), Color.Black );

            base.Draw( sb );

        }
    }
}
