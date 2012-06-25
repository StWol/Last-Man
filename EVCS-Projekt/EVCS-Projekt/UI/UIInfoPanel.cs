using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using EVCS_Projekt.Objects;
using EVCS_Projekt.Objects.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EVCS_Projekt.UI
{
    class UIInfoPanel : UIPanel
    {
        private Item item;
        string group = "";
        string name = "";
        string desc = "";

        int lineHeight = 30;
        int padding = 20;

        

        public Item Item
        {
            get { return item; }
            set { item = value; }
        }



        public UIInfoPanel( int width, int height, Vector2 position )
            : base( width, height, position )
        {


        }


        public override void Draw( Microsoft.Xna.Framework.Graphics.SpriteBatch sb )
        {
            

            //sb.Draw( Main.ContentManager.Load<Texture2D>( "images/pixelWhite" ), GetBoundingBox(), Color.White );

            // Falls Visier
            if(Item != null) 
                if (Item.GetType() == typeof(Antrieb))
                {
                    DrawAntrieb(sb,(Antrieb) Item);
                }
                else if (Item.GetType() == typeof(Hauptteil))
                {
                    DrawHauptteil(sb,(Hauptteil) Item);
                }
                else if (Item.GetType() == typeof(Munition))
                {
                    DrawMun(sb,(Munition) Item);
                }
                else if (Item.GetType() == typeof(Powerup))
                {
                    DrawPowerup(sb, (Powerup) Item);
                }
                else if (Item.GetType() == typeof(Stabilisator))
                {
                    DrawStabilisator(sb, (Stabilisator) Item);
                }
                else if (Item.GetType() == typeof(Visier))
                {
                    DrawVisier(sb, (Visier) Item);
                }
                else if (Item.GetType() == typeof(Weapon))
                {
                    DrawWeapon(sb, (Weapon) Item);
                }

            
            base.Draw( sb );

        }

        private void DrawMun(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Munition mun)
        {
            int x = (int)(GetPosition().X);
            int y = (int)(GetPosition().Y);


            sb.DrawString(UIButton.FONT_DEFAULT, mun.Name, new Vector2(padding + x, padding + y), Color.Black);
            sb.DrawString(UIButton.FONT_DEFAULT, "Schaden: " + mun.Damage + " Magazin: " + mun.MagazineSize, new Vector2(padding + x, padding + y + lineHeight), Color.Black);
            sb.DrawString(UIButton.FONT_DEFAULT, "Gewicht: " + mun.Weight, new Vector2(padding + x, padding + y + lineHeight * 2), Color.Black);
        }

        private void DrawWeapon(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Weapon weapon)
        {
            int x = (int)(GetPosition().X);
            int y = (int)(GetPosition().Y);

            sb.DrawString(UIButton.FONT_DEFAULT, weapon.Name, new Vector2(padding + x, padding + y), Color.Black);
            sb.DrawString(UIButton.FONT_DEFAULT, "Schaden: " + weapon.Damage + " Feuerrate: " + weapon.RateOfFire, new Vector2(padding + x, padding + y + lineHeight), Color.Black);
            sb.DrawString(UIButton.FONT_DEFAULT, "Accuracy: " + weapon.Accuracy + " Gewicht: " + weapon.Weight, new Vector2(padding + x, padding + y + lineHeight * 2), Color.Black);
        }

        private void DrawVisier(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Visier visier)
        {
            int x = (int)(GetPosition().X);
            int y = (int)(GetPosition().Y);

            sb.DrawString(UIButton.FONT_DEFAULT, visier.Name, new Vector2(padding + x, padding + y), Color.Black);
            sb.DrawString(UIButton.FONT_DEFAULT, "Accuracy: " + visier.Accuracy, new Vector2(padding + x, padding + y + lineHeight), Color.Black);
            sb.DrawString(UIButton.FONT_DEFAULT, "Gewicht: " + visier.Weight, new Vector2(padding + x, padding + y + lineHeight * 2), Color.Black);
        }

        private void DrawAntrieb(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Antrieb antrieb)
        {
            int x = (int)(GetPosition().X);
            int y = (int)(GetPosition().Y);

            sb.DrawString(UIButton.FONT_DEFAULT, antrieb.Name, new Vector2(padding + x, padding + y), Color.Black);
            sb.DrawString(UIButton.FONT_DEFAULT, "Schaden: " + antrieb.Damage + " Feuerrate: " + antrieb.RateOfFire, new Vector2(padding + x, padding + y + lineHeight), Color.Black);
            sb.DrawString(UIButton.FONT_DEFAULT, "Gewicht: " + antrieb.Weight, new Vector2(padding + x, padding + y + lineHeight * 2), Color.Black);
        }

        private void DrawPowerup(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Powerup powerup)
        {
            int x = (int)(GetPosition().X);
            int y = (int)(GetPosition().Y);

            sb.DrawString(UIButton.FONT_DEFAULT, powerup.Name, new Vector2(padding + x, padding + y), Color.Black);
            //sb.DrawString(UIButton.FONT_DEFAULT, "Schaden: " + antrieb.Damage + " Feuerrate: " + antrieb.RateOfFire, new Vector2(padding + x, padding + y + lineHeight), Color.Black);
            sb.DrawString(UIButton.FONT_DEFAULT, "Gewicht: " + powerup.Weight, new Vector2(padding + x, padding + y + lineHeight * 2), Color.Black);
        }

        private void DrawStabilisator(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Stabilisator stabilisator)
        {
            int x = (int)(GetPosition().X);
            int y = (int)(GetPosition().Y);

            sb.DrawString(UIButton.FONT_DEFAULT, stabilisator.Name, new Vector2(padding + x, padding + y), Color.Black);
            sb.DrawString(UIButton.FONT_DEFAULT, "Accuracy: " + stabilisator.Accuracy , new Vector2(padding + x, padding + y + lineHeight), Color.Black);
            sb.DrawString(UIButton.FONT_DEFAULT, "Gewicht: " + stabilisator.Weight, new Vector2(padding + x, padding + y + lineHeight * 2), Color.Black);
        }

        private void DrawHauptteil(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Hauptteil hauptteil)
        {
            int x = (int)(GetPosition().X);
            int y = (int)(GetPosition().Y);

            sb.DrawString(UIButton.FONT_DEFAULT, hauptteil.Name, new Vector2(padding + x, padding + y), Color.Black);
            sb.DrawString(UIButton.FONT_DEFAULT, "Feuerrate: " + hauptteil.RateOfFire + "Schüsse: " + hauptteil.ShotCount, new Vector2(padding + x, padding + y + lineHeight), Color.Black);
            sb.DrawString(UIButton.FONT_DEFAULT, "Gewicht: " + hauptteil.Weight, new Vector2(padding + x, padding + y + lineHeight * 2), Color.Black);
        }
    }
}
