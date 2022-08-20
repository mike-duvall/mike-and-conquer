using String = System.String;

using GameTime = Microsoft.Xna.Framework.GameTime;

namespace mike_and_conquer_monogame.gamestate
{
    public abstract class GameState
    {
        abstract public String GetName();
        abstract public GameState Update(GameTime gameTime);
    }
}
