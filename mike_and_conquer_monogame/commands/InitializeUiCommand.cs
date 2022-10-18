
using System.Collections.Generic;
using mike_and_conquer_monogame.main;

using mike_and_conquer_simulation.events;

namespace mike_and_conquer_monogame.commands
{
    public class InitiInitalizeUiCommand : AsyncViewCommand
    {

        private readonly int mapWidth;
        private readonly int mapHeight;
        private readonly List<MapTileInstanceCreateEventData> mapTileInstanceCreateEventDataList;
        private readonly List<TerrainItemCreateEventData> terrainItemCreateEventDataList;

        public InitiInitalizeUiCommand(
            int mapWidth,
            int mapHeight,
            List<MapTileInstanceCreateEventData> mapTileInstanceCreateEventDataList,
            List<TerrainItemCreateEventData> terrainItemCreateEventDataList)


        {
            this.mapWidth = mapWidth;
            this.mapHeight = mapHeight;
            this.mapTileInstanceCreateEventDataList = mapTileInstanceCreateEventDataList;
            this.terrainItemCreateEventDataList = terrainItemCreateEventDataList;
        }

        protected override void ProcessImpl()
        {

            MikeAndConquerGame.instance.InitializeUI(
                mapWidth,
                mapHeight,
                mapTileInstanceCreateEventDataList,
                terrainItemCreateEventDataList);

        }
    }
}
