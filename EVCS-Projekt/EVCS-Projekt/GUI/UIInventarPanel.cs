using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EVCS_Projekt.Objects;
using EVCS_Projekt.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EVCS_Projekt.GUI
{
    class UIInventarPanel : UIPanel, UIActionListener
    {
        private UIList inventarList;
        private Texture2D pixel;
        public bool Visible { get; set; }
        private Rectangle background;
        private UIPanel checkBoxPanel;
        private Color backgroundColor = Color.Gray;
        private List<Item> playerInventar;

        private UIToggleButton toggleWaffe;
        private UIToggleButton toggleHauptteil;
        private UIToggleButton toggleStabilisator;
        private UIToggleButton toggleMunition;
        private UIToggleButton toggleVisier;
        private UIToggleButton toggleAntrieb;

        public UIInventarPanel( int width, int height, Vector2 position )
            : base( width, height, position )
        {
            playerInventar = Main.MainObject.GameManager.GameState.Player.Inventar;
            inventarList = new UIList( 380, 250, new Vector2( 300, 100 ), playerInventar );

            Add( inventarList );
            Visible = false;

            if ( pixel == null )
                pixel = Main.ContentManager.Load<Texture2D>( "images/pixelWhite" );

            background = new Rectangle( ( int ) GetPosition().X, ( int ) GetPosition().Y, width, height );
            Add( inventarList );


            CreateCheckBoxPanel();


        }

        private void CreateCheckBoxPanel()
        {
            ContentManager content = Main.ContentManager;

            Texture2D waffe = content.Load<Texture2D>( "images/gui/inventar/waffe" );
            Texture2D waffeH = content.Load<Texture2D>( "images/gui/inventar/waffe_h" );
            Texture2D waffeA = content.Load<Texture2D>( "images/gui/inventar/waffe_a" );
            Texture2D waffeAH = content.Load<Texture2D>( "images/gui/inventar/waffe_a_h" );

            Texture2D hauptteil = content.Load<Texture2D>( "images/gui/inventar/hauptteil" );
            Texture2D hauptteilH = content.Load<Texture2D>( "images/gui/inventar/hauptteil_h" );
            Texture2D hauptteilA = content.Load<Texture2D>( "images/gui/inventar/hauptteil_a" );
            Texture2D hauptteilAH = content.Load<Texture2D>( "images/gui/inventar/hauptteil_a_h" );

            Texture2D stabilisator = content.Load<Texture2D>( "images/gui/inventar/stabilisator" );
            Texture2D stabilisatorH = content.Load<Texture2D>( "images/gui/inventar/stabilisator_h" );
            Texture2D stabilisatorA = content.Load<Texture2D>( "images/gui/inventar/stabilisator_a" );
            Texture2D stabilisatorAH = content.Load<Texture2D>( "images/gui/inventar/stabilisator_a_h" );

            Texture2D munition = content.Load<Texture2D>( "images/gui/inventar/munition" );
            Texture2D munitionH = content.Load<Texture2D>( "images/gui/inventar/munition_h" );
            Texture2D munitionA = content.Load<Texture2D>( "images/gui/inventar/munition_a" );
            Texture2D munitionAH = content.Load<Texture2D>( "images/gui/inventar/munition_a_h" );

            Texture2D visier = content.Load<Texture2D>( "images/gui/inventar/visier" );
            Texture2D visierH = content.Load<Texture2D>( "images/gui/inventar/visier_h" );
            Texture2D visierA = content.Load<Texture2D>( "images/gui/inventar/visier_a" );
            Texture2D visierAH = content.Load<Texture2D>( "images/gui/inventar/visier_a_h" );
            
            Texture2D antrieb = content.Load<Texture2D>( "images/gui/inventar/antrieb" );
            Texture2D antriebH = content.Load<Texture2D>( "images/gui/inventar/antrieb_h" );
            Texture2D antriebA = content.Load<Texture2D>( "images/gui/inventar/antrieb_a" );
            Texture2D antriebAH = content.Load<Texture2D>( "images/gui/inventar/antrieb_a_h" );

            toggleWaffe = new UIToggleButton( 120, 30, new Vector2( 300, 20 ), waffe, waffeH, waffeA, waffeAH, "" );
            toggleHauptteil = new UIToggleButton( 120, 30, new Vector2( 430, 20 ), hauptteil, hauptteilH, hauptteilA, hauptteilAH, "" );
            toggleStabilisator = new UIToggleButton( 120, 30, new Vector2( 560, 20 ), stabilisator, stabilisatorH, stabilisatorA, stabilisatorAH, "" );
            toggleMunition = new UIToggleButton( 120, 30, new Vector2( 300, 60 ), munition, munitionH, munitionA, munitionAH, "" );
            toggleVisier = new UIToggleButton( 120, 30, new Vector2( 430, 60 ), visier, visierH, visierA, visierAH, "" );
            toggleAntrieb = new UIToggleButton( 120, 30, new Vector2( 560, 60 ), antrieb, antriebH, antriebA, antriebAH, "" );

            toggleWaffe.AddActionListener( this );
            toggleHauptteil.AddActionListener( this );
            toggleStabilisator.AddActionListener( this );
            toggleMunition.AddActionListener( this );
            toggleVisier.AddActionListener( this );
            toggleAntrieb.AddActionListener( this );


            Add( toggleWaffe );
            Add( toggleHauptteil );
            Add( toggleStabilisator );
            Add( toggleMunition );
            Add( toggleVisier );
            Add( toggleAntrieb );
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
