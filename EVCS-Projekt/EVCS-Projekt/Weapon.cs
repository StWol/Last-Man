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
    public class Weapon : Microsoft.Xna.Framework.GameComponent
    {

        //Attributes
        private Visier visier;
        private Munition munition;
        private Antrieb antrieb;
        private Stabilisator stabilisator;
        private Hauptteil hauptteil;

        //Constructor
        public Weapon(Game game, Visier visier, Antrieb antrieb, Stabilisator stabilisator, Hauptteil hauptteil, int id, EGroup group, String name, float weight)
            : base(game)
        {
            this.visier = visier;
            this.antrieb = antrieb;
            this.stabilisator = stabilisator;
            this.hauptteil = hauptteil;
        }

        //Functions
        public Item Shoot (float accuratcy) 
        {
            return null; //Schuss generiert neues Item
        }

        public Munition GetMunition()
        {
            return this.munition;
        }

        public void SetMunition (Munition newMunition)
        {
            this.munition= newMunition;
        }

    }
}
