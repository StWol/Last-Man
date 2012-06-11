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

        private bool mouseDown = false;

        private Boolean isHover;
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
            sb.Draw(currentTexture, new Rectangle(x, y, width, height), Color.White);
        }

    }
}
