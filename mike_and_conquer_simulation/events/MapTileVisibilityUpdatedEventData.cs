using System;


namespace mike_and_conquer_simulation.events
{
    public class MapTileVisibilityUpdatedEventData
    {

        public const string EventType = "MapTileVisibilityUpdated";


        public int MapTileInstanceId { get;  }

        public string Visibility { get; }



        public MapTileVisibilityUpdatedEventData(int mapTileInstanceId, String visibility)
        {
            this.MapTileInstanceId = mapTileInstanceId;
            this.Visibility = visibility;
        }

    }
}
