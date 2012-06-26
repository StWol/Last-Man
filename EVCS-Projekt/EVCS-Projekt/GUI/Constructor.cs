using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EVCS_Projekt.Objects;
using EVCS_Projekt.Objects.Items;
using EVCS_Projekt.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EVCS_Projekt.GUI
{
    class Constructor : UIPanel, UIActionListener
    {
        private bool isVisible;
        private UIFilteredConstructorList filteredConstructorList;
        private Player player;
        private Item  activeItem;
        private Rectangle background;
        private Texture2D backgroundTextur;

        public bool Visible
        {
            get { return isVisible; }
            set
            {
                if ( value && !isVisible )
                {
                    filteredConstructorList.GenerateFilteredLists( player.Inventar );
                    //GenerateUiComponents();
                    
                    //filteredConstructorList.SetItems( tempDict );
                    filteredConstructorList.ResetToggleButtons();
                    activeItem = null;
                }
                isVisible = value;
            }
        }

        public Constructor( int width, int height, Vector2 position )
            : base( width, height, position )
        {
            player = Main.MainObject.GameManager.GameState.Player;

            Visible = false;

            background = new Rectangle( ( int ) GetPosition().X, ( int ) GetPosition().Y, base.width, base.height );
            backgroundTextur = Main.ContentManager.Load<Texture2D>( "images/gui/inventar/inventar_background" );

            Helper.DrawHelper.AddDimension( "TextLinePadding", 20, 20 );
            GenerateUiComponents();
        }

        private void GenerateUiComponents()
        {
            var shortCutTitel = new UIButton( 240, 40, new Vector2( 20, 20 ), "Konstruktor" ) { BackgroundColor = Color.LightGray };
            var inventarTitel = new UIButton( 250, 40, new Vector2( 340, 20 ), "Inventar" ) { BackgroundColor = Color.LightGray };
            var filterTitel = new UIButton( 120, 40, new Vector2( 620, 20 ), "Filter" ) { BackgroundColor = Color.LightGray };

            var ok = Main.ContentManager.Load<Texture2D>( "images/gui/inventar/btn_ok" );
            var ok_h = Main.ContentManager.Load<Texture2D>( "images/gui/inventar/btn_ok_h" );
            var cancel = Main.ContentManager.Load<Texture2D>( "images/gui/inventar/btn_cancel" );
            var cancel_h = Main.ContentManager.Load<Texture2D>( "images/gui/inventar/btn_cancel_h" );


            filteredConstructorList = new UIFilteredConstructorList( 260, 236, new Vector2( 340, 60 ), this );
            Add( filteredConstructorList );


            //btnOk = new UIButton( new Vector2( 300, 306 ), ok, ok_h );
            //btnCancel = new UIButton( new Vector2( 350, 306 ), cancel, cancel_h );

            //btnOk.IsEnabled = false;
            //btnCancel.IsEnabled = false;

            //btnOk.AddActionListener( this );
            //btnCancel.AddActionListener( this );


            Add( shortCutTitel );
            Add( inventarTitel );
            Add( filterTitel );

            //Add( btnOk );
            //Add( btnCancel );
        }

        public override void Draw( SpriteBatch sb )
        {
            sb.Draw( backgroundTextur, background, Color.White );
            base.Draw( sb );
        }

        public void OnMouseDown(UIElement element)
        {
            throw new NotImplementedException();
        }

        public void OnMouseUp(UIElement element)
        {
            throw new NotImplementedException();
        }
    }
}
