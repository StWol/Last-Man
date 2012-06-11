using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace EVCS_Projekt.UI
{
    class UIButton : UIElement
    {

        private Texture2D texture;
        private Texture2D hoverTexture;
        public string Text { get; set; }

        private bool mouseDown = false;

        private Boolean isHover;
        private SpriteFont fontDefault;

        private Texture2D currentTexture
        {
            get
            {
                if (isHover)
                    return hoverTexture;
                else
                    return texture;
            }
        }

        public UIButton(int width, int height, Vector2 position, Texture2D texture, Texture2D hoverTexture)
            : base(width, height, position)
        {
            this.texture = texture;
            this.hoverTexture = hoverTexture;
            isHover = false;
            fontDefault = Main.ContentManager.Load<SpriteFont>("fonts/defaultMedium");
        }

        public UIButton(int width, int height, Vector2 position, string text)
            : base(width, height, position)
        {
            Text = text;
            isHover = false;
            fontDefault = Main.ContentManager.Load<SpriteFont>("fonts/defaultMedium");
        }

        public UIButton(Vector2 position, Texture2D texture, Texture2D hoverTexture)
            : base(texture.Width, texture.Height, position)
        {
            this.texture = texture;
            this.hoverTexture = hoverTexture;
            isHover = false;
        }


        public override void Draw(SpriteBatch sb)
        {
            int x = (int)(position.X + parent.GetPosition().X);
            int y = (int)(position.Y + parent.GetPosition().Y);
            
            if(texture == null)
                sb.DrawString(fontDefault, Text, new Vector2(x, y), Color.Black);
            else
            {
                sb.Draw(currentTexture, new Rectangle(x, y, width, height), Color.White);
            }
        }
        
    }
}
