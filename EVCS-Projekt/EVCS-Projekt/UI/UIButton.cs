using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace EVCS_Projekt.UI
{
    internal class UIButton : UIElement
    {


        public string Text { get; set; }

        protected bool mouseDown = false;

        private Boolean isHover;
        public static readonly SpriteFont FONT_DEFAULT = Main.ContentManager.Load<SpriteFont>( Configuration.Get( "defaultFont" ) );
        public Color BackgroundColor { get; set; }

        private static Texture2D textureDefault = Main.ContentManager.Load<Texture2D>( "images/pixelWhite" );

        


        public UIButton( int width, int height, Vector2 position, Texture2D texture, Texture2D hoverTexture, string text )
            : base( width, height, position, texture, hoverTexture )
        {
            isHover = false;
            BackgroundColor = Color.Gray;
            Text = text.Trim();

        }



        public UIButton( int width, int height, Vector2 position, string text )
            : this( width, height, position, null, null, text )
        {

        }

        public UIButton( Vector2 position, Texture2D texture, Texture2D hoverTexture )
            : this( texture.Width, texture.Height, position, texture, hoverTexture, "" )
        {

        }


        public override void Draw( SpriteBatch sb )
        {
            int x = ( int ) ( GetPosition().X );
            int y = ( int ) ( GetPosition().Y );

            if ( BackgroundTextur != null )
            {
                sb.Draw( BackgroundTextur, new Rectangle( x, y, width, height ), Color.White );
            }

            if ( Text != null )
            {
                Vector2 measureString = FONT_DEFAULT.MeasureString( Text );

                //sb.Draw(Main.ContentManager.Load<Texture2D>("images/pixelWhite"), new Rectangle(x, y, width, height), BackgroundColor);
                sb.DrawString( FONT_DEFAULT, Text, new Vector2( x + 10, y + height / 2 - measureString.Y / 2 ), Color.Black );
            }

            if ( IsEnabled )
            {
                BackgroundColor = Color.White;

            }
            else
            {
                BackgroundColor = new Color( 255, 255, 255, 30 );
            }
            if ( CurrentTexture != null )
            {
                sb.Draw( CurrentTexture, new Rectangle( x, y, width, height ), BackgroundColor );
            }


        }

    }
}
