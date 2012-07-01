using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LastMan.Objects;
using LastMan.Objects.Items;
using LastMan;
using LastMan.Objects.Items;
using LastMan.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LastMan.UI
{
    class UIConstructionPanel : UIPanel, UIActionListener
    {
        private UIActionListener listener;

        private UIWeaponPanel weaponPanel;
        private UIButton antriebButton;
        private UIButton stabilisatorButton;
        private UIButton visierButton;

        private UIButton nameButton;

        private UIButton rateOfFireButton;
        private UIButton demageButton;
        private UIButton accuracyButton;

        public Hauptteil Hauptteil { get; private set; }
        public Antrieb Antrieb { get; private set; }
        public Stabilisator Stabilisator { get; private set; }
        public Visier Visier { get; private set; }

        private Weapon createdWeapon;

        private UIVerticalProgressBar bar1;
        private UIVerticalProgressBar bar2;
        private UIVerticalProgressBar bar3;

        private UIButton okButton;

        private int initWidth;
        private int initHeigth;

        public bool NameIsActive;


        private string rateOfFire;
        private string demage;
        private string accuracy;

        private Texture2D nameButtonTextur;
        private Texture2D nameButtonActiveTextur;

        MouseState oldMouseState;
        Keys lastKey = Keys.None;
        public string InputText = "";

        private Player player;

        public UIConstructionPanel( int width, int height, Vector2 position, UIActionListener listener )
            : base( width, height, position )
        {
            this.listener = listener;
            initWidth = width;
            initHeigth = height;

            player = Main.MainObject.GameManager.GameState.Player;

            InitComponents();
            NameIsActive = false;


            rateOfFire = "Feuerate : ";
            demage = "Schaden : ";
            accuracy = "Genauigkeit : ";
        }

        private void InitComponents()
        {
            var shortCutTitel = new UIButton( initWidth, 40, new Vector2( 0, 0 ), "Konstruktor" );

            weaponPanel = new UIWeaponPanel( 100, new Vector2( 0, 40 ) );

            nameButton = new UIButton( initWidth, 30, new Vector2( 0, 140 ), "Name der Waffe" );

            antriebButton = new UIButton( DEFAULT_HEIGHT, DEFAULT_HEIGHT, new Vector2( 0, 180 ), "" );
            stabilisatorButton = new UIButton( DEFAULT_HEIGHT, DEFAULT_HEIGHT, new Vector2( 0, 230 ), "" );
            visierButton = new UIButton( DEFAULT_HEIGHT, DEFAULT_HEIGHT, new Vector2( 0, 280 ), "" );

            demageButton = new UIButton( 120, DEFAULT_HEIGHT, new Vector2( 50, 180 ), "Schaden : 5" );
            rateOfFireButton = new UIButton( 120, DEFAULT_HEIGHT, new Vector2( 50, 230 ), "Feuerate : 10" );
            accuracyButton = new UIButton( 120, DEFAULT_HEIGHT, new Vector2( 50, 280 ), "Genauigkeit : 13" );

            player.Liquids = new Vector3( 14, 11, 22 );

            bar1 = new UIVerticalProgressBar( 40, 140, new Vector2( 175, 180 ), 100, player.Liquids.X ) { BackgroundColor = Color.Blue };
            bar2 = new UIVerticalProgressBar( 40, 140, new Vector2( 215, 180 ), 100, player.Liquids.Y ) { BackgroundColor = Color.Green };
            bar3 = new UIVerticalProgressBar( 40, 140, new Vector2( 255, 180 ), 100, player.Liquids.Z ) { BackgroundColor = Color.Red };


            Texture2D okButtonTexture = Main.ContentManager.Load<Texture2D>( "images/gui/inventar/craft_button" );
            Texture2D okButtonHoverTexture = Main.ContentManager.Load<Texture2D>( "images/gui/inventar/craft_button_h" );

            okButton = new UIButton( initWidth, 30, new Vector2( 0, 330 ), okButtonTexture, okButtonHoverTexture,"" );


            Texture2D pixelWhite = Main.ContentManager.Load<Texture2D>( "images/pixelWhite" );
            nameButtonActiveTextur = Main.ContentManager.Load<Texture2D>( "images/gui/inventar/name_button_a" );
            nameButtonTextur = Main.ContentManager.Load<Texture2D>( "images/gui/inventar/name_button" );

            okButton.AddActionListener( listener );
            nameButton.AddActionListener( this );


            Add( shortCutTitel );

            Add( nameButton );

            Add( weaponPanel );
            Add( antriebButton );
            Add( stabilisatorButton );
            Add( visierButton );

            Add( demageButton );
            Add( rateOfFireButton );
            Add( accuracyButton );

            Add( bar1 );
            Add( bar2 );
            Add( bar3 );

            Add( okButton );
        }



        public override void Update()
        {
            if ( NameIsActive )
            {
                if ( Keyboard.GetState().IsKeyUp( lastKey ) )
                {
                    lastKey = Keys.None;
                }

                if ( Keyboard.GetState().GetPressedKeys().Length > 0 && lastKey == Keys.None )
                {
                    nameButton.Text = "";
                    lastKey = Keyboard.GetState().GetPressedKeys()[ 0 ];
                    if ( lastKey == Keys.Back )
                    {
                        if ( InputText.Length != 0 )
                            InputText = InputText.Substring( 0, InputText.Length - 1 );
                    }
                    else if ( InputText.Length < 25 )
                    {
                        InputText += ( char ) lastKey.GetHashCode();
                    }
                }

                nameButton.Text = InputText;
                nameButton.BackgroundTextur = nameButtonActiveTextur;

            }
            else
            {
                nameButton.BackgroundTextur = nameButtonTextur;
            }
            if ( Hauptteil != null && Visier != null && Antrieb != null && Stabilisator != null && IsEnoughLiquid() )
            {
                okButton.IsEnabled = true;
            }
            else
            {
                okButton.IsEnabled = false;
            }



            if ( Mouse.GetState().LeftButton == ButtonState.Pressed && oldMouseState.LeftButton != ButtonState.Pressed )
            {
                NameIsActive = false;
            }
            oldMouseState = Mouse.GetState();
            base.Update();
        }

        private bool IsEnoughLiquid()
        {
            return bar1.IsSnoughLiquid() && bar2.IsSnoughLiquid() && bar3.IsSnoughLiquid();
        }

        private void DecrimentRequired( Item item )
        {
            Vector3 vector =new Vector3();
            if ( item != null )
                vector = item.RequiredLiquid;

            bar1.Required -= vector.X;
            bar2.Required -= vector.Y;
            bar3.Required -= vector.Z;
        }

        private void IncrementRequired( Item item )
        {
            Vector3 vector =new Vector3();
            if ( item != null )
                vector = item.RequiredLiquid;

            bar1.Required += vector.X;
            bar2.Required += vector.Y;
            bar3.Required += vector.Z;
        }


        public void SetVisier( Visier newVisier )
        {
            DecrimentRequired( Visier );
            IncrementRequired( newVisier );

            Visier = newVisier;
            visierButton.BackgroundTextur = newVisier.Icon;
            visierButton.BackgroundColor = Color.White;
            weaponPanel.SetVisierIcon( newVisier );
            accuracyButton.Text = accuracy + GetTotalAccuracy();
        }


        public void SetStabilisator( Stabilisator newStabilisator )
        {
            DecrimentRequired( Stabilisator );
            IncrementRequired( newStabilisator );

            Stabilisator = newStabilisator;
            stabilisatorButton.BackgroundTextur = newStabilisator.Icon;
            stabilisatorButton.BackgroundColor = Color.White;
            weaponPanel.SetStabilisatorIcon( newStabilisator );
            accuracyButton.Text = accuracy + GetTotalAccuracy();
        }

        public void SetAntrieb( Antrieb newAntrieb )
        {
            DecrimentRequired( Antrieb );
            IncrementRequired( newAntrieb );

            Antrieb = newAntrieb;
            antriebButton.BackgroundTextur = newAntrieb.Icon;
            antriebButton.BackgroundColor = Color.White;
            weaponPanel.SetAntriebIcon( newAntrieb );
            rateOfFireButton.Text = rateOfFire + GetTotalRateOfFire();
            demageButton.Text = demage + newAntrieb.Damage;
        }

        public void SetHauptteil( Hauptteil newHauptteil )
        {
            DecrimentRequired( Hauptteil );
            IncrementRequired( newHauptteil );

            Hauptteil = newHauptteil;
            weaponPanel.SetHauptteilIcon( newHauptteil );
            rateOfFireButton.Text = rateOfFire + GetTotalRateOfFire();
        }


        private float GetTotalRateOfFire()
        {
            float totalRateOfFire = 0;
            if ( Hauptteil != null )
            {
                totalRateOfFire += Hauptteil.RateOfFire;
            }
            if ( Antrieb != null )
            {
                totalRateOfFire += Antrieb.RateOfFire;
            }
            return totalRateOfFire;
        }

        private float GetTotalAccuracy()
        {
            float totalAccuracy = 0;
            if ( Visier != null )
            {
                totalAccuracy += Visier.Accuracy;
            }
            if ( Stabilisator != null )
            {
                totalAccuracy += Stabilisator.Accuracy;
            }
            return totalAccuracy;
        }

        public void OnMouseDown( UIElement element )
        {
            NameIsActive = !NameIsActive;
        }

        public void OnMouseUp( UIElement element )
        {
            throw new NotImplementedException();
        }

        public void ResetPanel()
        {
            Texture2D pixel = Main.ContentManager.Load<Texture2D>( "images/pixelTransparent" );

            weaponPanel.ResetPanel();
            nameButton.Text = "Name der Waffe";
            antriebButton.BackgroundTextur = pixel;
            stabilisatorButton.BackgroundTextur = pixel;
            visierButton.BackgroundTextur = pixel;

            demageButton.Text = demage;
            accuracyButton.Text = accuracy;
            rateOfFireButton.Text = rateOfFire;

            Hauptteil = null;
            Antrieb = null;
            Stabilisator = null;
            Visier = null;

            bar1.Required = 0;
            bar2.Required = 0;
            bar3.Required = 0;

            bar1.Progress = player.Liquids.X;
            bar2.Progress = player.Liquids.Y;
            bar3.Progress = player.Liquids.Z;


        }
    }
}
