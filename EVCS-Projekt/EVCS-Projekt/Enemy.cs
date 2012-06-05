using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EVCS_Projekt.Location;
using EVCS_Projekt.Objects;
using Microsoft.Xna.Framework;
using EVCS_Projekt.Renderer;

namespace EVCS_Projekt
{
    public class Enemy: GameObject
    {
        public float Speed { get; private set; }
        public float Health{ get; private set; }  
        private float maxHealth;
        private float sightiningDistance;
        private float attackDistance;
        private float ratOfFire;
        private List<Buff> buffList;
        public int TypOfEnemy { get; private set; }
        public bool IsDead { get { if (Health <= 0F) return true; else return false; } }

        // Vordefinierte Gegner
        public static Dictionary<EEnemyType, Enemy> DefaultEnemies { get; set; }

        public Enemy (Enemy e, Vector2 position):
            this(e.LocationBehavior, e.Renderer, e.ratOfFire, e.attackDistance, e.sightiningDistance, e.maxHealth, e.Speed, e.Health, e.TypOfEnemy)
        {
            LocationBehavior.Position = position;
        }
        
        public Enemy(ILocationBehavior locationBehavoir, float ratOfFire, float attackDistance,
            float sightiningDistance, float maxHealth, float speed, float health, int typeOfEnemy)
            : this(locationBehavoir, new NoRenderer(), ratOfFire, attackDistance, sightiningDistance, maxHealth, speed,health, typeOfEnemy)
        {
        }

        public Enemy(ILocationBehavior locationBehavoir, IRenderBehavior renderBehavior ,float ratOfFire,float attackDistance,
            float sightiningDistance, float maxHealth,  float speed, float health, int typeOfEnemy)
            : base(locationBehavoir, renderBehavior)
        {
            this.buffList = new List<Buff>();
            this.ratOfFire = ratOfFire;
            this.attackDistance = attackDistance;
            this.sightiningDistance = sightiningDistance;
            this.maxHealth = maxHealth;
            TypOfEnemy = typeOfEnemy;

            this.Speed = speed;
            this.Health = health;
        }

        // Clont den Gegner
        public Enemy Clone()
        {
            Enemy c = new Enemy(LocationBehavior.Clone(), Renderer.Clone(), ratOfFire, attackDistance, sightiningDistance, maxHealth, Speed, Health, TypOfEnemy);
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
            
        }




        // Hier ist es etwas seltsam, dass der Gegner ein Item bekommt, welches es dann auch glei droppt. Ausserdem ist die 
        // Methode private. Wer soll sie aufrufen?
        // -- Stan: Die Idee war, dass diese Methode von der Die()-Methode aufgerufen wird, wenn der Gegner verreckt.s
        private void Drop(Item item)
        {
            
        }

        public void AddBuffs(List<Buff> buffs)
        {
        }

       
            
    }
}
