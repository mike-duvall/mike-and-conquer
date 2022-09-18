using mike_and_conquer_monogame.commands;

using mike_and_conquer_monogame.main;
using mike_and_conquer_simulation.events;
using Newtonsoft.Json;
using SharpDX.Direct3D9;


namespace mike_and_conquer_monogame.eventhandler
{
    public class UpdateMapTileViewVisibilityWhenMapTileVisibilityChangedEventHandler : SimulationStateListener
    {
        private MikeAndConquerGame mikeAndConquerGame = null;


        public UpdateMapTileViewVisibilityWhenMapTileVisibilityChangedEventHandler(MikeAndConquerGame aGame)
        {
            this.mikeAndConquerGame = aGame;
        }

        public override void Update(SimulationStateUpdateEvent anEvent)
        {
            // if (anEvent.EventType.Equals(UnitPositionChangedEventData.EventType))
            // {
            //     UnitPositionChangedEventData unitPositionChangedEventData =
            //         JsonConvert.DeserializeObject<UnitPositionChangedEventData>(anEvent.EventData);
            //     x
            //     UpdateUnitViewPositionCommand command = new UpdateUnitViewPositionCommand(unitPositionChangedEventData);
            //     mikeAndConquerGame.PostCommand(command);
            //
            // }

            if (anEvent.EventType.Equals(MapTileVisibilityUpdatedEventData.EventType))
            {
                // UnitPositionChangedEventData unitPositionChangedEventData =
                //     JsonConvert.DeserializeObject<UnitPositionChangedEventData>(anEvent.EventData);

                MapTileVisibilityUpdatedEventData eventData =
                    JsonConvert.DeserializeObject<MapTileVisibilityUpdatedEventData>(anEvent.EventData);

                UpdateMapTileViewVisibilityCommand command = new UpdateMapTileViewVisibilityCommand(eventData);
                mikeAndConquerGame.PostCommand(command);

            }




        }
    }
}
