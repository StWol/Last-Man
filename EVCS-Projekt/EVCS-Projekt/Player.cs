using System;
using System.Collections.Generic;
using System.Linq;
using LastMan.Audio;
using LastMan.Location;
using LastMan.Managers;
using LastMan.Map;
using LastMan.Objects;
using LastMan.Objects.Items;
using LastMan.Renderer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LastMan
{
    public class Player : GameObject
    {
        //  Wie lange das schussbild gezeigt wird
        private const float ShotTime = 0.05F;
        public Dictionary<EBuffType, Buff> Buffs;

        //Attributes
        private float _health;
        private float _waypointTimeout = 1F;
        private int _activeShortcut = 1;
        private readonly MapLocation _footLocation;
        private IRenderBehavior _footRenderer;
        private bool _isMoving;
        private float _shotTimer;

        /// <summary>
        ///   Kosntruktor
        /// </summary>
        /// <param name="locationBehavior"> Position des Spielers auf der Map </param>
        /// <param name="maxHealth"> Maximal Leben </param>
        /// <param name="health"> Aktuelles Leben </param>
        /// <param name="speed"> Bewegungsgeschwindigkeit </param>
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

            RendererDying = LoadedRenderer.GetAnimation("A_Splatter_01");
            RendererDying.PlayOnce();

            Texture2D[] textureFootMoving = new[]
                                                {
                                                    Main.ContentManager.Load<Texture2D>("images/character/left_foot"),
                                                    Main.ContentManager.Load<Texture2D>("images/character/right_foot")
                                                };
            RendererFootMoving = new AnimationRenderer(textureFootMoving, 4F);

            _footRenderer = new NoRenderer();

            // Location für die Füße
            _footLocation = new MapLocation(locationBehavior.Position,
                                           new Vector2(textureFootMoving[0].Width, textureFootMoving[0].Height));

            // Standardmäßig den StandingRenderer zuweisen
            //Renderer = RendererBigWeapon;

            // LocationSize anpassen
            LocationSizing();
            LocationBehavior.Size = Renderer.Size; // Sollte LocationSizing machen

            // InventarListe init
            Inventar = new Dictionary<int, int>();

            Shortcuts = new Dictionary<int, Weapon>();
            // Schusstime auf 0 setzten
            _shotTimer = 0;

            // Poweruplist init
            ActivePowerups = new List<Powerup>();

            // Liquids auf 0 setzten
            Liquids = new Vector3(0, 0, 0);

            // Rect Methode setzten
            base.GetRect = base.RectPlayer;
        }

        public Dictionary<int, Weapon> Shortcuts { get; private set; }

        /// <summary>
        ///   Maximales Leben
        /// </summary>
        public float MaxHealth { get; private set; }

        /// <summary>
        ///   Aktuelles Leben
        /// </summary>
        public float Health
        {
            get { return _health; }
            set
            {
                // Schaden für HS speichern
                if (value < _health)
                {
                    Main.MainObject.GameManager.GameState.GameStatistic.DamageTaken += _health - value;
                }

                _health = value;
                if (_health > MaxHealth)
                    _health = MaxHealth;
            }
        }

        /// <summary>
        ///   Bewegungsgeschwindigkeit
        /// </summary>
        public float Speed { get; private set; }

        /// <summary>
        ///   Ist Spieler am Leben
        /// </summary>
        public bool IsAlive
        {
            get { return Health > 0; }
        }

        /// <summary>
        ///   Gewicht des Inventars
        /// </summary>
        public float TotalInventarWeight
        {
            get
            {
                float nventarWeight = Inventar.Sum(pair => Item.Get(pair.Key).Weight * pair.Value);
                float shortcutWeight = Shortcuts.Sum(pair => (pair.Value).Weight);
                return nventarWeight + shortcutWeight;
            }
        }

        /// <summary>
        ///   Gibt ausgewählte Waffe zurück
        /// </summary>
        public Weapon Weapon
        {
            get
            {
                if (Shortcuts != null)
                {
                    return Shortcuts.ContainsKey(ActiveShortcut) ? Shortcuts[ActiveShortcut] : Shortcuts.Values.ToList()[0];
                }
                return null;
            }
        }

        /// <summary>
        ///   Items, welche der Spieler im Inventar hat
        /// </summary>
        public Dictionary<int, int> Inventar { get; private set; }

        /// <summary>
        ///   Powerups, welche gerade aktiv sind
        /// </summary>
        public List<Powerup> ActivePowerups { get; private set; }

        /// <summary>
        ///   Richtung in die der Spieler schaut
        /// </summary>
        public Vector2 Direction { get; set; }

        /// <summary>
        ///   Wegpunkt, der dem Spieler am nähsten ist
        /// </summary>
        public WayPoint NearestWayPoint { get; private set; }

        /// <summary>
        ///   Stand der Flüssigkeiten
        /// </summary>
        public Vector3 Liquids { get; set; }

        /// <summary>
        ///   Handelt es sich bei der ausgewählten Waffe um eine "große" Waffe oder um eine kleine. Einfluss auf den Renderer.
        /// </summary>
        public bool BigWeapon { get; set; }

        /// <summary>
        ///   Gibt aktuellen Renderer zurück. Je nach dem ob der Spieler tot ist, steht, eine große oder kleine Waffe hat..
        /// </summary>
        public new IRenderBehavior Renderer
        {
            get
            {
                if (!IsAlive)
                    return RendererDying;

                if (Weapon == null)
                    return RendererStanding;

                if (Weapon.BigWeapon)
                {
                    return _shotTimer > 0 ? RendererBigWeaponShot : RendererBigWeapon;
                }
                else
                {
                    return _shotTimer > 0 ? RendererSmallWeaponShot : RendererSmallWeapon;
                }
            }
        }

        /// <summary>
        ///   Zeit die der Spieler noch am Nachladen ist
        /// </summary>
        public float Reloading { get; set; }

        /// <summary>
        ///   Drehung der Füße - je nachdem in welche Richtung der Spieler läuft
        /// </summary>
        public float FootRotation
        {
            set { _footLocation.Rotation = value; }
        }

        /// <summary>
        ///   Bewegt sich der Spieler gerade
        /// </summary>
        public bool IsMoving
        {
            get { return _isMoving; }
            set
            {
                if (_isMoving == value)
                    return;

                _isMoving = value;
                if (_isMoving)
                {
                    //Renderer = RendererMoving;
                    _footRenderer = RendererFootMoving;
                }
                else
                {
                    //Renderer = RendererStanding;
                    _footRenderer = new NoRenderer();
                }
            }
        }

        // Verschiedene Renderer
        private StaticRenderer RendererStanding { get; set; }
        private StaticRenderer RendererBigWeapon { get; set; }
        private StaticRenderer RendererBigWeaponShot { get; set; }

        private StaticRenderer RendererSmallWeapon { get; set; }
        private StaticRenderer RendererSmallWeaponShot { get; set; }

        private AnimationRenderer RendererDying { get; set; }

        // Füße
        private AnimationRenderer RendererFootMoving { get; set; }

        /// <summary>
        ///   Aktiver Weapon Shortcut
        /// </summary>
        public int ActiveShortcut
        {
            get { return _activeShortcut; }
            set
            {
                if (Shortcuts.ContainsKey(value))
                {
                    _activeShortcut = value;
                }
            }
        }

        /// <summary>
        ///   Kleinere BoundingBox als "Rect"
        /// </summary>
        public Rectangle LittleBoundingBox
        {
            get
            {
                int s = (int)LocationBehavior.Size.Y;
                return new Rectangle((int)(LocationBehavior.Position.X - s / 2F),
                                     (int)(LocationBehavior.Position.Y - s / 2F), s, s);
            }
        }

        /// <summary>
        ///   Gesamtgewicht des Invetars, ohne Shortcuts
        /// </summary>
        /// <returns> </returns>
        public float TotalWeight
        {
            get { return Inventar.Sum(pair => Item.Get(pair.Key).Weight * pair.Value); }
        }

        /// <summary>
        ///   Gibt die im Invetar enthaltene Anzahl des gewünschten Items zurück
        /// </summary>
        /// <param name="key"> key des Items dessen Menge man haben will </param>
        /// <returns> Menge des angegeben Items </returns>
        public int GetItemCountFromInventar(int key)
        {
            return Inventar.ContainsKey(key) ? Inventar[key] : 0;
        }

        /// <summary>
        ///   Item in das Inventar hinzufügen
        /// </summary>
        /// <param name="item"> Item, dass hinzugefügt wird </param>
        public void AddItemToInventar(Item item)
        {
            int anzahl = 1;
            if (item.GetType() == typeof(Munition))
                anzahl = ((Munition)item).Count;
            AddRangeItemToInventar(item, anzahl);
        }

        /// <summary>
        ///   Fügt eine gewisse Anzahl eines Items in das Inventar
        /// </summary>
        /// <param name="item"> Itemtyp das hinzugefüt wird </param>
        /// <param name="range"> Wieviel des angegebenen Items wird hinzugefügt </param>
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

        /// <summary>
        ///   Entfernt ein Item aus dem Inventar
        /// </summary>
        /// <param name="item"> Item, dass entfernt wird </param>
        /// <returns> Typ des Items wird zurückgegeben </returns>
        public Item RemoveItemFromInventar(Item item)
        {
            int anzahl = 1;
            if (item.GetType() == typeof(Munition))
                anzahl = ((Munition)item).Count;
            return RemoveRangeItemFromInventar(item, anzahl);
        }

        /// <summary>
        ///   Mehrere Items des selben Typs entfernen
        /// </summary>
        /// <param name="item"> Itemtyp der entfernt wird </param>
        /// <param name="range"> Wieviele Items entfernt werden </param>
        /// <returns> Typ des Items wird zurückgegeben </returns>
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

        /// <summary>
        ///   Waffe einem Shortcut hinzufügen
        /// </summary>
        /// <param name="key"> Shortcutplatz </param>
        /// <param name="weapon"> Waffe die hinzugefügt wird </param>
        public void AddWeaponToShortcutList(int key, Weapon weapon)
        {
            if (!Shortcuts.ContainsKey(key))
            {
                Shortcuts[key] = weapon;
            }
        }

        /// <summary>
        ///   Waffe aus Shortcut entfernen
        /// </summary>
        /// <param name="key"> Shortcutplatz </param>
        public void RemoveWeaponFromShortcutList(int key)
        {
            if (Shortcuts.ContainsKey(key))
            {
                Shortcuts.Remove(key);
            }
        }

        /// <summary>
        ///   Zeichnet Füße, Oberteil und Effekte der Buffs, falls welche vorhanden sind
        /// </summary>
        /// <param name="spriteBatch"> </param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // Füße zeichnen
            _footRenderer.Draw(spriteBatch, _footLocation);

            // Character oberteil
            float angle = LocationBehavior.Rotation;
            if (IsMoving)
                LocationBehavior.Rotation = LocationBehavior.Rotation +
                                            (float)(Math.Sin(Main.GameTimeDraw.TotalGameTime.TotalSeconds * 12) / 6);
            Renderer.Draw(spriteBatch, LocationBehavior);
            LocationBehavior.Rotation = angle;

            // Effecte der Powerups zeichnen
            foreach (Powerup p in ActivePowerups)
            {
                p.Effect.Draw(spriteBatch, new MapLocation(LocationBehavior.Position, p.EffectSize));
            }
        }

        /// <summary>
        ///   Update der Renderer, Timer, ...
        /// </summary>
        public void Update()
        {
            // NÄCHSTER WEGPUNKT ZUM PLAYER
            List<WayPoint> l =
                Main.MainObject.GameManager.GameState.Karte.QuadTreeWayPoints.GetObjects(
                    new Rectangle((int)LocationBehavior.Position.X - 100, (int)LocationBehavior.Position.Y - 100, 200,
                                  200));
            if (l.Count > 0)
            {
                NearestWayPoint = Karte.SearchNearest(LocationBehavior.Position, LocationBehavior.Size, l);
            }

            // Renderer Updaten
            Renderer.Update();
            _footRenderer.Update();

            // Waffe f+r cooldown etc
            Weapon.Update(Buffs);

            // Fußposition updaten
            _footLocation.Position = LocationBehavior.Position;

            // ReloadTimer
            if (Reloading > 0)
                Reloading -= (float)Main.GameTimeUpdate.ElapsedGameTime.TotalSeconds;

            // ShotTimer verringer
            if (_shotTimer > 0)
                _shotTimer -= (float)Main.GameTimeUpdate.ElapsedGameTime.TotalSeconds;

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

        /// <summary>
        ///   Player schießt mit aktiver Waffe in die Richtung, in die er gerade schaut
        /// </summary>
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
                    Vector2 accuracy =
                        new Vector2((float)(r.NextDouble() * Weapon.AccuracyPercent - Weapon.AccuracyPercent / 2),
                                    (float)(r.NextDouble() * Weapon.AccuracyPercent - Weapon.AccuracyPercent / 2));

                    // Position und richtung des schussen berechnen
                    Vector2 direction = -LocationBehavior.Direction + accuracy; // + accuracy

                    // Flugrichtung/position setzten und rotation des schusses
                    s.LocationBehavior.Position = LocationBehavior.Position;
                    s.SetDirection(direction);
                    s.LocationSizing();

                    // schusscount ++
                    Main.MainObject.GameManager.GameState.GameStatistic.Shots++;

                    // schuss adden
                    Main.MainObject.GameManager.GameState.ShotListVsEnemies.Add(s);
                }

                // Sound abspielen
                Sound.Sounds[Weapon.Antrieb.SoundId].Play();

                // Shottimer setzten
                _shotTimer = ShotTime;
            }
            else if (Weapon.Cooldown <= 0 && Weapon.MunitionCount == 0)
            {
                // Waffe leer
                Weapon.ResetCooldown();

                // Sound abspielen
                Sound.Sounds["Weapon_Empty"].Play();
            }
        }

        /// <summary>
        ///   Player wird von Schuss getroffen. Bekommt Damage und die Buffs des Schusses
        /// </summary>
        /// <param name="shot"> Schuss der den Spieler trifft </param>
        public void TakeDamage(Shot shot)
        {
            // Schaden abziehen
            Health = Health - shot.Damage;

            // Buffs übernehmen
            AddBuffs(shot.Buffs);
        }

        /// <summary>
        ///   Powerup benutzen und aus Inventar entfernen
        /// </summary>
        /// <param name="powerup"> Powerup, welches verbaucht wird </param>
        public void UsePowerup(Powerup powerup)
        {
            // Aus inventar löschem
            Inventar.Remove(powerup.TypeId);

            // in active liste hinzufügen
            ActivePowerups.Add(powerup);

            // Buffs des powerups adden
            AddBuffs(powerup.Buffs);
        }

        /// <summary>
        ///   Fügt dem Liquid des Types "type", die Menge "amount" hinzu
        /// </summary>
        /// <param name="type"> Zu welchem Liquid hinzugefügt wird </param>
        /// <param name="amount"> Menge die hinzugefügt wird </param>
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

        /// <summary>
        ///   Zieht von dem Liquid des Types "type", die Menge "amount" ab
        /// </summary>
        /// <param name="type"> Von welchem Liquid abgezogen wird </param>
        /// <param name="amount"> Menge die abgezogen wird </param>
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

        /// <summary>
        ///   Erhöht die Anzahl der Flüssigkeiten
        /// </summary>
        /// <param name="liquid"> Anzahl um wieviel die Flüssigkeiten erhöht werden </param>
        public void AddLiquid(Vector3 liquid)
        {
            Liquids += liquid;
        }

        /// <summary>
        ///   Reduziert die Anzahl der Flöüssigkeiten
        /// </summary>
        /// <param name="liquid"> Vector mit Verbrauchsanzahl </param>
        public void ReduceLiquid(Vector3 liquid)
        {
            Liquids -= liquid;
        }

        /// <summary>
        ///   Fügt die übergebene Buffdictionary dem Player hinzu
        /// </summary>
        /// <param name="buffs"> Buffs die hinzugefügt werden </param>
        public void AddBuffs(Dictionary<EBuffType, Buff> buffs)
        {
            foreach (Buff b in buffs.Values)
            {
                if (Buffs.ContainsKey(b.Type))
                    Buffs[b.Type] = b;
                else
                    Buffs.Add(b.Type, b);
            }
        }

        /// <summary>
        ///   Fügt die übergebene Buffliste dem Player hinzu
        /// </summary>
        /// <param name="buffs"> Buffs die hinzugefügt werden </param>
        public void AddBuffs(List<Buff> buffs)
        {
            foreach (Buff b in buffs)
            {
                if (Buffs.ContainsKey(b.Type))
                    Buffs[b.Type] = b;
                else
                    Buffs.Add(b.Type, b);
            }
        }


        /// <summary>
        ///   Prüft ob Player mit der Map kolidiert, wenn er an Position des Vectors steht
        /// </summary>
        /// <param name="newPosition"> Position die grpüft wird </param>
        /// <returns> bool ob Player an der Position stehen kann </returns>
        public bool CheckPosition(Vector2 newPosition)
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