using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EVCS_Projekt.Location;
using EVCS_Projekt.Managers;
using EVCS_Projekt.Objects.Items;
using EVCS_Projekt.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;
using EVCS_Projekt.Renderer;
using EVCS_Projekt.Audio;


namespace EVCS_Projekt
{
    public class Player : GameObject
    {
        //  Wie lange das schussbild gezeigt wird
        private static float ShotTime = 0.05F;

        //Attributes
        private float maxHealth;
        public float Health { get; private set; }
        public float Speed { get; set; }
        public float[] Liquid { get; set; }
        public Weapon Weapon { set; get; }
        public List<Item> Inventar { get; private set; }
        private Dictionary<int, Item> shortcuts;
        private List<Buff> bufflist;
        public Vector2 Direction { get; set; }

        // ob ein schuss eine gewisse zeit her war
        private float shotTimer;
        public bool BigWeapon { get; set; }

        public new IRenderBehavior Renderer
        {
            get
            {
                if (Weapon == null)
                    return RendererStanding;

                if (Weapon.BigWeapon)
                {
                    if (shotTimer > 0)
                        return RendererBigWeaponShot;
                    return RendererBigWeapon;
                }
                else
                {
                    if (shotTimer > 0)
                        return RendererSmallWeaponShot;
                    return RendererSmallWeapon;
                }
            }
            set
            {
            }
        }

        public float Reloading { get; set; }

        public float FootRotation
        {
            set
            {
                footLocation.Rotation = value;
            }
        }

        private bool isMoving = false;
        public bool IsMoving
        {
            get { return isMoving; }
            set
            {
                if (isMoving == value)
                    return;

                isMoving = value;
                if (isMoving)
                {
                    //Renderer = RendererMoving;
                    footRenderer = RendererFootMoving;
                }
                else
                {
                    //Renderer = RendererStanding;
                    footRenderer = new NoRenderer();
                }
            }
        }

        // Verschiedene Renderer
        private StaticRenderer RendererStanding { get; set; }
        private StaticRenderer RendererBigWeapon { get; set; }
        private StaticRenderer RendererBigWeaponShot { get; set; }

        private StaticRenderer RendererSmallWeapon { get; set; }
        private StaticRenderer RendererSmallWeaponShot { get; set; }

        // Füße
        private AnimationRenderer RendererFootMoving { get; set; }

        // 
        private IRenderBehavior footRenderer;
        private MapLocation footLocation;


        // ***************************************************************************
        // Konstruktor
        public Player(ILocationBehavior locationBehavior, float maxHealth, float health, float speed)
            : base(locationBehavior)
        {
            bufflist = new List<Buff>();
            Speed = speed;

            // Texturen für Renderer laden
            RendererStanding = LoadedRenderer.GetStatic("S_Player_Standing");
            RendererBigWeapon = LoadedRenderer.GetStatic("S_Player_BW");
            RendererBigWeaponShot = LoadedRenderer.GetStatic("S_Player_BW_Shot");
            RendererSmallWeapon = LoadedRenderer.GetStatic("S_Player_SW");
            RendererSmallWeaponShot = LoadedRenderer.GetStatic("S_Player_SW_Shot");

            /*Texture2D[] textureMoving = new Texture2D[]{ 
                Main.ContentManager.Load<Texture2D>("images/character/moving_1"),
                Main.ContentManager.Load<Texture2D>("images/character/moving_2")
            };*/
            //RendererMoving = new AnimationRenderer(textureMoving, 4F);

            Texture2D[] textureFootMoving = new Texture2D[]{ 
                Main.ContentManager.Load<Texture2D>("images/character/left_foot"),
                Main.ContentManager.Load<Texture2D>("images/character/right_foot")
            };
            RendererFootMoving = new AnimationRenderer(textureFootMoving, 4F);

            footRenderer = new NoRenderer();

            // Location für die Füße
            footLocation = new MapLocation(locationBehavior.Position, new Vector2(textureFootMoving[0].Width, textureFootMoving[0].Height));

            // Standardmäßig den StandingRenderer zuweisen
            Renderer = RendererBigWeapon;

            // LocationSize anpassen
            LocationSizing();
            LocationBehavior.Size = Renderer.Size; // Sollte LocationSizing machen

            // InventarListe init
            Inventar = new List<Item>();

            // Schusstime auf 0 setzten
            shotTimer = 0;

            // Rect Methode setzten
            base.GetRect = base.RectPlayer;
        }

