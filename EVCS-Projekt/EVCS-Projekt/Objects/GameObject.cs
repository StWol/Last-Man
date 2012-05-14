using EVCS_Projekt.Location;
using EVCS_Projekt.Renderer;
using Microsoft.Xna.Framework;

namespace EVCS_Projekt.Objects
{

    public abstract class GameObject : Microsoft.Xna.Framework.GameComponent
    {

        private IRenderBehavior renderBehavoir;
        private ILocationBehavior locationBehavior;


        public GameObject(Game game, ILocationBehavior locationBehavior)
            : base(game)
        {
            this.locationBehavior = locationBehavior;
        }

 
        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }
    }
}
