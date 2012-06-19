using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EVCS_Projekt.UI
{
    class UIToggleButton: UIButton
    {
        private Texture2D activeTexture;
        private bool isActive;

        public UIToggleButton(int width, int height, Vector2 position, Texture2D texture, Texture2D hoverTexture, Texture2D activeTexture, string text) 
            : base(width, height, position, texture, hoverTexture, text)
        {
            this.activeTexture = activeTexture;
            isActive = false;
        }

        public override void Draw(SpriteBatch sb)
        {
            if (IsMousePressed() && isActive)
            {
                List<UIActionListener> listenerList = new List<UIActionListener>(listener);
                foreach (UIActionListener al in listenerList)
                {
                    al.OnMouseUp(this);
                }
            }
            else
            {
                //texture = activeTexture;
            }
            base.Draw(sb);
        }
    }
}
