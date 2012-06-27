﻿using System;
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
using EVCS_Projekt.Map;


namespace EVCS_Projekt
{
    public class Player : GameObject
    {
        //  Wie lange das schussbild gezeigt wird
        private static float ShotTime = 0.05F;

        //Attributes
        public float MaxHealth { get; private set; }
        private float _health = 0;
        public float Health
        {
            get { return _health; }
            set
            {
                // Schaden für HS speichern
                if (value < _health) {
                    Main.MainObject.GameManager.GameState.DamageTaken += _health - value;
                }

                _health = value;
                if (_health > MaxHealth)
                    _health = MaxHealth;
            }
        }
        public float Speed { get; set; }

        public float TotalInventarWeight
        {
            get
            {
                float nventarWeight = Inventar.Sum(pair => Item.Get(pair.Key).Weight * pair.Value);
                float shortcutWeight = shortcuts.Sum(pair => ((Weapon)pair.Value).Weight);
                return nventarWeight + shortcutWeight;
            }
        }
        //Waffe wird über ActiveShortcut gesetzt
        public Weapon Weapon
        {
            get
            {
                if (shortcuts != null)
                {
                    if (shortcuts.ContainsKey(ActiveShortcut))
                        return shortcuts[ActiveShortcut];
                    return shortcuts.Values.ToList()[0]; // TODO Nullpointer
                }
                return null;
            }
        }

        public Dictionary<int, int> Inventar { get; private set; }
        private Dictionary<int, Weapon> shortcuts;

        public List<Powerup> ActivePowerups { get; private set; }

        public Dictionary<EBuffType, Buff> Buffs;
        public Vector2 Direction { get; set; }
        public WayPoint NearestWayPoint { get; private set; }

        public Vector3 Liquids { get; private set; }

        // ob ein schuss eine gewisse zeit her war
        private float shotTimer;
        public bool BigWeapon { get; set; }

        // wegpunkt timeout.. nur einmal pro sekunde wegpunkt updaten
        private float _waypointTimeout = 1F;

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


        public int GetItemCountFromInventar(int key)
        {
            if (Inventar.ContainsKey(key))
                return Inventar[key];
            return 0;
        }

        public void AddItemToInventar(Item item)
        {
            int anzahl = 1;
            if (item.GetType() == typeof(Munition))
                anzahl = ((Munition)item).Count;
            AddRangeItemToInventar(item, anzahl);
        }


        public void AddRangeItemToInventar(Item item, int range)
        {
            if (Inventar.ContainsKey(item.TypeId))
            {
                Inventar[item.TypeId] += range;
            }
            else
            {
                Inventar[item.TypeId] = range;
            }
        }

        public Item RemoveItemFromInventar(Item item)
        {
            int anzahl = 1;
            if (item.GetType() == typeof(Munition))
                anzahl = ((Munition)item).Count;
            return RemoveRangeItemFromInventar(item, anzahl);
        }

        public Item RemoveRangeItemFromInventar(Item item, int range)
        {

            if (Inventar.ContainsKey(item.TypeId))
            {
                Inventar[item.TypeId] -= range;
                if (Inventar[item.TypeId] < 1)
                {
                    Inventar.Remove(item.TypeId);
                    return null;
                }
                return item;
            }
            return null;
        }
        public void AddWeaponToShortcutList(int key, Weapon weapon)
        {
            if (!shortcuts.ContainsKey(key))
            {
                shortcuts[key] = weapon;
            }
        }

        public void RemoveWeaponFromShortcutList(int key)
        {
            if (shortcuts.ContainsKey(key))
            {
                shortcuts.Remove(key);
            }
        }

        public float GetTotalWeight()
        {
            float sum = 0;
            foreach (KeyValuePair<int, int> pair in Inventar)
            {
                sum += Item.Get(pair.Key).Weight * pair.Value;
            }
            return sum;
        }

