
using Microsoft.Xna.Framework.Input;

namespace mike_and_conquer_monogame.humancontroller
{
    public abstract class HumanControllerState
    {
        public abstract HumanControllerState Update(
            MouseState newMouseState,
            MouseState oldMouseState);

    }
}

