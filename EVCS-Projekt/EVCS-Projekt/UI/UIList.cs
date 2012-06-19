using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using EVCS_Projekt.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EVCS_Projekt.UI
{
    class UIList : UIPanel, UIActionListener
    {
        private readonly List<UIElement> buttonList;
        private readonly List<Item> itemList;
        private readonly Dictionary<int, int> countItemsDict;
        private int firsVisibleButtonIndex = 0;
        private const int MAX_VISIBLE_BUTTON_COUNT = 5;

        private Rectangle box;

        private readonly UIButton btnPrevious;
        private readonly UIButton btnNext;

        public UIList( int width, int height, Vector2 position, List<Item> itemList )
            : base( width, height, position )
        {
            this.itemList = itemList;
            buttonList = new List<UIElement>();
            countItemsDict = new Dictionary<int, int>();
            box = new Rectangle((int)position.X, (int)position.Y, width, height);
            CountItems();

            var imgPreviousButton = Main.ContentManager.Load<Texture2D>( "images/gui/list_previous" );
            var imgPreviousButtonHover = Main.ContentManager.Load<Texture2D>( "images/gui/list_previous_hover" );

            var imgNextButton = Main.ContentManager.Load<Texture2D>( "images/gui/list_next" );
            var imgNextButtonHover = Main.ContentManager.Load<Texture2D>( "images/gui/list_next_hover" );

            btnPrevious = new UIButton( new Vector2(0,0), imgPreviousButton, imgPreviousButtonHover );
            btnNext = new UIButton( new Vector2(0, height - imgNextButton.Height ), imgNextButton, imgNextButtonHover );

            

            btnPrevious.AddActionListener( this );
            btnNext.AddActionListener( this );


            GenerateButtons();

        }

        private void CountItems()
        {
            foreach ( var item in itemList )
            {
                if ( !countItemsDict.ContainsKey( item.Id ) )
                    countItemsDict.Add( item.Id, 0 );
                else
                    countItemsDict[ item.Id ] += 1;
            }
        }

        private void GenerateButtons()
        {
            for ( int i = 0; i < itemList.Count; i++ )
            {
                var item = itemList[ i ];
                var x = ( int ) ( position.X );
                var y = (int)(position.Y + (50 * i)) + btnPrevious.GetHeight();
                var button = new UIListButton(width, 24, new Vector2(x, y), item.Icon, item.Name, countItemsDict[item.Id], item.Weight);

                buttonList.Add( button );
            }
        }

        public override void Draw( SpriteBatch sb )
        {
            Clear();
            sb.Draw( Main.ContentManager.Load<Texture2D>( "images/pixelWhite" ), box, Color.Fuchsia );
            Add( btnPrevious );
            Debug.WriteLine("Position  x:" + btnPrevious.GetPosition().X + " y:" + btnPrevious.GetPosition().Y);
            for ( int i = firsVisibleButtonIndex; i < MAX_VISIBLE_BUTTON_COUNT + firsVisibleButtonIndex; i++ )
            {
                Add( buttonList[ i ] );
                
            }
            Add( btnNext );
            base.Draw( sb );
        }


        /// <summary>
        /// Ein Button fuer die Liste
        /// </summary>
        private class UIListButton : UIPanel
        {
            private readonly UIButton btnIcon;
            private readonly UIButton btnName;
            private readonly UIButton btnItemCount;
            private readonly UIButton btnWeightIcon;
            private readonly UIButton btnWeightCount;
            private const int DEFAULT_HEIGHT = 48;

            public UIListButton( int width, int height, Vector2 position, Texture2D itemIcon, string name, int count, float weigth )
                : base(width, DEFAULT_HEIGHT, position)
            {
                var weightIcon = Main.ContentManager.Load<Texture2D>("images/gui/weight");

                this.btnIcon = new UIButton(DEFAULT_HEIGHT,DEFAULT_HEIGHT, new Vector2( 0, 0 ), itemIcon, itemIcon );
                this.btnName = new UIButton(width - DEFAULT_HEIGHT*4, DEFAULT_HEIGHT, new Vector2(DEFAULT_HEIGHT, 0), name);
                this.btnItemCount = new UIButton(DEFAULT_HEIGHT, DEFAULT_HEIGHT, new Vector2(width - DEFAULT_HEIGHT * 3, 0), count + "");
                this.btnWeightIcon = new UIButton(DEFAULT_HEIGHT, DEFAULT_HEIGHT, new Vector2(width - DEFAULT_HEIGHT * 2, 0), weightIcon, weightIcon);
                this.btnWeightCount = new UIButton(DEFAULT_HEIGHT, DEFAULT_HEIGHT, new Vector2(width - DEFAULT_HEIGHT, 0), weigth.ToString());

                btnIcon.BackgroundColor = Color.Peru;
                btnName.BackgroundColor = Color.Salmon;
                btnItemCount.BackgroundColor = Color.Green;
                btnWeightIcon.BackgroundColor = Color.RoyalBlue;
                btnWeightCount.BackgroundColor = Color.White;

                Add( this.btnIcon );
                Add( this.btnName );
                Add( this.btnItemCount );
                Add( this.btnWeightIcon );
                Add( this.btnWeightCount );
            }
        }

        public void ActionEvent( UIElement element )
        {
            Debug.WriteLine("Action");
            if ( element == btnPrevious && firsVisibleButtonIndex > 0 )
            {
                firsVisibleButtonIndex--;
                Debug.WriteLine("Action");
            }
            else if ( element == btnNext )
            {
                firsVisibleButtonIndex++;
            }
        }
    }
}
