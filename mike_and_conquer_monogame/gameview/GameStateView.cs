
using GameTime = Microsoft.Xna.Framework.GameTime;

namespace mike_and_conquer_monogame.gameview
{
    abstract class GameStateView
    {
        public abstract void Draw(GameTime gameTime);
        public abstract void Update(GameTime gameTime);
    }
}
