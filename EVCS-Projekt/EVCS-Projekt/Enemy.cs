using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EVCS_Projekt.Location;
using EVCS_Projekt.Objects;
using Microsoft.Xna.Framework;

namespace EVCS_Projekt
{
    class Enemy:GameObject
    {
        public float Speed { get; private set; }
        public float Health{ get; private set; }
        
        private float maxHealth;
        private float sightiningDistance;
        private float attackDistance;
        private float ratOfFire;
        private List<Buff> buffList;

        public Enemy(Game game,ILocationBehavior locationBehavoir,float ratOfFire,float attackDistance,
            float sightiningDistance, float maxHealth,  float speed, float health)
            :base(game,locationBehavoir)
        {
            this.buffList = new List<Buff>();
            this.ratOfFire = ratOfFire;
            this.attackDistance = attackDistance;
            this.sightiningDistance = sightiningDistance;
            this.maxHealth = maxHealth;
            
            Speed = speed;
            Health = health;
        }

        private void Die()
        {
            
        }

        public void TakeDamage()
        {
            
        }

        public void Attack()
        {
            
        }


        // Hier ist es etwas seltsam, dass der Gegner ein Item bekommt, welches es dann auch glei droppt. Ausserdem ist die 
        // Methode private. Wer soll sie aufrufen?
        private void Drop(Item item)
        {
            
        }

        public void AddBuffs(List<Buff> buffs )
        {
            
        }

        //weiss nicht, ob man das braucht.
        public void AddBuff(Buff buff)
        {
            
        }
    }
}
