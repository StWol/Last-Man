﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LastMan.UI
{
    class UIToggleButton : UIButton
    {
        private Texture2D activeTexture;
        private Texture2D activeTextureHover;
        public bool IsActive;

        
        public UIToggleButton( int width, int height, Vector2 position, Texture2D texture, Texture2D hoverTexture, Texture2D activeTexture, Texture2D activeTextureHover, string text )
            : base( width, height, position, texture, hoverTexture, text )
        {
            
            this.activeTexture = activeTexture;
            this.activeTextureHover = activeTextureHover;
            IsActive = false;
        }

        public override Texture2D CurrentTexture
        {
            get
            {
                if ( IsActive )
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
                    if ( IsActive )
                    {
                        al.OnMouseUp( this );
                    }
                    else
                    {
                        al.OnMouseDown( this );
                    }
                }
                IsActive = !IsActive;
            }
        }
    }
}
