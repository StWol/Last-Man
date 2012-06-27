using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using EVCS_Projekt.Objects;
using EVCS_Projekt.Objects.Items;
using EVCS_Projekt.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EVCS_Projekt.GUI
{
    /// <summary>
    /// Ein Button fuer die Liste
    /// </summary>
    internal class UIListButton : UIButton
    {
        public  Item Item;
        private  Texture2D icon;
        private  Texture2D weightIcon;
        private  string name;
        public  string CountString;
        private  string weightCount;

        public Color Color;
        public Color HoverColor;
        private Rectangle iconRect;
        private Vector2 namePos;
        private Vector2 countPos;
        private Rectangle weightRect;
        private Vector2 weightCountPos;

        private Texture2D active;
        private Texture2D activeHover;

        public bool isActive;

        public override Texture2D CurrentTexture
        {
            get
            {
                if ( isActive )
                    if ( IsHover )
                        return activeHover;
                    else
                    {
                        return active;
                    }
                else
                {
                    return base.CurrentTexture;
                }

            }
        }



        public UIListButton( int width, int height, Vector2 position, Item item, int count )
            : base( width, DEFAULT_HEIGHT, position, Main.ContentManager.Load<Texture2D>( "images/gui/inventar/listitem" ), Main.ContentManager.Load<Texture2D>( "images/gui/inventar/listitem_h" ), "" )
        {

            active = Main.ContentManager.Load<Texture2D>( "images/gui/inventar/listitem_a" );
            activeHover = Main.ContentManager.Load<Texture2D>( "images/gui/inventar/listitem_a_h" );

            Item = item;

            icon = item.Icon;
            name = item.Name;
            CountString = count + "";
            weightIcon = Main.ContentManager.Load<Texture2D>( "images/gui/inventar/weight" );
            if(item.GetType() == typeof(Weapon))
                Debug.WriteLine("");
            weightCount = item.Weight + "";

            Color = new Color( 0, 0, 0, 100 );
            HoverColor = new Color( 255, 255, 255, 100 );
            isActive = false;
        }

        public override void Draw( SpriteBatch sb )
        {
            base.Draw( sb );
            Color active;
            if ( IsMouseOver() )
            {
                Color = Color.White;
            }
            else
            {
                Color = Color.Gray;

            }

            Vector2 measureString = UIButton.FONT_DEFAULT.MeasureString( name );

            int x = ( int ) GetPosition().X;
            int y = ( int ) GetPosition().Y;

            float textY = y + height / 2 - measureString.Y / 2;

            iconRect = new Rectangle( x + 0, y + 0, height, height );
            namePos = new Vector2( x + height + 10, textY );
            countPos = new Vector2( x + width - height * 2 - 13 + 10, textY );
            weightRect = new Rectangle( x + width - height - 13, y + 0, 13, height );
            weightCountPos = new Vector2( x + width - height + 10, textY );

            sb.Draw( icon, iconRect, Color.White );
            sb.DrawString( UIButton.FONT_DEFAULT, name, namePos, Color.Black );
            sb.DrawString( UIButton.FONT_DEFAULT, CountString, countPos, Color.Black );
            sb.Draw( weightIcon, weightRect, Color.White );
            sb.DrawString( UIButton.FONT_DEFAULT, weightCount, weightCountPos, Color.Black );
        }
    }
}
