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

    public class Buff
    {

        //Attributes
        public float Duration { get; private set; }
        public float Modifier { get; private set; }
        public float Value { get; private set; }
        public EBuffType Type { get; private set; }
        public string Name { get; private set; }

        private bool expired;

        //Constructor
        public Buff(Game game, String name, float value, float modifier, float duration, EBuffType type, bool expired)
        {
            this.Name = name;
            this.Value = value;
            this.Modifier = modifier;
            this.Duration = duration;
            this.Type = type;
            this.expired = false;
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





