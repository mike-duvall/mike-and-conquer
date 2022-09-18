using System;
using mike_and_conquer_simulation.gameworld;

namespace mike_and_conquer_simulation.events
{
    public class MapTileVisibilityUpdatedEventData
    {

        public const string EventType = "UnitOrderedToMove";


        public int MapTileInstanceId { get; set; }

        public string Visibility { get; }



        public MapTileVisibilityUpdatedEventData(int mapTileInstanceId, String visibility)
        {
            this.MapTileInstanceId = mapTileInstanceId;
            this.Visibility = visibility;
        }

    }
}
