using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace EVCS_Projekt
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
            this.buffList = buffList;
            this.distance = distance;
        }


        //Functions
        public float GetDamage()
        {
            return damage;
        }
    }
}
