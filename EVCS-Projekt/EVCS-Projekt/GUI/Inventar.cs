using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EVCS_Projekt.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EVCS_Projekt.GUI
{
    class Inventar : UIPanel, UIActionListener
    {
        private UIList inventarList;
        private Texture2D pixel;
        public bool Visible { get; set; }
        private Rectangle background;
        private UIPanel checkBoxPanel;
        private UIToggleButton toggle;
        private Color backgroundColor = Color.Gray;

        public Inventar( int width, int height, Vector2 position )
            : base( width, height, position )
        {
            inventarList = new UIList( 400, 300, new Vector2( 0, 0 ), Main.MainObject.GameManager.GameState.Player.Inventar );

            Add( inventarList );
            Visible = false;

            if ( pixel == null )
                pixel = Main.ContentManager.Load<Texture2D>( "images/pixelWhite" );

            background = new Rectangle( ( int ) GetPosition().X, ( int ) GetPosition().Y, width, height );
            Add( inventarList );

            ContentManager content = Main.ContentManager;

            Texture2D abtrieb = content.Load<Texture2D>( "images/gui/toggle_antrieb" );
            Texture2D abtriebHover = content.Load<Texture2D>( "images/gui/toggle_antriebHover" );
            Texture2D abtriebActive = content.Load<Texture2D>( "images/gui/toggle_active_antrieb" );
            Texture2D abtriebActiveHover = content.Load<Texture2D>( "images/gui/toggle_active_antriebHover" );


            toggle = new UIToggleButton( 300, 50, new Vector2( 400, 0 ), abtrieb, abtriebHover, abtriebActive, abtriebActiveHover, "" );
            toggle.AddActionListener(this);
            Add( toggle );
        }

        private void CreateCheckBoxPanel()
        {
            checkBoxPanel = new UIPanel( 400, 200, new Vector2( 600, 0 ) );


        }

        public override void Draw( SpriteBatch sb )
        {
            sb.Draw( pixel, background, backgroundColor );
            base.Draw( sb );
        }

        public void OnMouseDown(UIElement element)
        {
            backgroundColor = Color.Green;
        }

        public void OnMouseUp(UIElement element)
        {
            backgroundColor = Color.Gray;
        }
    }
}
