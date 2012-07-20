using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LastMan.UI
{
    class UIVerticalProgressBar : UIPanel
    {
        public  float MaxValue;
        public float Progress;

        public float Required { get; set; }
        private Texture2D overlay;

        public UIVerticalProgressBar( int width, int height, Vector2 position, float maxValue, float progress )
            : base( width, height, position )
        {
            MaxValue = maxValue;
            Progress = progress;
            Required = 0;
            BackgroundTextur = Main.ContentManager.Load<Texture2D>( "images/gui/health_bar" );
            overlay = Main.ContentManager.Load<Texture2D>( "images/gui/bar_overlay" );
        }

        public override void Draw( Microsoft.Xna.Framework.Graphics.SpriteBatch sb )
        {
            Texture2D pixelWhite = Main.ContentManager.Load<Texture2D>( "images/pixelWhite" );
            //sb.Draw(pixelWhite, new Rectangle((int)position.X, (int)position.Y, width, height), Color.White);

            float prog = Math.Min( Progress * 100 / MaxValue / 100, 1.0F );
            float prog1 = Math.Min( Required * 100 / MaxValue / 100, 1.0F );

            int barHeight = height;
            int barWidht = 30;

            int barX = ( int ) position.X + width - 30;

            Rectangle clippingBox = new Rectangle( barX, ( int ) position.Y, barWidht, ( barHeight ) );
            Rectangle box = new Rectangle( barX, ( int ) ( position.Y + barHeight - barHeight * prog ), barWidht, ( int ) ( barHeight * prog ) );

            Rectangle box2 = new Rectangle( box.X, box.Y, barWidht, Math.Min( ( int ) ( barHeight * prog1 ), box.Height ) );
            sb.Draw( overlay, clippingBox, Color.White );

            sb.Draw( BackgroundTextur, box, BackgroundColor * 0.5F );
            sb.Draw( BackgroundTextur, box2, BackgroundColor );

            //sb.DrawString(UIButton.FONT_DEFAULT, "20/23", new Vector2(position.X, position.Y + height - 20), Color.Black);

            string text = Required + "/" + Progress;
            Vector2 measureString = UIButton.FONT_DEFAULT.MeasureString( text );

            Vector2 origin = new Vector2( width / 2, measureString.Y / 2 );

            Color textColor = Color.Red;
            if ( IsSnoughLiquid() )
                textColor = Color.Black;
            sb.DrawString( UIButton.FONT_DEFAULT, text, new Vector2( position.X + 20, position.Y + height - width / 2 - measureString.Y / 2 ), textColor, -( float ) Math.PI / 2, origin, 1F, SpriteEffects.None, 1F );


            //base.Draw(sb);
        }

        public bool IsSnoughLiquid()
        {
            return Progress >= Required;
        }
    }
}
