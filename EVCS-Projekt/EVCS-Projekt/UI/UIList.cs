using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Text;
using LastMan.GUI;
using LastMan.Objects;
using LastMan.Objects.Items;
using LastMan;
using LastMan.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LastMan.UI
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

        private UIListButton activeItemButton;
        private UIActionListener listener;

        private Player player;


        public UIList( int width, int height, Vector2 position, UIActionListener listener )
            : base( width, height, position )
        {
            
            itemList = new Dictionary<int, int>();
            buttonList = new List<UIElement>();
            countItemsDict = new Dictionary<int, int>();

            unscaledWidth = width;
            unscaledHeight = height;
            unscaledPos = position;

            var imgPreviousButton = Main.ContentManager.Load<Texture2D>( "images/gui/inventar/list_previous" );
            var imgPreviousButtonHover = Main.ContentManager.Load<Texture2D>( "images/gui/inventar/list_previous_h" );

            var imgNextButton = Main.ContentManager.Load<Texture2D>( "images/gui/inventar/list_next" );
            var imgNextButtonHover = Main.ContentManager.Load<Texture2D>( "images/gui/inventar/list_next_h" );

            btnPrevious = new UIButton(unscaledWidth, imgPreviousButton.Height, new Vector2(0, 0), imgPreviousButton, imgPreviousButtonHover, "");
            btnNext = new UIButton(unscaledWidth, imgNextButton.Height, new Vector2(0, height - imgNextButton.Height), imgNextButton, imgNextButtonHover, "");

            this.listener = listener;

            int listHeight = height - imgNextButton.Height*2;
            MAX_VISIBLE_BUTTON_COUNT = (listHeight - listHeight % DEFAULT_HEIGHT) / DEFAULT_HEIGHT;

            player = Main.MainObject.GameManager.GameState.Player;

            btnPrevious.AddActionListener( this );
            btnNext.AddActionListener( this );
        }


        public void RefreshActivItem()
        {
            Item item = activeItemButton.Item;
            int itemCount = player.GetItemCountFromInventar(item.TypeId);

            if (itemCount > 0)
                activeItemButton.CountString = itemCount + "";
            else
            {
                RemoveActiveItem();
            }


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

     
        public void AddItem(Item item)
        {
            if(!itemList.ContainsKey(item.TypeId))
            {
                itemList[item.TypeId] = 1;
            }
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
                AddButton(item);

                //var y = ( int ) ( ( DEFAULT_HEIGHT * i ) ) + btnPrevious.GetHeight();
                //var button = new UIListButton( width, 24, new Vector2( 0, y ), item, itemList[ typeId ] );

                //button.AddActionListener( this );
                //button.AddActionListener( listener );
                //buttonList.Add( button );
                //i++;
            }
        }


        public void RefreshItemList()
        {
            Dictionary<int, int> temp = new Dictionary<int, int>(ItemList);
            foreach (KeyValuePair<int, int> pair in ItemList)
            {
                if (player.GetItemCountFromInventar(pair.Key) <= 0)
                    temp.Remove(pair.Key);
            }

            ItemList.Clear();
            foreach (KeyValuePair<int, int> pair in player.Inventar)
            {
                if (temp.ContainsKey(pair.Key))
                    ItemList[pair.Key] = pair.Value ;
            }
            GenerateButtons();
        }


        private void AddButton(Item item)
        {
            var y = (int)((DEFAULT_HEIGHT * buttonList.Count)) + btnPrevious.GetHeight();
            var button = new UIListButton(unscaledWidth, 24, new Vector2(0, y), item, player.GetItemCountFromInventar(item.TypeId));

            button.AddActionListener(this);
            button.AddActionListener(listener);
            buttonList.Add(button);
        }

        public void RemoveActiveItem()
        {
            if ( ItemList.ContainsKey( activeItemButton.Item.TypeId ) )
            {
                int anzahl = 1;
                if(activeItemButton.Item.GetType() == typeof(Munition))
                {
                    anzahl = Math.Min(ItemList[activeItemButton.Item.TypeId], ((Munition) activeItemButton.Item).MagazineSize);
                }
                ItemList[activeItemButton.Item.TypeId] -= anzahl;
                activeItemButton.CountString = ItemList[activeItemButton.Item.TypeId]+"";
                if ( ItemList[ activeItemButton.Item.TypeId ] < 1 )
                {
                    ItemList.Remove( activeItemButton.Item.TypeId );
                    buttonList.Remove( activeItemButton );
                    activeItemButton = null;
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

                if ( activeItemButton != null )
                    activeItemButton.Color = Color.Gray;
                activeItemButton = ( UIListButton ) element;
                activeItemButton.Color = Color.Green;
            }
        }



        public void OnMouseUp( UIElement element )
        {
            // leer
        }

        
    }
}
