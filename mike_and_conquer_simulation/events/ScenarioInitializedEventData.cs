using System.Collections.Generic;

namespace mike_and_conquer_simulation.events
{
    public class ScenarioInitializedEventData
    {

        public const string EventType = "ScenarioInitialized";

        public int MapWidth { get;  }
        public int MapHeight { get;  }

        public List<MapTileInstanceCreateEventData> MapTileInstanceCreateEventDataList { get; }
        public List<TerrainItemCreateEventData> TerrainItemCreateEventDataList { get; }

        public ScenarioInitializedEventData(int mapWidth, int mapHeight, List<MapTileInstanceCreateEventData> mapTileInstanceCreateEventDataList, List<TerrainItemCreateEventData> terrainItemCreateEventDataList)
        {
            MapWidth = mapWidth;
            MapHeight = mapHeight;
            MapTileInstanceCreateEventDataList = mapTileInstanceCreateEventDataList;
            TerrainItemCreateEventDataList = terrainItemCreateEventDataList;
        }




    }
}
