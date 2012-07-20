using LastMan.Objects.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LastMan.UI
{
    class UIWeaponPanel : UIPanel
    {

        private Texture2D hauptteilCraftingIcon;
        private Texture2D antriebCraftingIcon;
        private Texture2D stabilisatorCraftingIcon;
        private Texture2D visierCraftingIcon;

        public UIWeaponPanel( int height, Vector2 position )
            : base( height * 3, height, position )
        {
            Texture2D pixel = Main.ContentManager.Load<Texture2D>( "images/pixelTransparent" );

            hauptteilCraftingIcon = pixel;
            antriebCraftingIcon = pixel;
            stabilisatorCraftingIcon = pixel;
            visierCraftingIcon = pixel;
        }

        public void SetWeapon(Weapon weapon)
        {
            if(weapon.Hauptteil != null)
            {
                hauptteilCraftingIcon = weapon.Hauptteil.CraftingItem;
            }
            if ( weapon.Antrieb != null )
            {
                antriebCraftingIcon = weapon.Antrieb.CraftingItem;
            }
            if ( weapon.Stabilisator != null )
            {
                stabilisatorCraftingIcon = weapon.Stabilisator.CraftingItem;
            }
            if ( weapon.Visier != null )
            {
                visierCraftingIcon = weapon.Visier.CraftingItem;
            }
        }

        public void SetHauptteilIcon(Hauptteil hauptteil)
        {
            hauptteilCraftingIcon = hauptteil.CraftingItem;
        }

        public void SetAntriebIcon( Antrieb antrieb )
        {
            antriebCraftingIcon = antrieb.CraftingItem;
        }

        public void SetStabilisatorIcon( Stabilisator stabilisator )
        {
            stabilisatorCraftingIcon = stabilisator.CraftingItem;
        }

        public void SetVisierIcon( Visier visier )
        {
            visierCraftingIcon = visier.CraftingItem;
        }

        public void ResetPanel()
        {
            Texture2D pixel = Main.ContentManager.Load<Texture2D>( "images/pixelTransparent" );

            hauptteilCraftingIcon = pixel;
            antriebCraftingIcon = pixel;
            stabilisatorCraftingIcon = pixel;
            visierCraftingIcon = pixel;
        }

        public override void Draw( Microsoft.Xna.Framework.Graphics.SpriteBatch sb )
        {
            //Texture2D pixel = Main.ContentManager.Load<Texture2D>( "images/pixelWhite" );
            Rectangle box = new Rectangle( ( int ) GetPosition().X, ( int ) ( GetPosition().Y ), GetWidth(), GetHeight() );
            //sb.Draw(pixel,box,Color.White);
            sb.Draw( hauptteilCraftingIcon, box, Color.White );
            sb.Draw( antriebCraftingIcon, box, Color.White );
            sb.Draw( visierCraftingIcon, box, Color.White );
            sb.Draw( stabilisatorCraftingIcon, box, Color.White );
        }
    }
}
