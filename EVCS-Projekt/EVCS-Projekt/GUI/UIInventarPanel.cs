using System;
using System.Collections.Generic;
using EVCS_Projekt.Objects;
using EVCS_Projekt.Objects.Items;
using EVCS_Projekt.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EVCS_Projekt.GUI
{
    class UIInventarPanel : UIPanel, UIActionListener, UIMouseHoverListener
    {
        private readonly UIList inventarList;
        private readonly Texture2D pixel;
        public bool Visible
        {
            get { return isVisible; }
            set { 
                if(value && !isVisible)
                {
                    playerInventar = Main.MainObject.GameManager.GameState.Player.Inventar;
                    GenerateFilteredLists(playerInventar);
                    GenerateUiComponents();
                }
                isVisible = value;
            }
        }

        private Rectangle background;
        private Color backgroundColor = Color.Gray;
        private Dictionary<int,int> playerInventar;

        private UIToggleButton toggleWaffe;
        private UIToggleButton toggleHauptteil;
        private UIToggleButton toggleStabilisator;
        private UIToggleButton toggleMunition;
        private UIToggleButton toggleVisier;
        private UIToggleButton toggleAntrieb;


        private Dictionary<int, int> listWaffe;
        private Dictionary<int, int> listHauptteil;
        private Dictionary<int, int> listStabilisator;
        private Dictionary<int, int> listMunition;
        private Dictionary<int, int> listVisier;
        private Dictionary<int, int> listAntrieb;

        private Player player;
        

        private bool isVisible;


        public UIInventarPanel( int width, int height, Vector2 position )
            : base( width, height, position )
        {
            player = Main.MainObject.GameManager.GameState.Player;
            playerInventar = player.Inventar;
            inventarList = new UIList( 380, 250, new Vector2( 300, 100 ));

            Add( inventarList );
            Visible = false;

            if ( pixel == null )
                pixel = Main.ContentManager.Load<Texture2D>( "images/pixelWhite" );

            background = new Rectangle( ( int ) GetPosition().X, ( int ) GetPosition().Y, width, height );
            Add( inventarList );

            //GenerateUiComponents();
            GenerateFilteredLists(playerInventar);
            CreateCheckBoxPanel();
        }

        private void GenerateUiComponents()
        {
            UIButton shortCutTitel = new UIButton(260, 40, new Vector2(20, 20), "Shortcuts"){BackgroundColor = Color.LightGray};

            Dictionary<int, Weapon> shortcuts = player.GetShortcuts();
            Weapon w = player.Weapon;
            UIShortCutButton s = new UIShortCutButton(260, 50, new Vector2(20, 70), 1, w);
            Add(shortCutTitel);
            Add(s);
        }
        
        private void GenerateFilteredLists(Dictionary<int, int> inventar)
        {
            listWaffe = new Dictionary<int, int>();
            listHauptteil = new Dictionary<int, int>();
            listStabilisator = new Dictionary<int, int>();
            listMunition = new Dictionary<int, int>();
            listVisier = new Dictionary<int, int>();
            listAntrieb = new Dictionary<int, int>();


            foreach (KeyValuePair<int, int> pair in inventar)
            {
                int typeId = pair.Key;
                int count = pair.Value;

                Item item = Item.Get(typeId);

                var type = item.GetType();
                if (type == typeof(Weapon))
                {
                    listWaffe[typeId] = count;
                }else if(type == typeof(Hauptteil))
                {
                    listHauptteil[typeId] = count;
                }
                else if (type == typeof(Stabilisator))
                {
                    listStabilisator[typeId] = count;
                }
                else if (type == typeof(Munition))
                {
                    listMunition[typeId] = count;
                }
                else if (type == typeof(Visier))
                {
                    listVisier[typeId] = count;
                }
                else if (type == typeof(Antrieb))
                {
                    listAntrieb[typeId] = count;
                }
            }
        }

        private void CreateCheckBoxPanel()
        {
            ContentManager content = Main.ContentManager;

            var waffe = content.Load<Texture2D>( "images/gui/inventar/waffe" );
            var waffeH = content.Load<Texture2D>( "images/gui/inventar/waffe_h" );
            var waffeA = content.Load<Texture2D>( "images/gui/inventar/waffe_a" );
            var waffeAH = content.Load<Texture2D>( "images/gui/inventar/waffe_a_h" );

            var hauptteil = content.Load<Texture2D>( "images/gui/inventar/hauptteil" );
            var hauptteilH = content.Load<Texture2D>( "images/gui/inventar/hauptteil_h" );
            var hauptteilA = content.Load<Texture2D>( "images/gui/inventar/hauptteil_a" );
            var hauptteilAH = content.Load<Texture2D>( "images/gui/inventar/hauptteil_a_h" );

            var stabilisator = content.Load<Texture2D>( "images/gui/inventar/stabilisator" );
            var stabilisatorH = content.Load<Texture2D>( "images/gui/inventar/stabilisator_h" );
            var stabilisatorA = content.Load<Texture2D>( "images/gui/inventar/stabilisator_a" );
            var stabilisatorAH = content.Load<Texture2D>( "images/gui/inventar/stabilisator_a_h" );

            var munition = content.Load<Texture2D>( "images/gui/inventar/munition" );
            var munitionH = content.Load<Texture2D>( "images/gui/inventar/munition_h" );
            var munitionA = content.Load<Texture2D>( "images/gui/inventar/munition_a" );
            var munitionAH = content.Load<Texture2D>( "images/gui/inventar/munition_a_h" );

            var visier = content.Load<Texture2D>( "images/gui/inventar/visier" );
            var visierH = content.Load<Texture2D>( "images/gui/inventar/visier_h" );
            var visierA = content.Load<Texture2D>( "images/gui/inventar/visier_a" );
            var visierAH = content.Load<Texture2D>( "images/gui/inventar/visier_a_h" );
            
            var antrieb = content.Load<Texture2D>( "images/gui/inventar/antrieb" );
            var antriebH = content.Load<Texture2D>( "images/gui/inventar/antrieb_h" );
            var antriebA = content.Load<Texture2D>( "images/gui/inventar/antrieb_a" );
            var antriebAH = content.Load<Texture2D>( "images/gui/inventar/antrieb_a_h" );

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
            inventarList.FirsVisibleButtonIndex = 0;
            if (element == toggleWaffe)
            {
                inventarList.AddItemList(listWaffe);
            }
            else if (element == toggleHauptteil)
            {
                inventarList.AddItemList(listHauptteil);
            }
            else if (element == toggleMunition)
            {
                inventarList.AddItemList(listMunition);
            }
            else if (element == toggleStabilisator)
            {
                inventarList.AddItemList(listStabilisator);
            }
            else if (element == toggleVisier)
            {
                inventarList.AddItemList(listVisier);
            }
            else if (element == toggleAntrieb)
            {
                inventarList.AddItemList(listAntrieb);
            }
        }

        public void OnMouseUp(UIElement element)
        {
            inventarList.FirsVisibleButtonIndex = 0;
            if (element == toggleWaffe)
            {
                inventarList.RemoveItems(listWaffe);
            }
            else if (element == toggleHauptteil)
            {
                inventarList.RemoveItems(listHauptteil);
            }
            else if (element == toggleMunition)
            {
                inventarList.RemoveItems(listMunition);
            }
            else if (element == toggleStabilisator)
            {
                inventarList.RemoveItems(listStabilisator);
            }
            else if (element == toggleVisier)
            {
                inventarList.RemoveItems(listVisier);
            }
            else if (element == toggleAntrieb)
            {
                inventarList.RemoveItems(listAntrieb);
            }
        }

        public void OnMouseIn(UIElement element)
        {
            throw new NotImplementedException();
        }

        public void OnMouseOut(UIElement element)
        {
            throw new NotImplementedException();
        }
    }
}
