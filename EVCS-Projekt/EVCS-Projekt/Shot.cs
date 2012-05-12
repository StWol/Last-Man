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
    public class Shot : Microsoft.Xna.Framework.GameComponent
    {
        //Attributes
        private Vector2 direction;
        private float speed;
        private float damage;
        private float distance;
        private List<Buff> buffList;

        public Shot(Game game, int id, EGroup group, float speed, Vector2 direction, float damage, String name, float distance)
            : base(game)
        {
            this.speed = speed;
            this.direction = direction;
            this.damage = damage;
            this.buffList = buffList;
            this.distance = distance;
        }

        
        public override void Initialize()
        {


            base.Initialize();
        }

        
        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }


        //Functions

        public float GetDamage()
        {
            return damage;
        }
    }
}
