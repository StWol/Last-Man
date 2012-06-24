using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            set
            {
                if ( value && !isVisible )
                {
                    GenerateFilteredLists( player.Inventar );
                    GenerateUiComponents();
                    inventarList.SetItems( player.Inventar );
                }
                isVisible = value;
            }
        }

        private Texture2D backgroundTextur;
        private Rectangle background;
        private Color backgroundColor = Color.Gray;


        private UIToggleButton toggleWaffe;
        private UIToggleButton toggleHauptteil;
        private UIToggleButton toggleStabilisator;
        private UIToggleButton toggleMunition;
        private UIToggleButton toggleVisier;
        private UIToggleButton toggleAntrieb;
        private UIToggleButton togglePowerup;


        private Dictionary<int, int> listWaffe;
        private Dictionary<int, int> listHauptteil;
        private Dictionary<int, int> listStabilisator;
        private Dictionary<int, int> listMunition;
        private Dictionary<int, int> listVisier;
        private Dictionary<int, int> listAntrieb;
        private Dictionary<int, int> listPowerup;

        private Player player;


        private bool isVisible;
        private Item activeItem;
        private UIInfoPanel infoPanel;

        private UIButton btnOk;
        private UIButton btnCancel;

        private List<UIShortcutButton> shortcutButtons;

        private int lastFreeShortcutIndex;

        public UIInventarPanel( int width, int height, Vector2 position )
            : base( width, height, position )
        {
            player = Main.MainObject.GameManager.GameState.Player;

            Visible = false;

            background = new Rectangle( ( int ) GetPosition().X, ( int ) GetPosition().Y, width, height );
            backgroundTextur = Main.ContentManager.Load<Texture2D>( "images/gui/inventar/inventar_background" );

            shortcutButtons = new List<UIShortcutButton>();

            //GenerateUiComponents();
            GenerateFilteredLists( player.Inventar );
            inventarList = new UIList( 260, 236, new Vector2( 280, 60 ), this );
            inventarList.AddItemList( player.Inventar );
            Add( inventarList );

            CreateCheckBoxPanel();
            GenerateShortcuts();
        }


        private void GenerateShortcuts()
        {
            Dictionary<int, Weapon> shortcuts = player.GetShortcuts();

            for ( int i = 0; i < 4; i++ )
            {
                UIShortcutButton shortcutButton = new UIShortcutButton( 240, DEFAULT_HEIGHT, new Vector2( 20, 60 + DEFAULT_HEIGHT * i ), i + 1 );
                shortcutButton.AddActionListener( this );
                Add( shortcutButton );
                shortcutButtons.Add( shortcutButton );
            }


            foreach ( KeyValuePair<int,Weapon> pair in shortcuts )
            {
                shortcutButtons[ pair.Key - 1 ].Weapon = pair.Value;
                lastFreeShortcutIndex++;
            }
        }


        private void GenerateUiComponents()
        {
            var shortCutTitel = new UIButton( 240, 40, new Vector2( 20, 20 ), "Shortcuts" ) { BackgroundColor = Color.LightGray };
            var inventarTitel = new UIButton( 250, 40, new Vector2( 280, 20 ), "Inventar" ) { BackgroundColor = Color.LightGray };
            var filterTitel = new UIButton( 120, 40, new Vector2( 560, 20 ), "Filter" ) { BackgroundColor = Color.LightGray };



            var ok = Main.ContentManager.Load<Texture2D>( "images/gui/inventar/btn_ok" );
            var ok_h = Main.ContentManager.Load<Texture2D>( "images/gui/inventar/btn_ok_h" );
            var cancel = Main.ContentManager.Load<Texture2D>( "images/gui/inventar/btn_cancel" );
            var cancel_h = Main.ContentManager.Load<Texture2D>( "images/gui/inventar/btn_cancel_h" );

            btnOk = new UIButton( new Vector2( 300, 306 ), ok, ok_h );
            btnCancel = new UIButton( new Vector2( 350, 306 ), cancel, cancel_h );

            btnOk.IsEnabled = false;
            btnCancel.IsEnabled = false;

            btnOk.AddActionListener( this );
            btnCancel.AddActionListener( this );


            infoPanel = new UIInfoPanel( 240, 120, new Vector2( 20, 240 ) );
            Add( shortCutTitel );
            Add( inventarTitel );
            Add( filterTitel );

            //Add( infoPanel );

            Add( btnOk );
            Add( btnCancel );
        }

        private void GenerateFilteredLists( Dictionary<int, int> inventar )
        {
            listWaffe = new Dictionary<int, int>();
            listHauptteil = new Dictionary<int, int>();
            listStabilisator = new Dictionary<int, int>();
            listMunition = new Dictionary<int, int>();
            listVisier = new Dictionary<int, int>();
            listAntrieb = new Dictionary<int, int>();
            listPowerup = new Dictionary<int, int>();


            foreach ( KeyValuePair<int, int> pair in inventar )
            {
                int typeId = pair.Key;
                int count = pair.Value;

                Item item = Item.Get( typeId );

                var type = item.GetType();
                if ( type == typeof( Weapon ) )
                {
                    listWaffe[ typeId ] = count;
                }
                else if ( type == typeof( Hauptteil ) )
                {
                    listHauptteil[ typeId ] = count;
                }
                else if ( type == typeof( Stabilisator ) )
                {
                    listStabilisator[ typeId ] = count;
                }
                else if ( type == typeof( Munition ) )
                {
                    listMunition[ typeId ] = count;
                }
                else if ( type == typeof( Visier ) )
                {
                    listVisier[ typeId ] = count;
                }
                else if ( type == typeof( Antrieb ) )
                {
                    listAntrieb[ typeId ] = count;
                }
                else if ( type == typeof( Powerup ) )
                {
                    listPowerup[ typeId ] = count;
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

            var sonstiges = content.Load<Texture2D>( "images/gui/inventar/sonstiges" );
            var sonstigesH = content.Load<Texture2D>( "images/gui/inventar/sonstiges_h" );
            var sonstigesA = content.Load<Texture2D>( "images/gui/inventar/sonstiges_a" );
            var sonstigesAH = content.Load<Texture2D>( "images/gui/inventar/sonstiges_a_h" );

            toggleWaffe = new UIToggleButton( 120, 30, new Vector2( 560, 60 ), waffe, waffeH, waffeA, waffeAH, "" ) { isActive = true };
            toggleHauptteil = new UIToggleButton( 120, 30, new Vector2( 560, 100 ), hauptteil, hauptteilH, hauptteilA, hauptteilAH, "" ) { isActive = true };
            toggleStabilisator = new UIToggleButton( 120, 30, new Vector2( 560, 140 ), stabilisator, stabilisatorH, stabilisatorA, stabilisatorAH, "" ) { isActive = true };
            toggleMunition = new UIToggleButton( 120, 30, new Vector2( 560, 180 ), munition, munitionH, munitionA, munitionAH, "" ) { isActive = true };
            toggleVisier = new UIToggleButton( 120, 30, new Vector2( 560, 220 ), visier, visierH, visierA, visierAH, "" ) { isActive = true };
            toggleAntrieb = new UIToggleButton( 120, 30, new Vector2( 560, 260 ), antrieb, antriebH, antriebA, antriebAH, "" ) { isActive = true };
            togglePowerup = new UIToggleButton( 120, 30, new Vector2( 560, 300 ), sonstiges, sonstigesH, sonstigesA, sonstigesAH, "" ) { isActive = true };

            toggleWaffe.AddActionListener( this );
            toggleHauptteil.AddActionListener( this );
            toggleStabilisator.AddActionListener( this );
            toggleMunition.AddActionListener( this );
            toggleVisier.AddActionListener( this );
            toggleAntrieb.AddActionListener( this );
            togglePowerup.AddActionListener( this );

            Add( toggleWaffe );
            Add( toggleHauptteil );
            Add( toggleStabilisator );
            Add( toggleMunition );
            Add( toggleVisier );
            Add( toggleAntrieb );
            Add( togglePowerup );
        }

        public override void Draw( SpriteBatch sb )
        {
            sb.Draw( backgroundTextur, background, Color.White );
            base.Draw( sb );




            sb.DrawString( UIButton.FONT_DEFAULT, "Gewicht: " + player.GetTotalWeight(), new Vector2( GetPosition().X + 410, GetPosition().Y + 326 ), Color.Black );

            string group;
            string name;
            string desc;
            if ( activeItem != null )
            {

                group = activeItem.Group.ToString();
                name = activeItem.Name;
                desc = activeItem.Description;
            }
            else
            {
                group = "";
                name = "";
                desc = "";
            }
            int lineHeight = 30;
            int padding = 20;

            int x = ( int ) ( GetPosition().X + 20 );
            int y = ( int ) ( GetPosition().Y + 240 );

            sb.DrawString( UIButton.FONT_DEFAULT, name, new Vector2( padding + x, padding + y ), Color.Black );
            sb.DrawString( UIButton.FONT_DEFAULT, desc, new Vector2( padding + x, padding + y + lineHeight ), Color.Black );
            sb.DrawString( UIButton.FONT_DEFAULT, group, new Vector2( padding + x, padding + y + lineHeight * 2 ), Color.Black );
        }

        public void OnMouseDown( UIElement element )
        {
            //////////////////////////////////////////////////////
            // ToggleButtons
            if ( element.GetType() == typeof( UIToggleButton ) )
            {
                inventarList.FirsVisibleButtonIndex = 0;
            }


            if ( element == toggleWaffe )
            {
                inventarList.AddItemList( listWaffe );
            }
            else if ( element == toggleHauptteil )
            {
                inventarList.AddItemList( listHauptteil );
            }
            else if ( element == toggleMunition )
            {
                inventarList.AddItemList( listMunition );
            }
            else if ( element == toggleStabilisator )
            {
                inventarList.AddItemList( listStabilisator );
            }
            else if ( element == toggleVisier )
            {
                inventarList.AddItemList( listVisier );
            }
            else if ( element == toggleAntrieb )
            {
                inventarList.AddItemList( listAntrieb );
            }
            else if ( element == togglePowerup )
            {
                inventarList.AddItemList( listPowerup );
            }


            //////////////////////////////////////////////////////
            // ListButtons
            if ( element.GetType() == typeof( UIListButton ) )
            {

                activeItem = ( ( UIListButton ) element ).Item;

                infoPanel.Item = activeItem;

                if ( activeItem.GetType() == typeof( Powerup ) )
                {
                    //aus der Liste in die Shortcuts
                    btnOk.IsEnabled = true;
                }
                else
                {
                    btnOk.IsEnabled = false;
                }
                btnCancel.IsEnabled = true;
            }

            //////////////////////////////////////////////////////
            // OK & Cankel Buttons

            if ( activeItem != null )
                if ( element == btnOk )
                {
                    if ( activeItem.GetType() == typeof( Weapon ) )
                    {
                        if ( lastFreeShortcutIndex < 4 )
                        {
                            player.AddWeaponToShortcutList( lastFreeShortcutIndex + 1, ( Weapon ) activeItem );
                            GenerateFilteredLists( player.Inventar );
                            shortcutButtons[ lastFreeShortcutIndex ].Weapon = ( Weapon ) activeItem;
                            lastFreeShortcutIndex++;
                        }
                    }
                }
                else if ( element == btnCancel )
                {
                    //activeItem.LocationBehavior.Position = player.LocationBehavior.Position;
                    //activeItem.LocationSizing();
                    //Main.MainObject.GameManager.GameState.QuadTreeItems.Add( activeItem );


                    inventarList.RemoveActiveItem();
                    activeItem = player.RemoveItemFromInventar( activeItem );
                    GenerateFilteredLists( player.Inventar );
                    infoPanel.Item = activeItem;
                }

            //////////////////////////////////////////////////////
            // Shortcut buttons


            if ( element.GetType() == typeof( UIShortcutButton ) )
            {
                UIShortcutButton button = ( UIShortcutButton ) element;
                
                
                if (activeItem.GetType() == typeof(Munition) && button.Weapon != null)
                {
                    

                    
                    Munition mun = (Munition) activeItem;
                    Munition oldMun = button.Weapon.Munition;
                    int diff = mun.MagazineSize;

                    Munition newMun = mun.Clone();

                    if (oldMun != null )
                    {    
                        if(mun.TypeId == oldMun.TypeId)
                        {
                            button.Weapon.Reload();
                        }
                        else 
                        {
                            player.AddItemToInventar(oldMun);
                            //newMun.Count = player.Inventar[mun.TypeId];
                            button.Weapon.Munition = newMun;
                            button.Weapon.Reload();
                            player.RemoveItemFromInventar(newMun);
                        }
                    }
                    else
                    {
                        
                    }

                    

                    //button.Weapon.Munition = newMun;

                    
                    activeItem = null;
                }
                else if ( activeItem == null || activeItem.GetType() != typeof( Weapon ) )
                {
                    player.RemoveWeaponFromShortcutList( button.Key );
                    button.Weapon = null;
                }
                else if (activeItem.GetType() == typeof(Weapon))
                {
                    player.AddWeaponToShortcutList(lastFreeShortcutIndex + 1, (Weapon)activeItem);
                    button.Weapon = (Weapon)activeItem;
                    activeItem = null;
                }

                btnOk.IsEnabled = false;
                btnCancel.IsEnabled = false;
            }
        }

        public void OnMouseUp( UIElement element )
        {
            inventarList.FirsVisibleButtonIndex = 0;
            if ( element == toggleWaffe )
            {
                inventarList.RemoveItems( listWaffe );
            }
            else if ( element == toggleHauptteil )
            {
                inventarList.RemoveItems( listHauptteil );
            }
            else if ( element == toggleMunition )
            {
                inventarList.RemoveItems( listMunition );
            }
            else if ( element == toggleStabilisator )
            {
                inventarList.RemoveItems( listStabilisator );
            }
            else if ( element == toggleVisier )
            {
                inventarList.RemoveItems( listVisier );
            }
            else if ( element == toggleAntrieb )
            {
                inventarList.RemoveItems( listAntrieb );
            }
            else if ( element == togglePowerup )
            {
                inventarList.RemoveItems( listPowerup );
            }
        }

        public void OnMouseIn( UIElement element )
        {
            throw new NotImplementedException();
        }

        public void OnMouseOut( UIElement element )
        {
            throw new NotImplementedException();
        }
    }
}
