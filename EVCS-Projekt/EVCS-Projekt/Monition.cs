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
    public class Monition : Microsoft.Xna.Framework.GameComponent
    {

        //Attributes

        private List<Buff> buffList;

        public Monition(Game game, int id, EGroup group, String name, List<int> buffIDs)
            : base(game)
        {
            
        }


        public override void Initialize()
        {


            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {


            base.Update(gameTime);
        }

        public List<Buff> GetBuffList()
        {
            return buffList;
        }
    }
}
