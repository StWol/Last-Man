using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EVCS_Projekt.UI
{
    class UIToggleButton : UIButton
    {
        private Texture2D activeTexture;
        private Texture2D activeTextureHover;
        public bool isActive;

        
        public UIToggleButton( int width, int height, Vector2 position, Texture2D texture, Texture2D hoverTexture, Texture2D activeTexture, Texture2D activeTextureHover, string text )
            : base( width, height, position, texture, hoverTexture, text )
        {
            
            this.activeTexture = activeTexture;
            this.activeTextureHover = activeTextureHover;
            isActive = false;
        }

        public override Texture2D CurrentTexture
        {
            get
            {
                if ( isActive )
                {
                    if ( IsHover )
                        return activeTextureHover;
                    return activeTexture;
                }
                return base.CurrentTexture;
            }
        }

        public override void Update()
        {
            if ( IsMousePressed() )
            {
                var listenerList = new List<UIActionListener>( actionListener );
                foreach ( UIActionListener al in listenerList )
                {
                    if ( isActive )
                    {
                        al.OnMouseUp( this );
                    }
                    else
                    {
                        al.OnMouseDown( this );
                    }
                }
                isActive = !isActive;
            }
        }
    }
}