        // ***************************************************************************
        // Player Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            // Füße zeichnen
            footRenderer.Draw(spriteBatch, footLocation);

            // Character oberteil
            Renderer.Draw(spriteBatch, LocationBehavior);
        }

        // ***************************************************************************
        // Player Update
        public void Update()
        {
            // Renderer Updaten
            Renderer.Update();
            footRenderer.Update();

            // Waffe f+r cooldown etc
            Weapon.Update(bufflist);

            // Fußposition updaten
            footLocation.Position = LocationBehavior.Position;

            // ReloadTimer
            if (Reloading > 0)
                Reloading -= (float)Main.GameTimeUpdate.ElapsedGameTime.TotalSeconds;

            // ShotTimer verringer
            if (shotTimer > 0)
                shotTimer -= (float)Main.GameTimeUpdate.ElapsedGameTime.TotalSeconds;
        }

        // ***************************************************************************
        // Player schießt
        public void Shoot()
        {
            // Reloading checken
            if (Reloading > 0)
            {
                return;
            }

            // Waffe schießen lassen
            if (Weapon.Cooldown <= 0 && Weapon.ShotCount > 0)
            {
                // Schießt

                // Schuss von waffe erstellen lassen
                Shot s = Weapon.CreateShot();

                // Position und richtung des schussen berechnen
                Vector2 direction = -LocationBehavior.Direction; // + accuracy

                // Flugrichtung/position setzten und rotation des schusses
                s.LocationBehavior.Position = LocationBehavior.Position;
                s.SetDirection(direction);
                s.LocationSizing();

                // schuss adden
                Main.MainObject.GameManager.GameState.ShotListVsEnemies.Add(s);

                // Sound abspielen
                Sound.Sounds[Weapon.Antrieb.SoundId].Play();

                // Shottimer setzten
                shotTimer = ShotTime;
            }
            else if (Weapon.Cooldown <= 0 && Weapon.ShotCount == 0)
            {
                // Waffe leer
                Weapon.ResetCooldown();

                // Sound abspielen
                Sound.Sounds["Weapon_Empty"].Play();
            }
        }

        // ***************************************************************************
        // Player wird von Schuss getroffen => Schaden nehmen und Buffs übernehmen - manager muss auf Tod reagieren
        public void TakeDamage(Shot shot)
        {
            // Schaden abziehen
            Health -= shot.Damage;

            // Buffs übernehmen
            AddBuffs(shot.BuffList);
        }

        // ***************************************************************************
        // Entfernt das Item aus dem Inventar
        public void EatItem(Item item)
        {
            this.Inventar.Remove(item);
        }

        // ***************************************************************************
        // Fügt von Liquid mit der ID id, die Menge amount hinzu
        public void AddLiquid(int id, float amount)
        {
            this.Liquid[id] += amount;
        }

        // ***************************************************************************
        // Zieht von Liquid mit der ID id, die Menge amount ab
        public void ReduceLiquid(int id, float amount)
        {
            this.Liquid[id] -= amount;
            if (Liquid[id] - amount <= 0)
                amount = 0;
        }

        // ***************************************************************************
        // Fügt die übergebene Liste mit Buffs dem Player hinzu
        public void AddBuffs(List<Buff> buffList)
        {
            this.bufflist.AddRange(bufflist);
        }

        // Kleine BoundingBox
        public Rectangle LittleBoundingBox
        {
            get
            {
                int s = (int)LocationBehavior.Size.Y;
                return new Rectangle((int)(LocationBehavior.Position.X - s / 2F), (int)(LocationBehavior.Position.Y - s / 2F), s, s);
            }
            set
            {
            }
        }

        // ***************************************************************************
        // Prüft ob position außerhalb der map liegt
        public new bool CheckPosition(Vector2 newPosition)
        {
            // old position
            Vector2 old = LocationBehavior.Position;

            // neue positon setzen
            LocationBehavior.Position = newPosition;

            // checken ob die neue position kaputt ist
            // checken ob die neue position kaputt ist
            if (!GameManager.CheckRectangleInMap(LittleBoundingBox))
            {
                LocationBehavior.Position = old;
                return false;
            }
            else
            {
                LocationBehavior.Position = old;
                return true;
            }
        }
    }
}
