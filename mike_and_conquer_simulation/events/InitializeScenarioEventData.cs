using System.Collections.Generic;

namespace mike_and_conquer_simulation.events
{
    public class InitializeScenarioEventData
    {

        public const string EventType = "InitializeScenario";

        public int MapWidth { get;  }
        public int MapHeight { get;  }

        public List<MapTileInstanceCreateEventData> MapTileInstanceCreateEventDataList { get; }
        public List<TerrainItemCreateEventData> TerrainItemCreateEventDataList { get; }

        public InitializeScenarioEventData(int mapWidth, int mapHeight, List<MapTileInstanceCreateEventData> mapTileInstanceCreateEventDataList, List<TerrainItemCreateEventData> terrainItemCreateEventDataList)
        {
            MapWidth = mapWidth;
            MapHeight = mapHeight;
            MapTileInstanceCreateEventDataList = mapTileInstanceCreateEventDataList;
            TerrainItemCreateEventDataList = terrainItemCreateEventDataList;
        }




    }
}
