using System;
using System.Collections.Generic;
using EVCS_Projekt.Objects;
using EVCS_Projekt.Objects.Items;
using EVCS_Projekt.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EVCS_Projekt.GUI
{
    class UIInventarPanel : UIPanel, UIActionListener, UIMouseHoverListener
    {
        public bool Visible
        {
            get { return isVisible; }
            set
            {
                if ( value && !isVisible )
                {
                    filteredInventarList.GenerateFilteredLists( player.Inventar );
                    filteredInventarList.SetItems( player.Inventar );
                    filteredInventarList.ResetToggleButtons();
                    activeItem = null;
                }
                isVisible = value;
            }
        }



        private readonly Texture2D backgroundTextur;
        private readonly Rectangle background;

        private readonly Player player;

        private bool isVisible;

        private UIButton btnOk;
        private UIButton btnCancel;

// ReSharper disable FieldCanBeMadeReadOnly.Local
        private List<UIShortcutButton> shortcutButtons;
// ReSharper restore FieldCanBeMadeReadOnly.Local

        private UIFilteredInventarList filteredInventarList;
        private Item activeItem;


        private readonly int textX;
        private readonly int textY;
        private const int TEXT_LINE_HEIGHT = 30;
        private const int TEXT_PADDING = 20;

        public UIInventarPanel( int width, int height, Vector2 position )
            : base( width, height, position )
        {

            player = Main.MainObject.GameManager.GameState.Player;

            Visible = false;

            background = new Rectangle( ( int ) GetPosition().X, ( int ) GetPosition().Y, base.width, base.height );
            backgroundTextur = Main.ContentManager.Load<Texture2D>( "images/gui/inventar/inventar_background" );

            shortcutButtons = new List<UIShortcutButton>();

            Helper.DrawHelper.AddDimension( "TextLinePadding", 20, 20 );

            int textLinePadding = ( int ) Helper.DrawHelper.Get( "TextLinePadding" ).X;

            textX = ( int ) ( GetPosition().X + textLinePadding );
            textY = GetHeight() - textLinePadding * 3;

            GenerateUiComponents();
            GenerateShortcuts();
        }


        private void GenerateShortcuts()
        {
            Dictionary<int, Weapon> shortcuts = player.GetShortcuts();

            for ( int i = 0; i < 4; i++ )
            {
                UIShortcutButton shortcutButton = new UIShortcutButton( 300, DEFAULT_HEIGHT, new Vector2( 20, 60 + DEFAULT_HEIGHT * i ), i + 1 );
                shortcutButton.AddActionListener( this );
                Add( shortcutButton );
                shortcutButtons.Add( shortcutButton );
            }


            foreach ( KeyValuePair<int,Weapon> pair in shortcuts )
            {
                shortcutButtons[ pair.Key - 1 ].Weapon = pair.Value;
            }
        }


        private void GenerateUiComponents()
        {
            var shortCutTitel = new UIButton( 240, 40, new Vector2( 20, 20 ), "Shortcuts" ) { BackgroundColor = Color.LightGray };
            var inventarTitel = new UIButton( 250, 40, new Vector2( 340, 20 ), "Inventar" ) { BackgroundColor = Color.LightGray };
            var filterTitel = new UIButton( 120, 40, new Vector2( 620, 20 ), "Filter" ) { BackgroundColor = Color.LightGray };

            var ok = Main.ContentManager.Load<Texture2D>( "images/gui/inventar/btn_ok" );
            var ok_h = Main.ContentManager.Load<Texture2D>( "images/gui/inventar/btn_ok_h" );
            var cancel = Main.ContentManager.Load<Texture2D>( "images/gui/inventar/btn_cancel" );
            var cancel_h = Main.ContentManager.Load<Texture2D>( "images/gui/inventar/btn_cancel_h" );


            filteredInventarList = new UIFilteredInventarList( 260, 236, new Vector2( 340, 60 ), this );
            Add( filteredInventarList );


            btnOk = new UIButton( new Vector2( 300, 306 ), ok, ok_h );
            btnCancel = new UIButton( new Vector2( 350, 306 ), cancel, cancel_h );

            btnOk.IsEnabled = false;
            btnCancel.IsEnabled = false;

            btnOk.AddActionListener( this );
            btnCancel.AddActionListener( this );


            Add( shortCutTitel );
            Add( inventarTitel );
            Add( filterTitel );

            Add( btnOk );
            Add( btnCancel );
        }


        public override void Draw( SpriteBatch sb )
        {
            sb.Draw( backgroundTextur, background, Color.White );
            base.Draw( sb );

            sb.DrawString( UIButton.FONT_DEFAULT, "Gewicht: " + player.TotalInventarWeight, new Vector2( GetPosition().X + 410, GetPosition().Y + 326 ), Color.Black );
            DrawInfoPanel( sb );

        }

        private void DrawInfoPanel( SpriteBatch sb )
        {
            if ( activeItem != null )
                if ( activeItem.GetType() == typeof( Antrieb ) )
                {
                    DrawAntrieb( sb, ( Antrieb ) activeItem );
                }
                else if ( activeItem.GetType() == typeof( Hauptteil ) )
                {
                    DrawHauptteil( sb, ( Hauptteil ) activeItem );
                }
                else if ( activeItem.GetType() == typeof( Munition ) )
                {
                    DrawMun( sb, ( Munition ) activeItem );
                }
                else if ( activeItem.GetType() == typeof( Powerup ) )
                {
                    DrawPowerup( sb, ( Powerup ) activeItem );
                }
                else if ( activeItem.GetType() == typeof( Stabilisator ) )
                {
                    DrawStabilisator( sb, ( Stabilisator ) activeItem );
                }
                else if ( activeItem.GetType() == typeof( Visier ) )
                {
                    DrawVisier( sb, ( Visier ) activeItem );
                }
                else if ( activeItem.GetType() == typeof( Weapon ) )
                {
                    DrawWeapon( sb, ( Weapon ) activeItem );
                }
        }

        public void OnMouseDown( UIElement element )
        {

            //////////////////////////////////////////////////////
            // ListButtons
            if ( element.GetType() == typeof( UIListButton ) )
            {
                activeItem = ( ( UIListButton ) element ).Item;

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
                    if ( activeItem.GetType() == typeof( Powerup ) )
                    {
                        //Fressen activeItem
                        //Fressen activeItem
                        //Fressen activeItem
                        //Fressen activeItem
                    }
                }
                else if ( element == btnCancel )
                {
                    Item dropedItem = Item.Get( activeItem.TypeId );
                    dropedItem.LocationBehavior.Position = player.LocationBehavior.Position;
                    dropedItem.LocationSizing();
                    if ( dropedItem.GetType() == typeof( Munition ) )
                    {
                        ( ( Munition ) dropedItem ).Count = player.Inventar[ activeItem.TypeId ];
                    }
                    Main.MainObject.GameManager.GameState.QuadTreeItems.Add( dropedItem );


                    filteredInventarList.RemoveActiveItem();
                    activeItem = player.RemoveItemFromInventar( activeItem );
                    filteredInventarList.GenerateFilteredLists( player.Inventar );
                }

            //////////////////////////////////////////////////////
            // Shortcut buttons


            if ( element.GetType() == typeof( UIShortcutButton ) )
            {
                HandleShortcutButtonClick( ( UIShortcutButton ) element );
            }


            if ( activeItem == null )
            {

            }
        }


        private void SwitchWeapon( Weapon newWeapon, UIShortcutButton button )
        {
            Weapon oldWeapon = button.Weapon;


            if ( oldWeapon != null )
            {
                if ( oldWeapon.Munition != null )
                {
                    // munition ins Inventar
                    player.AddItemToInventar( oldWeapon.Munition );
                    filteredInventarList.AddItem( oldWeapon.Munition );
                }

                //Waffe ins inventar
                player.RemoveWeaponFromShortcutList( button.Key );
                player.AddItemToInventar( oldWeapon );
                filteredInventarList.AddItem( oldWeapon );
                button.Weapon = null;
            }
            if ( newWeapon != null )
            {
                //Waffe ändern


                player.AddWeaponToShortcutList( button.Key, newWeapon );
                player.RemoveItemFromInventar( newWeapon );
                button.Weapon = newWeapon;

            }

        }

        private void SwitchMun( Munition newMunition, UIShortcutButton button )
        {
            Weapon oldWeapon = button.Weapon;

            if ( oldWeapon != null )
            {
                if ( oldWeapon.Munition != null )
                {
                    Munition oldMun = button.Weapon.Munition;
                    Munition newMun = newMunition.Clone();

                    if ( newMun.TypeId == oldMun.TypeId )
                    {
                        button.Weapon.Reload();
                    }
                    else
                    {
                        //mun setzten
                        player.AddItemToInventar( oldMun );
                        filteredInventarList.AddItem( oldMun );

                        newMun.Count = Math.Min( player.Inventar[ newMun.TypeId ], newMun.MagazineSize );

                        button.Weapon.Munition = newMun;
                        player.RemoveItemFromInventar( newMun );
                    }

                }
                else
                {
                    //mun ändern
                    // Wenn die Waffe, die am Shortcut "klebt" auch munition hat
                    newMunition.Count = Math.Min( player.Inventar[ newMunition.TypeId ], newMunition.MagazineSize );
                    button.Weapon.Munition = newMunition;
                    player.RemoveItemFromInventar( newMunition );
                }
            }
        }


        private void HandleShortcutButtonClick( UIShortcutButton element )
        {
            UIShortcutButton button = element;

            if ( activeItem == null )
            {
                if ( player.GetShortcuts().Values.Count > 1 )
                {
                    SwitchWeapon( null, button );
                }
            }
            else if ( activeItem.GetType() == typeof( Weapon ) )
            {
                SwitchWeapon( ( Weapon ) activeItem, button );
            }
            else if ( activeItem.GetType() == typeof( Munition ) )
            {
                SwitchMun( ( Munition ) activeItem, button );
            }
            else
            {
                if ( player.GetShortcuts().Values.Count > 1 )
                {
                    SwitchWeapon( null, button );
                }
            }
            activeItem = null;

            filteredInventarList.RefreshItemList();

            btnOk.IsEnabled = false;
            btnCancel.IsEnabled = false;
        }

        public void OnMouseUp( UIElement element )
        {
            throw new NotImplementedException();
        }

        public void OnMouseIn( UIElement element )
        {
            throw new NotImplementedException();
        }

        public void OnMouseOut( UIElement element )
        {
            throw new NotImplementedException();
        }



        private void DrawMun( Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Munition mun )
        {
            sb.DrawString( UIButton.FONT_DEFAULT, mun.Name, new Vector2( TEXT_PADDING + textX, TEXT_PADDING + textY ), Color.Black );
            sb.DrawString( UIButton.FONT_DEFAULT, "Schaden: " + mun.Damage, new Vector2( TEXT_PADDING + textX, TEXT_PADDING + textY + TEXT_LINE_HEIGHT * 2 ), Color.Black );
            sb.DrawString( UIButton.FONT_DEFAULT, "Magazin: " + mun.MagazineSize, new Vector2( TEXT_PADDING + textX, TEXT_PADDING + textY + TEXT_LINE_HEIGHT * 3 ), Color.Black );
            sb.DrawString( UIButton.FONT_DEFAULT, "Gewicht: " + mun.Weight, new Vector2( TEXT_PADDING + textX, TEXT_PADDING + textY + TEXT_LINE_HEIGHT * 4 ), Color.Black );
        }

        private void DrawWeapon( Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Weapon weapon )
        {
            sb.DrawString( UIButton.FONT_DEFAULT, weapon.Name, new Vector2( TEXT_PADDING + textX, TEXT_PADDING + textY ), Color.Black );
            sb.DrawString( UIButton.FONT_DEFAULT, "Schaden: " + weapon.Damage, new Vector2( TEXT_PADDING + textX, TEXT_PADDING + textY + TEXT_LINE_HEIGHT * 2 ), Color.Black );
            sb.DrawString( UIButton.FONT_DEFAULT, "Feuerrate: " + weapon.RateOfFire, new Vector2( TEXT_PADDING + textX, TEXT_PADDING + textY + TEXT_LINE_HEIGHT * 3 ), Color.Black );
            sb.DrawString( UIButton.FONT_DEFAULT, "Accuracy: " + weapon.Accuracy, new Vector2( TEXT_PADDING + textX, TEXT_PADDING + textY + TEXT_LINE_HEIGHT * 4 ), Color.Black );
            sb.DrawString( UIButton.FONT_DEFAULT, "Gewicht: " + weapon.Weight, new Vector2( TEXT_PADDING + textX, TEXT_PADDING + textY + TEXT_LINE_HEIGHT * 5 ), Color.Black );
        }

        private void DrawVisier( Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Visier visier )
        {
            sb.DrawString( UIButton.FONT_DEFAULT, visier.Name, new Vector2( TEXT_PADDING + textX, TEXT_PADDING + textY ), Color.Black );
            sb.DrawString( UIButton.FONT_DEFAULT, "Accuracy: " + visier.Accuracy, new Vector2( TEXT_PADDING + textX, TEXT_PADDING + textY + TEXT_LINE_HEIGHT * 2 ), Color.Black );
            sb.DrawString( UIButton.FONT_DEFAULT, "Gewicht: " + visier.Weight, new Vector2( TEXT_PADDING + textX, TEXT_PADDING + textY + TEXT_LINE_HEIGHT * 3 ), Color.Black );
        }

        private void DrawAntrieb( Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Antrieb antrieb )
        {
            sb.DrawString( UIButton.FONT_DEFAULT, antrieb.Name, new Vector2( TEXT_PADDING + textX, TEXT_PADDING + textY ), Color.Black );
            sb.DrawString( UIButton.FONT_DEFAULT, "Schaden: " + antrieb.Damage, new Vector2( TEXT_PADDING + textX, TEXT_PADDING + textY + TEXT_LINE_HEIGHT * 2 ), Color.Black );
            sb.DrawString( UIButton.FONT_DEFAULT, "Feuerrate: " + antrieb.RateOfFire, new Vector2( TEXT_PADDING + textX, TEXT_PADDING + textY + TEXT_LINE_HEIGHT * 3 ), Color.Black );
            sb.DrawString( UIButton.FONT_DEFAULT, "Gewicht: " + antrieb.Weight, new Vector2( TEXT_PADDING + textX, TEXT_PADDING + textY + TEXT_LINE_HEIGHT * 4 ), Color.Black );
        }

        private void DrawPowerup( Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Powerup powerup )
        {
            sb.DrawString( UIButton.FONT_DEFAULT, powerup.Name, new Vector2( TEXT_PADDING + textX, TEXT_PADDING + textY ), Color.Black );
            //sb.DrawString(UIButton.FONT_DEFAULT, "Schaden: " + antrieb.Damage + " Feuerrate: " + antrieb.RateOfFire, new Vector2(textPadding + x, textPadding + y + textLineHeight), Color.Black);
            sb.DrawString( UIButton.FONT_DEFAULT, "Gewicht: " + powerup.Weight, new Vector2( TEXT_PADDING + textX, TEXT_PADDING + textY + TEXT_LINE_HEIGHT * 2 ), Color.Black );
        }

        private void DrawStabilisator( Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Stabilisator stabilisator )
        {
            sb.DrawString( UIButton.FONT_DEFAULT, stabilisator.Name, new Vector2( TEXT_PADDING + textX, TEXT_PADDING + textY ), Color.Black );
            sb.DrawString( UIButton.FONT_DEFAULT, "Accuracy: " + stabilisator.Accuracy, new Vector2( TEXT_PADDING + textX, TEXT_PADDING + textY + TEXT_LINE_HEIGHT * 2 ), Color.Black );
            sb.DrawString( UIButton.FONT_DEFAULT, "Gewicht: " + stabilisator.Weight, new Vector2( TEXT_PADDING + textX, TEXT_PADDING + textY + TEXT_LINE_HEIGHT * 3 ), Color.Black );
        }

        private void DrawHauptteil( Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Hauptteil hauptteil )
        {
            sb.DrawString( UIButton.FONT_DEFAULT, hauptteil.Name, new Vector2( TEXT_PADDING + textX, TEXT_PADDING + textY ), Color.Black );
            sb.DrawString( UIButton.FONT_DEFAULT, "Feuerrate: " + hauptteil.RateOfFire, new Vector2( TEXT_PADDING + textX, TEXT_PADDING + textY + TEXT_LINE_HEIGHT * 2 ), Color.Black );
            sb.DrawString( UIButton.FONT_DEFAULT, "Schüsse: " + hauptteil.ShotCount, new Vector2( TEXT_PADDING + textX, TEXT_PADDING + textY + TEXT_LINE_HEIGHT * 3 ), Color.Black );
            sb.DrawString( UIButton.FONT_DEFAULT, "Gewicht: " + hauptteil.Weight, new Vector2( TEXT_PADDING + textX, TEXT_PADDING + textY + TEXT_LINE_HEIGHT * 4 ), Color.Black );
        }
    }
}
