using mike_and_conquer_monogame.commands;

using mike_and_conquer_monogame.main;
using mike_and_conquer_simulation.events;
using Newtonsoft.Json;



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

            if (anEvent.EventType.Equals(MapTileVisibilityUpdatedEventData.EventType))
            {
                MapTileVisibilityUpdatedEventData eventData =
                    JsonConvert.DeserializeObject<MapTileVisibilityUpdatedEventData>(anEvent.EventData);

                UpdateMapTileViewVisibilityCommand command = new UpdateMapTileViewVisibilityCommand(eventData.MapTileInstanceId, eventData.Visibility);
                mikeAndConquerGame.PostCommand(command);
            }




        }
    }
}
