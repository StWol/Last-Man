using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Text;
using EVCS_Projekt.GUI;
using EVCS_Projekt.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EVCS_Projekt.UI
{
    class UIList : UIPanel, UIActionListener
    {
        private  List<UIElement> buttonList;

        public Dictionary<int, int> ItemList
        {
            get { return itemList; }
            set
            {
                itemList = value;
                GenerateButtons();
            }
        }


        private  Dictionary<int, int> countItemsDict;

        public int FirsVisibleButtonIndex { get { return firsVisibleButtonIndex; } set { firsVisibleButtonIndex = 0; } }
        private int firsVisibleButtonIndex = 0;
        private int MAX_VISIBLE_BUTTON_COUNT = 5;

        private Rectangle box;

        private readonly UIButton btnPrevious;
        private readonly UIButton btnNext;
        private static Dictionary<int, int> itemList;

        private UIListButton activeItem;
        private UIActionListener listener;

        public UIList( int width, int height, Vector2 position, UIActionListener listener )
            : base( width, height, position )
        {
            MAX_VISIBLE_BUTTON_COUNT = height / DEFAULT_HEIGHT;
            itemList = new Dictionary<int, int>();
            buttonList = new List<UIElement>();
            countItemsDict = new Dictionary<int, int>();

            var imgPreviousButton = Main.ContentManager.Load<Texture2D>( "images/gui/inventar/list_previous" );
            var imgPreviousButtonHover = Main.ContentManager.Load<Texture2D>( "images/gui/inventar/list_previous_h" );

            var imgNextButton = Main.ContentManager.Load<Texture2D>( "images/gui/inventar/list_next" );
            var imgNextButtonHover = Main.ContentManager.Load<Texture2D>( "images/gui/inventar/list_next_h" );

            btnPrevious = new UIButton( width, imgPreviousButton.Height, new Vector2( 0, 0 ), imgPreviousButton, imgPreviousButtonHover, "" );
            btnNext = new UIButton( width, imgNextButton.Height, new Vector2( 0, height - imgNextButton.Height ), imgNextButton, imgNextButtonHover, "" );

            this.listener = listener;

            btnPrevious.AddActionListener( this );
            btnNext.AddActionListener( this );
        }

        public void SetItems( Dictionary<int, int> inventar )
        {
            ItemList.Clear();
            buttonList.Clear();
            
            foreach ( KeyValuePair<int, int> pair in inventar )
            {
                ItemList.Add( pair.Key, pair.Value );
            }
            GenerateButtons();
        }

        public void AddItemList( Dictionary<int, int> list )
        {
            foreach ( KeyValuePair<int, int> pair in list )
            {
                if (!ItemList.ContainsKey(pair.Key))
                    ItemList.Add( pair.Key, pair.Value );
            }
            GenerateButtons();
        }

        public void RemoveItems( Dictionary<int, int> list )
        {
            foreach ( KeyValuePair<int, int> pair in list )
            {
                if ( ItemList.ContainsKey( pair.Key ) )
                    ItemList.Remove( pair.Key );
            }
            GenerateButtons();
        }

        private void GenerateButtons()
        {
            buttonList.Clear();
            int i = 0;
            foreach ( int typeId in itemList.Keys )
            {
                Item item = Item.Get( typeId );
                var x = ( int ) ( position.X );
                var y = ( int ) ( ( DEFAULT_HEIGHT * i ) ) + btnPrevious.GetHeight();
                var button = new UIListButton( width, 24, new Vector2( 0, y ), item, itemList[ typeId ] );

                button.AddActionListener( this );
                button.AddActionListener( listener );
                buttonList.Add( button );
                i++;
            }
        }


        public void RemoveActiveItem()
        {
            if ( ItemList.ContainsKey( activeItem.Item.TypeId ) )
            {
                ItemList[ activeItem.Item.TypeId ] -= 1;
                activeItem.CountString = ItemList[activeItem.Item.TypeId]+"";
                if ( ItemList[ activeItem.Item.TypeId ] < 1 )
                {
                    ItemList.Remove( activeItem.Item.TypeId );
                    buttonList.Remove( activeItem );
                    activeItem = null;
                }
            }
            
        }

        public override void Draw( SpriteBatch sb )
        {
            Clear();
            //            box = new Rectangle((int)position.X, (int)position.Y, width, height);
            //            sb.Draw( Main.ContentManager.Load<Texture2D>( "images/pixelWhite" ), box, Color.Fuchsia );
            Add( btnPrevious );

            if ( buttonList.Count > 0 )
            {
                int j = 0;
                for ( int i = firsVisibleButtonIndex; ( i < MAX_VISIBLE_BUTTON_COUNT + firsVisibleButtonIndex ) && ( i < buttonList.Count ); i++, j++ )
                {

                    UIListButton button = ( UIListButton ) buttonList[ i ];
                    button.SetPosition( new Vector2( 0, j * DEFAULT_HEIGHT + 18 ) );
                    Add( buttonList[ i ] );
                }
            }

            Add( btnNext );
            base.Draw( sb );
        }


        public void AddActioinListener( UIActionListener l )
        {
            foreach ( UIListButton button in buttonList )
            {
                button.AddActionListener( l );
            }
        }



        public void OnMouseDown( UIElement element )
        {

            if ( element == btnPrevious && firsVisibleButtonIndex > 0 )
            {
                firsVisibleButtonIndex--;
            }

            if ( element == btnNext )
            {
                if ( ( firsVisibleButtonIndex + MAX_VISIBLE_BUTTON_COUNT ) < buttonList.Count )
                {
                    firsVisibleButtonIndex++;
                }
            }

            if ( element.GetType() == typeof( UIListButton ) )
            {

                foreach ( UIListButton i in buttonList )
                {
                    i.isActive = false;
                    if ( i == element )
                    {
                        i.isActive = true;
                    }
                }

                if ( activeItem != null )
                    activeItem.Color = Color.Gray;
                activeItem = ( UIListButton ) element;
                activeItem.Color = Color.Green;

            }
        }



        public void OnMouseUp( UIElement element )
        {
            // leer
        }

        
    }
}
