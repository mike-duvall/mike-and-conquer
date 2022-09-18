
using mike_and_conquer_monogame.main;

using mike_and_conquer_simulation.events;

namespace mike_and_conquer_monogame.commands
{
    public class UpdateMapTileViewVisibilityCommand : AsyncViewCommand
    {


        // private UnitPositionChangedEventData unitPositionChangedEventData;
        private MapTileVisibilityUpdatedEventData eventData;

        public UpdateMapTileViewVisibilityCommand(MapTileVisibilityUpdatedEventData data)
        {
            this.eventData = data;
        }

        protected override void ProcessImpl()
        {
            // MikeAndConquerGame.instance.UpdateUnitViewPosition(unitPositionChangedEventData);
            MikeAndConquerGame.instance.UpdateMapTileViewVisibility(eventData);
        }
    }
}
