using Microsoft.Xna.Framework.Graphics;

namespace LastMan.Managers
{
    public abstract class Manager
    {
        public abstract void Update();
        public abstract void Draw(SpriteBatch batch);
    }
}