        public Dictionary<int, Weapon> GetShortcuts()
        {
            return shortcuts;
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
        public int ActiveShortcut
        {
            get { return activeShortcut; }
            set
            {
                if (shortcuts.ContainsKey(value))
                {
                    activeShortcut = value;
                }
            }
        }
        private int activeShortcut = 1;

        // ***************************************************************************
        // Konstruktor
        public Player(ILocationBehavior locationBehavior, float maxHealth, float health, float speed)
            : base(locationBehavior)
        {
            Buffs = new Dictionary<EBuffType, Buff>();
            Speed = speed;
            MaxHealth = maxHealth;
            Health = maxHealth;

            // Texturen für Renderer laden
            RendererStanding = LoadedRenderer.GetStatic("S_Player_Standing");
            RendererBigWeapon = LoadedRenderer.GetStatic("S_Player_BW");
            RendererBigWeaponShot = LoadedRenderer.GetStatic("S_Player_BW_Shot");
            RendererSmallWeapon = LoadedRenderer.GetStatic("S_Player_SW");
            RendererSmallWeaponShot = LoadedRenderer.GetStatic("S_Player_SW_Shot");

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
            Inventar = new Dictionary<int, int>();

            shortcuts = new Dictionary<int, Weapon>();
            // Schusstime auf 0 setzten
            shotTimer = 0;

            // Poweruplist init
            ActivePowerups = new List<Powerup>();

            // Liquids auf 0 setzten
            Liquids = new Vector3(0, 0, 0);

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

            // Effecte der Powerups zeichnen
            foreach (Powerup p in ActivePowerups)
            {
                p.Effect.Draw(spriteBatch, new MapLocation(LocationBehavior.Position, p.EffectSize));
            }
        }

        // ***************************************************************************
        // Player Update
        public void Update()
        {
            // NÄCHSTER WEGPUNKT ZUM PLAYER
            List<WayPoint> l = Main.MainObject.GameManager.GameState.Karte.QuadTreeWayPoints.GetObjects(new Rectangle((int)LocationBehavior.Position.X - 100, (int)LocationBehavior.Position.Y - 100, 200, 200));
            if (l.Count > 0)
            {
                NearestWayPoint = Karte.SearchNearest(LocationBehavior.Position, LocationBehavior.Size ,l);
            }

            // Renderer Updaten
            Renderer.Update();
            footRenderer.Update();

            // Waffe f+r cooldown etc
            Weapon.Update(Buffs);

            // Fußposition updaten
            footLocation.Position = LocationBehavior.Position;

            // ReloadTimer
            if (Reloading > 0)
                Reloading -= (float)Main.GameTimeUpdate.ElapsedGameTime.TotalSeconds;

            // ShotTimer verringer
            if (shotTimer > 0)
                shotTimer -= (float)Main.GameTimeUpdate.ElapsedGameTime.TotalSeconds;

            // Buff updaten
            Dictionary<EBuffType, Buff> tempDic = new Dictionary<EBuffType, Buff>(Buffs);
            foreach (Buff b in tempDic.Values)
            {
                // auswirkung
                switch (b.Type)
                {
                    case EBuffType.HealthRegenaration:
                        // Leben regenerieren
                        Health = Health + b.ValueSinceLast;
                        break;
                    case EBuffType.FireDamage:
                        // Leben regenerieren
                        Health = Health - b.ValueSinceLast;
                        break;
                }

                // Dauer updaten
                b.Update();

                // entfrnen falls abgelaufen
                if (b.IsExpired)
                    Buffs.Remove(b.Type);
            }

            // Poweruprenderer updaten und entfernen falls nötig
            List<Powerup> tempList = new List<Powerup>(ActivePowerups);
            foreach (Powerup p in tempList)
            {
                p.Effect.Update();

                p.Duration -= (float)Main.GameTimeUpdate.ElapsedGameTime.TotalSeconds;

                // entfernen wenn expired
                if (p.Duration < 0)
                    ActivePowerups.Remove(p);
            }
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
            if (Weapon.Cooldown <= 0 && Weapon.MunitionCount > 0)
            {
                // Schießt X mal (ShotCount der Waffe )
                Random r = new Random();

                for (int x = 0; x < Weapon.ShotCount && Weapon.MunitionCount > 0; x++)
                {
                    // Schuss von waffe erstellen lassen
                    Shot s = Weapon.CreateShot();

                    //ungenauigkeit: 10 - random - häflte (um 0 zentriert)
                    Vector2 accuracy = new Vector2((float)(r.NextDouble() * Weapon.AccuracyPercent - Weapon.AccuracyPercent / 2), (float)(r.NextDouble() * Weapon.AccuracyPercent - Weapon.AccuracyPercent / 2));

                    // Position und richtung des schussen berechnen
                    Vector2 direction = -LocationBehavior.Direction + accuracy; // + accuracy

                    // Flugrichtung/position setzten und rotation des schusses
                    s.LocationBehavior.Position = LocationBehavior.Position;
                    s.SetDirection(direction);
                    s.LocationSizing();

                    // schusscount ++
                    Main.MainObject.GameManager.GameState.Shots++;

                    // schuss adden
                    Main.MainObject.GameManager.GameState.ShotListVsEnemies.Add(s);
                }

                // Sound abspielen
                Sound.Sounds[Weapon.Antrieb.SoundId].Play();

                // Shottimer setzten
                shotTimer = ShotTime;
            }
            else if (Weapon.Cooldown <= 0 && Weapon.MunitionCount == 0)
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
            Health = Health - shot.Damage;

            // Buffs übernehmen
            AddBuffs(shot.Buffs);
        }

        // ***************************************************************************
        // Entfernt das Item aus dem UIInventarPanel
        public void UsePowerup(Powerup powerup)
        {
            // Aus inventar löschem
            Inventar.Remove(powerup.TypeId);

            // in active liste hinzufügen
            ActivePowerups.Add(powerup);

            // Buffs des powerups adden
            AddBuffs(powerup.Buffs);
        }

        // ***************************************************************************
        // Fügt von Liquid mit der ID id, die Menge amount hinzu
        public void AddLiquid(ELiquid type, int amount)
        {
            switch (type)
            {
                case ELiquid.Green:
                    Liquids = new Vector3(Liquids.X + amount, Liquids.Y, Liquids.Z);
                    break;
                case ELiquid.Blue:
                    Liquids = new Vector3(Liquids.X, Liquids.Y + amount, Liquids.Z);
                    break;
                case ELiquid.Red:
                    Liquids = new Vector3(Liquids.X, Liquids.Y, Liquids.Z + amount);
                    break;
            }
        }

        // ***************************************************************************
        // Zieht von Liquid mit der ID id, die Menge amount ab
        public void ReduceLiquid(ELiquid type, int amount)
        {
            switch (type)
            {
                case ELiquid.Green:
                    Liquids = new Vector3(Math.Max(0, Liquids.X - amount), Liquids.Y, Liquids.Z);
                    break;
                case ELiquid.Blue:
                    Liquids = new Vector3(Liquids.X, Liquids.Y - amount, Liquids.Z);
                    break;
                case ELiquid.Red:
                    Liquids = new Vector3(Liquids.X, Liquids.Y, Liquids.Z - amount);
                    break;
            }
        }

        // ***************************************************************************
        // Fügt die übergebene dic mit Buffs dem Player hinzu
        public void AddBuffs(Dictionary<EBuffType, Buff> buffs)
        {
            foreach (Buff b in buffs.Values)
            {
                if (this.Buffs.ContainsKey(b.Type))
                    this.Buffs[b.Type] = b;
                else
                    this.Buffs.Add(b.Type, b);
            }
        }

        // ***************************************************************************
        // Fügt die übergebene Liste mit Buffs dem Player hinzu
        public void AddBuffs(List<Buff> buffs)
        {
            foreach (Buff b in buffs)
            {
                if (this.Buffs.ContainsKey(b.Type))
                    this.Buffs[b.Type] = b;
                else
                    this.Buffs.Add(b.Type, b);
            }
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
