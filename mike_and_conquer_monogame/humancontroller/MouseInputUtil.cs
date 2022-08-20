using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using mike_and_conquer_monogame.gameview;

namespace mike_and_conquer_monogame.humancontroller
{
    public class MouseInputUtil
    {

        public static bool LeftMouseButtonClicked(MouseState newMouseState, MouseState oldMouseState)
        {
            return newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released;
        }

        public static bool LeftMouseButtonIsBeingHeldDown(MouseState newMouseState, MouseState oldMouseState)
        {
            return newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton != ButtonState.Released;
        }


        public static bool RightMouseButtonClicked(MouseState newMouseState, MouseState oldMouseState)
        {
            return newMouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released;
        }

        public static bool IsOverSidebar(MouseState newMouseState)
        {
            Point mousePoint = newMouseState.Position;
            Vector2 mouseScreenLocation = new Vector2(mousePoint.X, mousePoint.Y);

            Vector2 sidebarLocation = GameWorldView.instance.ConvertScreenLocationToSidebarLocation(mouseScreenLocation);

            if (sidebarLocation.X > 0 && sidebarLocation.Y > 0)
            {
                return true;
            }

            return false;

        }

        public static bool LeftMouseButtonUnclicked(MouseState newMouseState, MouseState oldMouseState)
        {
            return newMouseState.LeftButton == ButtonState.Released && oldMouseState.LeftButton == ButtonState.Pressed;
        }


        public static Point GetWorldLocationPointFromMouseState(MouseState mouseState)
        {
            Vector2 mouseScreenLocation = new Vector2(mouseState.X, mouseState.Y);
            Vector2 mouseWorldLocationVector2 = GameWorldView.instance.ConvertScreenLocationToWorldLocation(mouseScreenLocation);
            return new Point((int)mouseWorldLocationVector2.X, (int)mouseWorldLocationVector2.Y);
        }

        public static Point GetSidebarWorldLocationPointFromMouseState(MouseState mouseState)
        {
            Vector2 mouseScreenLocation = new Vector2(mouseState.X, mouseState.Y);
            Vector2 sidebarLocation = GameWorldView.instance.ConvertScreenLocationToSidebarLocation(mouseScreenLocation);
            return new Point((int)sidebarLocation.X, (int)sidebarLocation.Y);
        }



    }
}
