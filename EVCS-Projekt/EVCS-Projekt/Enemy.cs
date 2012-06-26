using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EVCS_Projekt.Location;
using EVCS_Projekt.Objects;
using Microsoft.Xna.Framework;
using EVCS_Projekt.Renderer;
using EVCS_Projekt.Objects.Items;
using System.Diagnostics;
using EVCS_Projekt.AI;
using EVCS_Projekt.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace EVCS_Projekt
{
    public class Enemy : GameObject
    {
        public float Speed { get; private set; }
        public float Health { get; private set; }
        public float MaxHealth { get; private set; }
        public float SightiningDistance { get; private set; }
        public float AttackDistance { get; private set; }
        private float ratOfFire;
        private double lastAttack;
        private List<Buff> buffList;
        public EEnemyType TypOfEnemy { get; private set; }
        public bool IsDead { get { if (Health <= 0F) return true; else return false; } }
        public bool CanSeePlayer { get; set; }
        public float Damage { get; set; }

        // Action die der Gegner ausführt
        public Activity Activity { get; set; }

        // Vordefinierte Gegner
        public static Dictionary<EEnemyType, Enemy> DefaultEnemies { get; set; }


        private IRenderBehavior moveRenderer;
        private IRenderBehavior standRenderer;

        public new IRenderBehavior Renderer
        {
            get
            {
                if (HasMoved)
                    return moveRenderer;
                else
                    return standRenderer;
            }
            set
            {
            }
        }


        public Enemy(Enemy e, Vector2 position) :
            this(e.LocationBehavior, e.Renderer, e.ratOfFire, e.AttackDistance, e.SightiningDistance, e.MaxHealth, e.Speed, e.Health, e.TypOfEnemy)
        {
            LocationBehavior.Position = position;
        }

        public Enemy(ILocationBehavior locationBehavoir, float ratOfFire, float attackDistance,
            float sightiningDistance, float maxHealth, float speed, float health, EEnemyType typeOfEnemy)
            : this(locationBehavoir, new NoRenderer(), ratOfFire, attackDistance, sightiningDistance, maxHealth, speed, health, typeOfEnemy)
        {
        }

        public Enemy(ILocationBehavior locationBehavoir, IRenderBehavior renderBehavior, float ratOfFire, float attackDistance,
            float sightiningDistance, float maxHealth, float speed, float health, EEnemyType typeOfEnemy)
            : base(locationBehavoir, renderBehavior)
        {
            this.buffList = new List<Buff>();
            this.ratOfFire = ratOfFire;
            this.AttackDistance = attackDistance;
            this.SightiningDistance = sightiningDistance;
            this.MaxHealth = maxHealth;
            TypOfEnemy = typeOfEnemy;

            lastAttack = 0;

            this.Speed = speed;
            this.Health = health;

            switch ( typeOfEnemy) {
                case EEnemyType.E1:
                    moveRenderer = LoadedRenderer.Get("A_Krabbler_Move");
                    standRenderer = LoadedRenderer.Get("A_Krabbler_Stand");
                    break;
                case EEnemyType.E2:
                    moveRenderer = LoadedRenderer.Get("A_Hellboy_Move");
                    standRenderer = LoadedRenderer.Get("A_Hellboy_Stand");
                    break;
            }

            this.Activity = new NoActivity();
        }

        // Update des Enemy (Renderer etc)
        public void Update()
        {
            // Renderer updaten
            moveRenderer.Update();
            standRenderer.Update();

            // Buff updaten
            foreach (Buff b in buffList)
            {
                b.Update();
            }
        }

        // Clont den Gegner
        public Enemy Clone()
        {
            Enemy c = new Enemy(LocationBehavior.Clone(), Renderer.Clone(), ratOfFire, AttackDistance, SightiningDistance, MaxHealth, Speed, Health, TypOfEnemy);
            c.Damage = Damage;
            return c;
        }

        /*
        *  Die(), das DropItem() und das Prüfen, ob ein Enemy gestorben ist - muss der GameManager checken und
        *  behandeln!
        */

        public void TakeDamage(Objects.Items.Shot shot)
        {
            this.Health -= shot.Damage;
        }

        public void Attack()
        {
            // Schießt ein Shot auf den Player
            if (Main.GameTimeUpdate.TotalGameTime.TotalSeconds - lastAttack > ratOfFire)
            {
                lastAttack = Main.GameTimeUpdate.TotalGameTime.TotalSeconds;

                // Schuss erstellen
                Shot s = new Shot(0, 0, 1000, -LocationBehavior.Direction, Damage, "", 0, "", 0, new MapLocation(LocationBehavior.Position));

                // Je nach Gegner andere SchussGrafik
                switch (TypOfEnemy)
                {
                    case EEnemyType.E1:
                        s.Renderer = LoadedRenderer.DefaultRenderer["S_Shot_Monster_01"];
                        break;
                    default:
                        s.Renderer = LoadedRenderer.DefaultRenderer["S_Shot_Normal"];
                        break;
                }

                // TEST BUFF
                s.AddBuff(new Buff(0, 10, 5, EBuffType.FireDamage));

                s.SetDirection(-LocationBehavior.Direction);
                s.LocationSizing();

                // Play Sound
                Sound.Sounds["Monster_Attack_02"].Play();

                // Schuss in gameState liste aufnehmen
                Main.MainObject.GameManager.GameState.ShotListVsPlayer.Add(s);
            }
        }

        // Zeichner die heatlhbar
        public void DrawHealthBar(SpriteBatch spriteBatch)
        {
            int x = (int)(LocationBehavior.RelativePosition.X - LocationBehavior.Size.X / 2);
            int y = (int)(LocationBehavior.RelativePosition.Y - LocationBehavior.Size.Y / 2 - 4);
            int w = (int)(LocationBehavior.Size.X);
            int h = 4;

            int w2 = (int)(LocationBehavior.Size.X / MaxHealth * Health);

            spriteBatch.Draw(Main.MainObject.GameManager.PixelWhite, new Rectangle(x, y, w, h), new Color(255, 0, 0, 64));
            spriteBatch.Draw(Main.MainObject.GameManager.PixelWhite, new Rectangle(x, y, w2, h), new Color(255, 0, 0, 192));
        }

        // Berechnet was die neue activität genau machen soll
        public void CalculateActivity()
        {
            Activity.CalculateAction(this);
        }

        // Macht die Activität
        public void DoActivity()
        {
            Activity.DoActivity(this);
        }

        // Hier ist es etwas seltsam, dass der Gegner ein Item bekommt, welches es dann auch glei droppt. Ausserdem ist die 
        // Methode private. Wer soll sie aufrufen?
        // -- Stan: Die Idee war, dass diese Methode von der Die()-Methode aufgerufen wird, wenn der Gegner verreckt.s
        private void Drop(Item item)
        {

        }

        public void AddBuffs(Dictionary<EBuffType, Buff> buffs)
        {
        }



    }
}
