using System;
using System.Collections.Generic;
using EVCS_Projekt.Location;
using Microsoft.Xna.Framework;

namespace EVCS_Projekt.Objects.Items
{
    /**
     * @UMLVersion = 12.Mai2012
     * @Last Changes = 12.Mai2012
     *
     */
    public class Shot : Item
    {
        //Attributes
        private Vector2 direction;
        private float speed;
        private float damage;
        private float distance;
        private List<Buff> buffList;

        //Constructor
        public Shot(Game game, int id, EGroup group, float speed, Vector2 direction, float damage, String name, float distance, string description, float weight, ILocationBehavior locationBehavior)
            : base( game,  id,  group,  name,  description,  weight,  locationBehavior)
        {
            this.speed = speed;
            this.direction = direction;
            this.damage = damage;
            this.buffList = new List<Buff>();
            this.distance = distance;
        }


        //Functions
        public float GetDamage()
        {
            return 0;
        }
    }
}