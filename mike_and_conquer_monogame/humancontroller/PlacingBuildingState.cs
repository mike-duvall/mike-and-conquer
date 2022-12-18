


using MouseState = Microsoft.Xna.Framework.Input.MouseState;
using Point = Microsoft.Xna.Framework.Point;

using SimulationMapTileLocation = mike_and_conquer_simulation.gameworld.MapTileLocation;
using GameWorldView = mike_and_conquer_monogame.gameview.GameWorldView;
using SimulationMain = mike_and_conquer_simulation.main.SimulationMain;

using PlaceBarracksCommand =  mike_and_conquer_simulation.commands.PlaceBarracksCommand;

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
                if (MouseInputUtil.LeftMouseButtonUnclicked(newMouseState, oldMouseState))
                {
                    if(GameWorldView.instance.BarracksPlacementIndicatorView.ValidBuildingLocation())
                    {
                        // GDIConstructionYard gdiConstructionYard = GameWorld.instance.GDIConstructionYard;
                        Point mouseWorldLocationPoint = MouseInputUtil.GetWorldLocationPointFromMouseState(newMouseState);
                        SimulationMapTileLocation mapTileLocation = SimulationMapTileLocation.CreateFromWorldCoordinates(mouseWorldLocationPoint.X, mouseWorldLocationPoint.Y);
                        // gdiConstructionYard.CreateBarracksAtPosition(mapTileLocation);
                        // GameWorldView.instance.Notify_DonePlacingBarracks();
                        // return new PointerOverMapState();


                        SendPlaceBarracksCommand(mapTileLocation);

                        // TODO: Should this part wait on events that barracks was placed?
                        GameWorldView.instance.Notify_DonePlacingBarracks();
                        return new PointerOverMapState();

                    }

                }


            }
            return this;

        }


        public void SendPlaceBarracksCommand(SimulationMapTileLocation simulationMapTileLocation)
        {
            // OrderUnitToMoveCommand command = new OrderUnitToMoveCommand();
            // command.UnitId = unitId;
            // command.DestinationXInWorldCoordinates = centerOfSquare.X;
            // command.DestinationYInWorldCoordinates = centerOfSquare.Y;
            PlaceBarracksCommand command = new PlaceBarracksCommand();

            command.XInWorldCoordinates = simulationMapTileLocation.WorldCoordinatesAsPoint.X;
            command.YInWorldCoordinates = simulationMapTileLocation.WorldCoordinatesAsPoint.Y;
            SimulationMain.instance.PostCommand(command);

        }



    }
}
