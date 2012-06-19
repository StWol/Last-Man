using System;
using System.Collections.Generic;
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

        private bool mouseDown = false;

        private Boolean isHover;
        private SpriteFont fontDefault;
        public Color BackgroundColor { get; set; }



        public UIButton(int width, int height, Vector2 position, Texture2D texture, Texture2D hoverTexture)
        : base(width, height, position, texture, hoverTexture)
        {
            
            isHover = false;
            fontDefault = Main.ContentManager.Load<SpriteFont>("fonts/defaultSmall");
            BackgroundColor = Color.Gray;
        }

        public UIButton(int width, int height, Vector2 position, string text)
            : base(width, height, position)
        {
            Text = text;
            isHover = false;
            fontDefault = Main.ContentManager.Load<SpriteFont>("fonts/defaultSmall");
            BackgroundColor = Color.Gray;
        }

        public UIButton(Vector2 position, Texture2D texture, Texture2D hoverTexture)
            : base(texture.Width, texture.Height, position, texture, hoverTexture)
        {
            isHover = false;
        }


        public override void Draw(SpriteBatch sb)
        {
            int x = (int)(GetPosition().X);
            int y = (int)(GetPosition().Y);

            if (CurrentTexture == null)
            {

                sb.Draw(Main.ContentManager.Load<Texture2D>("images/pixelWhite"), new Rectangle(x, y, width, height), BackgroundColor);
                sb.DrawString(fontDefault, Text, new Vector2(x, y), Color.Black);
            }
            else
            {
                sb.Draw(CurrentTexture, new Rectangle(x, y, width, height), Color.White);
            }
        }
        
    }
}
