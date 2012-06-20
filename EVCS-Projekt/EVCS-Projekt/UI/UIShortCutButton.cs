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
    class UIShortCutButton: UIPanel
    {
        private int key;
        public Weapon Weapon { get; set; }

        private readonly UIButton btnKey;
        private readonly UIButton btnMun;
        private Texture2D weaponTexture;

        public UIShortCutButton(int width, int height, Vector2 position, int key, Weapon weapon) : base(width, height, position)
        {
            this.key = key;
            Weapon = weapon;

            weaponTexture = Main.ContentManager.Load<Texture2D>("images/items/item_1");
            //weaponTexture = LoadedRenderer.GetStatic("S_Item_" + weapon.TypeId).Texture;

            //weapon.Renderer.Draw(spriteBatch, new UILocation(new Rectangle(0,0,10,10));

            btnKey = new UIButton(DEFAULT_HEIGHT, DEFAULT_HEIGHT, new Vector2(0, 0), key+"");
            btnMun = new UIButton(DEFAULT_HEIGHT, DEFAULT_HEIGHT, new Vector2(width - DEFAULT_HEIGHT, 0), Weapon.Icon, Weapon.Icon, Weapon.MunitionCount + "");
            
            Add(btnKey);
            Add(btnMun);
        }

        public override void Draw(SpriteBatch sb)
        {
            btnKey.Draw(sb);
            sb.Draw(weaponTexture, new Rectangle((int)GetPosition().X + DEFAULT_HEIGHT, (int)GetPosition().Y, width - (DEFAULT_HEIGHT * 2), height), Color.Beige);
            btnKey.Draw(sb);
        }

    }
}
