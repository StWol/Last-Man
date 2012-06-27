using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using EVCS_Projekt.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EVCS_Projekt.UI
{
    class UIPanel : UIElement
    {


        protected List<UIElement> children;

        public UIPanel( int width, int height, Vector2 position )
            : base( width, height, position )
        {

            children = new List<UIElement>();
        }

        public UIPanel( int width, int height, Vector2 position , Texture2D texture, Texture2D hoverTextur)
            : base( width, height, position, texture, hoverTextur )
        {

            children = new List<UIElement>();
        }

        public void Add( UIElement element )
        {
            element.SetParent( this );
            children.Add( element );
        }

        public void Remove( UIElement element )
        {
            children.Remove( element );
        }

        public override void Draw( SpriteBatch sb )
        {
            sb.Draw(BackgroundTextur, new Rectangle((int)position.X, (int)position.Y, width, height), BackgroundColor);

            foreach ( UIElement e in children )
            {
                e.Draw( sb );
            }
        }

        public void Clear()
        {
            children.Clear();
        }

        public override void Update()
        {
            base.Update();
            List<UIElement> clone = new List<UIElement>( children );
            foreach ( UIElement e in clone )
            {
                e.Update();
            }
        }
    }
}
