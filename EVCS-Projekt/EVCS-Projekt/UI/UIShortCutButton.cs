using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using EVCS_Projekt.Location;
using EVCS_Projekt.Objects.Items;
using EVCS_Projekt.Renderer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EVCS_Projekt.UI
{
    class UIShortcutButton : UIPanel
    {
        public int Key;
        private Weapon weapon;

        private UIWeaponPanel weaponPanel;
        public Weapon Weapon
        {
            get { return weapon; }
            set
            {
                if(value==null)
                {
                    weaponPanel.ResetPanel();
                }
                weapon = value;
            }
        }



        private Texture2D weaponTexture;
        //private Texture2D weaponBackgroundTexture;

        public UIShortcutButton( int width, int height, Vector2 position, int key )
            : base( width, height, position )
        {
            Key = key;


            weaponTexture = Main.ContentManager.Load<Texture2D>( "images/items/item_1" );
            //weaponTexture = LoadedRenderer.GetStatic("S_Item_" + weapon.TypeId).Texture;

            //weapon.Renderer.Draw(spriteBatch, new UILocation(new Rectangle(0,0,10,10));

            //weaponBackgroundTexture = Main.ContentManager.Load<Texture2D>( "images/gui/inventar/shortcut_weapon" );

            weaponPanel = new UIWeaponPanel( height, new Vector2( ( int ) height, 0 ) );
            Add(weaponPanel); 
            
        }


        public override void Draw( SpriteBatch sb )
        {
            int x = ( int ) GetPosition().X;
            int y = ( int ) GetPosition().Y;

            Vector2 measureString = UIButton.FONT_DEFAULT.MeasureString( Key + "" );

            sb.DrawString( UIButton.FONT_DEFAULT, Key + "", new Vector2( x + 10, y + height / 2 - measureString.Y / 2 ), Color.Black );

            //sb.Draw( weaponBackgroundTexture, new Rectangle( ( int ) GetPosition().X + height, ( int ) GetPosition().Y, width - ( height * 3 ), height ), Color.White );

            if ( weapon != null )
            {
                weaponPanel.SetWeapon(weapon);
                if ( weapon.Munition != null )
                {
                    sb.Draw( weapon.Munition.Icon, new Rectangle( ( int ) x + width - height * 2, ( int ) GetPosition().Y, height, height ), Color.White );
                    sb.DrawString( UIButton.FONT_DEFAULT, weapon.Munition.Count + "", new Vector2( x + 10 + width - height, y + height / 2 - measureString.Y / 2 ), Color.Black );
                }
            }


            base.Draw( sb );
        }

    }
}
