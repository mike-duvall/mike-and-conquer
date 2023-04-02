using System;
using mike_and_conquer_monogame.main;
using Serilog;
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

        private static readonly ILogger Logger = Log.ForContext<PlacingBuildingState>();

        public override HumanControllerState Update(MouseState newMouseState, MouseState oldMouseState)
        {


            GameWorldView.instance.Notify_PlacingBarracks();

            if (!MouseInputUtil.IsOverSidebar(newMouseState))
            {

                if (!newMouseState.Equals(oldMouseState))
                {
                    GameWorldView.instance.Notify_PlacingBarracksWithMouseOverMap(newMouseState.Position);
                }


                if (MouseInputUtil.LeftMouseButtonUnclicked(newMouseState, oldMouseState))
                {
                    if(GameWorldView.instance.BarracksPlacementIndicatorView.ValidBuildingLocation())
                    {
                        Logger.Information("Sending Place Barracks Command");
                        
                        Point mouseWorldLocationPoint = MouseInputUtil.GetWorldLocationPointFromMouseState(newMouseState);
                        SimulationMapTileLocation mapTileLocation = SimulationMapTileLocation.CreateFromWorldCoordinates(mouseWorldLocationPoint.X, mouseWorldLocationPoint.Y);
                        SendPlaceBarracksCommand(mapTileLocation);

                        // TODO: Should this part wait on events that barracks was placed?
                        GameWorldView.instance.Notify_DonePlacingBarracks();
                        return new PointerOverMapState();
                    }
                    else
                    {
                        Logger.Warning(
                             "Did not send Place Barracks Command because ValidBuildingLocation() returned false");
                    }

                }


            }
            return this;

        }


        public void SendPlaceBarracksCommand(SimulationMapTileLocation simulationMapTileLocation)
        {
            PlaceBarracksCommand command = new PlaceBarracksCommand();

            command.XInWorldCoordinates = simulationMapTileLocation.WorldCoordinatesAsPoint.X;
            command.YInWorldCoordinates = simulationMapTileLocation.WorldCoordinatesAsPoint.Y;
            SimulationMain.instance.PostCommand(command);

        }



    }
}
