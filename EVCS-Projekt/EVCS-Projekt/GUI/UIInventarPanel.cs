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
        public bool Visible
        {
            get { return isVisible; }
            set
            {
                if ( value && !isVisible )
                {
                    filteredList.GenerateFilteredLists( player.Inventar );
                    GenerateUiComponents();
                    filteredList.SetItems(player.Inventar);
                    filteredList.ResetToggleButtons();
                    activeItem = null;
                }
                isVisible = value;
            }
        }

        int textLineHeight = 30;
        int textPadding = 20;

        private Texture2D backgroundTextur;
        private Rectangle background;

        private Player player;


        private bool isVisible;
        //public Item activeItem;
        private UIInfoPanel infoPanel;

        private UIButton btnOk;
        private UIButton btnCancel;

        private List<UIShortcutButton> shortcutButtons;

        private int lastFreeShortcutIndex;
        private UIFilteredList filteredList;
        private Item activeItem;

        

        public UIInventarPanel( int width, int height, Vector2 position )
            : base( width, height, position )
        {
            player = Main.MainObject.GameManager.GameState.Player;

            Visible = false;

            background = new Rectangle((int)GetPosition().X, (int)GetPosition().Y, base.width, base.height);
            backgroundTextur = Main.ContentManager.Load<Texture2D>( "images/gui/inventar/inventar_background" );

            shortcutButtons = new List<UIShortcutButton>();

            filteredList = new UIFilteredList(260, 236, new Vector2(280, 60), this);
            Add( filteredList );

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


        public override void Draw( SpriteBatch sb )
        {
            sb.Draw( backgroundTextur, background, Color.White );
            base.Draw( sb );


            DrawInfoPanel( sb );

        }

        private void DrawInfoPanel( SpriteBatch sb )
        {
            if (activeItem != null)
                if (activeItem.GetType() == typeof(Antrieb))
                {
                    DrawAntrieb(sb, (Antrieb)activeItem);
                }
                else if (activeItem.GetType() == typeof(Hauptteil))
                {
                    DrawHauptteil(sb, (Hauptteil)activeItem);
                }
                else if (activeItem.GetType() == typeof(Munition))
                {
                    DrawMun(sb, (Munition)activeItem);
                }
                else if (activeItem.GetType() == typeof(Powerup))
                {
                    DrawPowerup(sb, (Powerup)activeItem);
                }
                else if (activeItem.GetType() == typeof(Stabilisator))
                {
                    DrawStabilisator(sb, (Stabilisator)activeItem);
                }
                else if (activeItem.GetType() == typeof(Visier))
                {
                    DrawVisier(sb, (Visier)activeItem);
                }
                else if (activeItem.GetType() == typeof(Weapon))
                {
                    DrawWeapon(sb, (Weapon)activeItem);
                }
        }

        public void OnMouseDown( UIElement element )
        {
           
            //////////////////////////////////////////////////////
            // ListButtons
            if (element.GetType() == typeof(UIListButton))
            {

                activeItem = ((UIListButton)element).Item;

                infoPanel.Item = activeItem;

                if (activeItem.GetType() == typeof(Powerup))
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
                            filteredList.GenerateFilteredLists( player.Inventar );
                            shortcutButtons[ lastFreeShortcutIndex ].Weapon = ( Weapon ) activeItem;
                            lastFreeShortcutIndex++;
                        }
                    }
                }
                else if ( element == btnCancel )
                {
                    Item dropedItem = Item.Get(activeItem.TypeId);
                    dropedItem.LocationBehavior.Position = player.LocationBehavior.Position;
                    dropedItem.LocationSizing();
                    if(dropedItem.GetType() == typeof(Munition))
                    {
                        ((Munition)dropedItem).Count = player.Inventar[activeItem.TypeId];
                    }
                    Main.MainObject.GameManager.GameState.QuadTreeItems.Add(dropedItem);


                    filteredList.RemoveActiveItem();
                    activeItem = player.RemoveItemFromInventar( activeItem );
                    filteredList.GenerateFilteredLists(player.Inventar);
                    infoPanel.Item = activeItem;
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


        private void SwitchWeapon(Weapon newWeapon, UIShortcutButton button)
        {
            Weapon oldWeapon = button.Weapon;

            
                if (oldWeapon != null)
                {
                    if (oldWeapon.Munition != null)
                    {
                        // munition ins Inventar
                        player.AddItemToInventar(oldWeapon.Munition);
                        filteredList.AddItem(oldWeapon.Munition);
                    }

                    //Waffe ins inventar
                    player.RemoveWeaponFromShortcutList(button.Key);
                    player.AddItemToInventar(oldWeapon);
                    filteredList.AddItem(oldWeapon);
                    button.Weapon = null;
                }
                if (newWeapon != null)
                {
                    //Waffe ändern


                    player.AddWeaponToShortcutList(button.Key, newWeapon);
                    player.RemoveItemFromInventar(newWeapon);
                    button.Weapon = newWeapon;

                }
            
        }

        private void SwitchMun(Munition newMunition, UIShortcutButton button)
        {
            Weapon oldWeapon = button.Weapon;
            
            if(oldWeapon !=null)
            {
                if(oldWeapon.Munition != null)
                {
                    Munition oldMun = button.Weapon.Munition;
                    Munition newMun = newMunition.Clone();

                    if (newMun.TypeId == oldMun.TypeId)
                    {
                        button.Weapon.Reload();
                    }
                    else
                    {
                        //mun setzten
                        player.AddItemToInventar(oldMun);
                        filteredList.AddItem(oldMun);

                        newMun.Count = Math.Min(player.Inventar[newMun.TypeId], newMun.MagazineSize);

                        button.Weapon.Munition = newMun;
                        player.RemoveItemFromInventar(newMun);
                    }
                    
                }
                else
                {
                    //mun ändern
                    // Wenn die Waffe, die am Shortcut "klebt" auch munition hat
                    newMunition.Count = Math.Min(player.Inventar[newMunition.TypeId], newMunition.MagazineSize);
                    button.Weapon.Munition = newMunition;
                    player.RemoveItemFromInventar(newMunition);
                }
            }
        }


        private void HandleShortcutButtonClick( UIShortcutButton element )
        {
            UIShortcutButton button = element;

            if (activeItem == null )
            {
                if (player.GetShortcuts().Values.Count > 1)
                {
                    SwitchWeapon(null, button);
                }
            }
            else if (activeItem.GetType() == typeof( Weapon ))
            {
                SwitchWeapon((Weapon) activeItem, button);
            }
            else if (activeItem.GetType() == typeof(Munition))
            {
                SwitchMun((Munition)activeItem, button);
            }else
            {
                if (player.GetShortcuts().Values.Count > 1)
                {
                    SwitchWeapon(null, button);
                }
            }
            activeItem = null;

            filteredList.RefreshItemList();

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

        private void DrawMun(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Munition mun)
        {
            int x = (int)(GetPosition().X + 20);
            int y = (int)(GetPosition().Y + 240);


            sb.DrawString(UIButton.FONT_DEFAULT, mun.Name, new Vector2(textPadding + x, textPadding + y), Color.Black);
            sb.DrawString(UIButton.FONT_DEFAULT, "Schaden: " + mun.Damage + " Magazin: " + mun.MagazineSize, new Vector2(textPadding + x, textPadding + y + textLineHeight), Color.Black);
            sb.DrawString(UIButton.FONT_DEFAULT, "Gewicht: " + mun.Weight, new Vector2(textPadding + x, textPadding + y + textLineHeight * 2), Color.Black);
        }

        private void DrawWeapon(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Weapon weapon)
        {
            int x = (int)(GetPosition().X + 20);
            int y = (int)(GetPosition().Y + 240);

            sb.DrawString(UIButton.FONT_DEFAULT, weapon.Name, new Vector2(textPadding + x, textPadding + y), Color.Black);
            sb.DrawString(UIButton.FONT_DEFAULT, "Schaden: " + weapon.Damage + " Feuerrate: " + weapon.RateOfFire, new Vector2(textPadding + x, textPadding + y + textLineHeight), Color.Black);
            sb.DrawString(UIButton.FONT_DEFAULT, "Accuracy: " + weapon.Accuracy + " Gewicht: " + weapon.Weight, new Vector2(textPadding + x, textPadding + y + textLineHeight * 2), Color.Black);
        }

        private void DrawVisier(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Visier visier)
        {
            int x = (int)(GetPosition().X + 20);
            int y = (int)(GetPosition().Y + 240);

            sb.DrawString(UIButton.FONT_DEFAULT, visier.Name, new Vector2(textPadding + x, textPadding + y), Color.Black);
            sb.DrawString(UIButton.FONT_DEFAULT, "Accuracy: " + visier.Accuracy, new Vector2(textPadding + x, textPadding + y + textLineHeight), Color.Black);
            sb.DrawString(UIButton.FONT_DEFAULT, "Gewicht: " + visier.Weight, new Vector2(textPadding + x, textPadding + y + textLineHeight * 2), Color.Black);
        }

        private void DrawAntrieb(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Antrieb antrieb)
        {
            int x = (int)(GetPosition().X + 20);
            int y = (int)(GetPosition().Y + 240);

            sb.DrawString(UIButton.FONT_DEFAULT, antrieb.Name, new Vector2(textPadding + x, textPadding + y), Color.Black);
            sb.DrawString(UIButton.FONT_DEFAULT, "Schaden: " + antrieb.Damage + " Feuerrate: " + antrieb.RateOfFire, new Vector2(textPadding + x, textPadding + y + textLineHeight), Color.Black);
            sb.DrawString(UIButton.FONT_DEFAULT, "Gewicht: " + antrieb.Weight, new Vector2(textPadding + x, textPadding + y + textLineHeight * 2), Color.Black);
        }

        private void DrawPowerup(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Powerup powerup)
        {
            int x = (int)(GetPosition().X + 20);
            int y = (int)(GetPosition().Y + 240);

            sb.DrawString(UIButton.FONT_DEFAULT, powerup.Name, new Vector2(textPadding + x, textPadding + y), Color.Black);
            //sb.DrawString(UIButton.FONT_DEFAULT, "Schaden: " + antrieb.Damage + " Feuerrate: " + antrieb.RateOfFire, new Vector2(textPadding + x, textPadding + y + textLineHeight), Color.Black);
            sb.DrawString(UIButton.FONT_DEFAULT, "Gewicht: " + powerup.Weight, new Vector2(textPadding + x, textPadding + y + textLineHeight * 2), Color.Black);
        }

        private void DrawStabilisator(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Stabilisator stabilisator)
        {
            int x = (int)(GetPosition().X + 20);
            int y = (int)(GetPosition().Y + 240);

            sb.DrawString(UIButton.FONT_DEFAULT, stabilisator.Name, new Vector2(textPadding + x, textPadding + y), Color.Black);
            sb.DrawString(UIButton.FONT_DEFAULT, "Accuracy: " + stabilisator.Accuracy, new Vector2(textPadding + x, textPadding + y + textLineHeight), Color.Black);
            sb.DrawString(UIButton.FONT_DEFAULT, "Gewicht: " + stabilisator.Weight, new Vector2(textPadding + x, textPadding + y + textLineHeight * 2), Color.Black);
        }

        private void DrawHauptteil(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Hauptteil hauptteil)
        {
            int x = (int)(GetPosition().X + 20);
            int y = (int)(GetPosition().Y + 240);

            sb.DrawString(UIButton.FONT_DEFAULT, hauptteil.Name, new Vector2(textPadding + x, textPadding + y), Color.Black);
            sb.DrawString(UIButton.FONT_DEFAULT, "Feuerrate: " + hauptteil.RateOfFire + "Schüsse: " + hauptteil.ShotCount, new Vector2(textPadding + x, textPadding + y + textLineHeight), Color.Black);
            sb.DrawString(UIButton.FONT_DEFAULT, "Gewicht: " + hauptteil.Weight, new Vector2(textPadding + x, textPadding + y + textLineHeight * 2), Color.Black);
        }
    }
}
