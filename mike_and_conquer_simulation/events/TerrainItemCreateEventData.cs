namespace mike_and_conquer_simulation.events
{
    public class TerrainItemCreateEventData
    {


        public int XInWorldMapTileCoordinates { get; }
        
        public int YInWorldMapTileCoordinates { get; }


        public string TerrainItemType { get; }



        public TerrainItemCreateEventData(int xInWorldMapTileCoordinates, int yInWorldMapTileCoordinates, string terrainItemType)
        {
            this.XInWorldMapTileCoordinates = xInWorldMapTileCoordinates;
            this.YInWorldMapTileCoordinates = yInWorldMapTileCoordinates;
            this.TerrainItemType = terrainItemType;
        }


    }
}



