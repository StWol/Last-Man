using System;
using System.Collections.Generic;
using System.Linq;
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
        public Weapon Weapon
        {
            get { return weapon; }
            set { weapon = value; }
        }


        private readonly UIButton btnKey;
        private readonly UIButton btnMun;
        private Texture2D weaponTexture;
        private Texture2D weaponBackgroundTexture;

        public UIShortcutButton( int width, int height, Vector2 position, int key )
            : base( width, height, position )
        {
            this.Key = key;


            weaponTexture = Main.ContentManager.Load<Texture2D>( "images/items/item_1" );
            //weaponTexture = LoadedRenderer.GetStatic("S_Item_" + weapon.TypeId).Texture;

            //weapon.Renderer.Draw(spriteBatch, new UILocation(new Rectangle(0,0,10,10));



            btnKey = new UIButton( DEFAULT_HEIGHT , DEFAULT_HEIGHT, new Vector2( 0, 0 ), key + "" );

            weaponBackgroundTexture = Main.ContentManager.Load<Texture2D>( "images/gui/inventar/shortcut_weapon" );

            btnMun = new UIButton( DEFAULT_HEIGHT, DEFAULT_HEIGHT, new Vector2( width - DEFAULT_HEIGHT, 0 ), "0" );

            Add( btnKey );
            Add( btnMun );
        }


        public override void Draw( SpriteBatch sb )
        {
            btnKey.Draw( sb );

            sb.Draw( weaponBackgroundTexture, new Rectangle( ( int ) GetPosition().X + DEFAULT_HEIGHT, ( int ) GetPosition().Y, width - ( DEFAULT_HEIGHT + DEFAULT_HEIGHT  ), height ), Color.White );

            if ( weapon != null )
                sb.Draw( weaponTexture, new Rectangle( ( int ) GetPosition().X + DEFAULT_HEIGHT, ( int ) GetPosition().Y, width - ( DEFAULT_HEIGHT * 2 ), height ), Color.Beige );

            btnMun.Draw( sb );
        }

    }
}
