using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using EVCS_Projekt.Location;
using EVCS_Projekt.Objects;
using EVCS_Projekt.Objects.Items;
using EVCS_Projekt.Renderer;
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

        private UIConstructionPanel constructionPanel;

        public bool NameIsActive { get { return constructionPanel.NameIsActive; } }
        public bool Visible
        {
            get { return isVisible; }
            set
            {
                if ( value && !isVisible )
                {
                    //InitComponents();

                    Dictionary<int, int> tempDict = new Dictionary<int, int>();
                    foreach (KeyValuePair<int, int> keyValuePair in player.Inventar)
                    {
                        Item item = Item.Get(keyValuePair.Key);
                        if (item.GetType() != typeof(Munition) && item.GetType() != typeof(Powerup))
                        {
                            tempDict[keyValuePair.Key] = keyValuePair.Value;
                        }
                    }

                    filteredConstructorList.GenerateFilteredLists(player.Inventar);
                    filteredConstructorList.SetItems(tempDict);

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

            BackgroundTextur = Main.ContentManager.Load<Texture2D>( "images/gui/inventar/inventar_background" );

            Helper.DrawHelper.AddDimension( "TextLinePadding", 20, 20 );
            InitComponents();
        }

        private void InitComponents()
        {
            var ok = Main.ContentManager.Load<Texture2D>( "images/gui/inventar/btn_ok" );
            var ok_h = Main.ContentManager.Load<Texture2D>( "images/gui/inventar/btn_ok_h" );
            var cancel = Main.ContentManager.Load<Texture2D>( "images/gui/inventar/btn_cancel" );
            var cancel_h = Main.ContentManager.Load<Texture2D>( "images/gui/inventar/btn_cancel_h" );

            constructionPanel = new UIConstructionPanel(300, 360,new Vector2(20,20),this );

            filteredConstructorList = new UIFilteredConstructorList(260, 360 , new Vector2(340, 20), this);

            Add( filteredConstructorList );
            Add(constructionPanel);


            //btnOk = new UIButton( new Vector2( 300, 306 ), ok, ok_h );
            //btnCancel = new UIButton( new Vector2( 350, 306 ), cancel, cancel_h );

            //btnOk.IsEnabled = false;
            //btnCancel.IsEnabled = false;

            //btnOk.AddActionListener( this );
            //btnCancel.AddActionListener( this );


            
            

            //Add( btnOk );
            //Add( btnCancel );
        }

        public override void Draw( SpriteBatch sb )
        {
            
            base.Draw( sb );
        }

        public void OnMouseDown(UIElement element)
        {
            if (element.GetType() == typeof(UIListButton))
            {
                Item item = ((UIListButton) element).Item;
                if (item.GetType() == typeof (Visier))
                {
                    constructionPanel.SetVisier((Visier) item);
                }
                else if (item.GetType() == typeof (Antrieb))
                {
                    constructionPanel.SetAntrieb((Antrieb) item);
                }
                else if (item.GetType() == typeof (Stabilisator))
                {
                    constructionPanel.SetStabilisator((Stabilisator) item);
                }
                else if (item.GetType() == typeof (Hauptteil))
                {
                    constructionPanel.SetHauptteil((Hauptteil) item);
                }
            }
            else if(element.GetType() == typeof(UIButton))
            {
                Visier v = constructionPanel.Visier;
                Antrieb a = constructionPanel.Antrieb;
                Stabilisator s = constructionPanel.Stabilisator;
                Hauptteil h = constructionPanel.Hauptteil;
                Weapon newWeapon = new Weapon(v, a, s, h, Item.StaticID, 0, constructionPanel.InputText, 0, "", new MapLocation(new Vector2(0,0)));

                player.RemoveItemFromInventar(v);
                player.RemoveItemFromInventar(a);
                player.RemoveItemFromInventar(s);
                player.RemoveItemFromInventar(h);

                Item.AllItems.Add(Item.StaticID++, newWeapon);
                player.AddItemToInventar(newWeapon);
                filteredConstructorList.AddItem(newWeapon);
                filteredConstructorList.RefreshItemList();
                
            }
        }

        public void OnMouseUp(UIElement element)
        {
        }
    }
}
