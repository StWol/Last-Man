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

    public class Buff : Microsoft.Xna.Framework.GameComponent
    {

        //Attributes

        private EBuffType type;
        private String name;
        private float value;
        private float modifier;
        private float duration;
        private bool expired;

        public Buff(Game game, String name, float value, float modifier, float duration, EBuffType type, bool expired)
            : base(game)
        {
            this.name = name;
            this.value = value;
            this.modifier = modifier;
            this.duration = duration;
            this.type = type;
            this.expired = false;
        }

        
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        
        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }



        // Getter 'n' Setter

        public String GetName()
        {
            return name;
        }

        public float GetValue()
        {
            return value;
        }

        public float GetModifikation()
        {
            return modifier;
        }

        public EBuffType GetBuffType()
        {
            return type;
        }


        //other Functions

        public void UpdateDuration()
        {

        }

        public bool IsExpired()
        {
            return expired;
        }
    }
}

        
        


        