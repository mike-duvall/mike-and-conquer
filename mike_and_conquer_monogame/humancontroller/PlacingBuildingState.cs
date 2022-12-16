

using MouseState = Microsoft.Xna.Framework.Input.MouseState;
using Point = Microsoft.Xna.Framework.Point;

using GameWorldView = mike_and_conquer_monogame.gameview.GameWorldView;

namespace mike_and_conquer_monogame.humancontroller
{
    class PlacingBuildingState : HumanControllerState
    {

        public override HumanControllerState Update(MouseState newMouseState, MouseState oldMouseState)
        {

            GameWorldView.instance.Notify_PlacingBarracks();

            if (!MouseInputUtil.IsOverSidebar(newMouseState))
            {

                if (!newMouseState.Equals(oldMouseState))
                {
                    GameWorldView.instance.Notify_PlacingBarracksWithMouseOverMap(newMouseState.Position);
                }


                // if (MouseInputUtil.LeftMouseButtonUnclicked(newMouseState, oldMouseState))
                // {
                //     if(GameWorldView.instance.barracksPlacementIndicator.ValidBuildingLocation())
                //     {
                //         GDIConstructionYard gdiConstructionYard = GameWorld.instance.GDIConstructionYard;
                //         Point mouseWorldLocationPoint = MouseInputUtil.GetWorldLocationPointFromMouseState(newMouseState);
                //         MapTileLocation mapTileLocation = MapTileLocation.CreateFromWorldCoordinates(mouseWorldLocationPoint.X, mouseWorldLocationPoint.Y);
                //         gdiConstructionYard.CreateBarracksAtPosition(mapTileLocation);
                //         GameWorldView.instance.Notify_DonePlacingBarracks();
                //         return new PointerOverMapState();
                //     }
                //
                // }
            }
            return this;

        }

    }
}
