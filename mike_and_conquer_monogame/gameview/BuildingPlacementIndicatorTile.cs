
using MapTileLocation = mike_and_conquer_simulation.gameworld.MapTileLocation;

namespace mike_and_conquer_monogame.gameview
{
    public class BuildingPlacementIndicatorTile
    {

        private int relativeX;
        private int relativeY;

        private MapTileLocation mapTileLocation;
        private bool canPlaceBulding;

        public bool CanPlaceBuilding
        {
            get { return canPlaceBulding; }
            set { canPlaceBulding = value; }
        }

        public MapTileLocation MapTileLocation
        {
            get
            {
                return mapTileLocation;
            }
        }

        public BuildingPlacementIndicatorTile(MapTileLocation baseMapTileLocation, int x, int y)
        {
            this.relativeX = x;
            this.relativeY = y;
            this.UpdateLocation(baseMapTileLocation);
        }


        public void UpdateLocation(MapTileLocation newMapTileLocation)
        {
            this.mapTileLocation = newMapTileLocation
                .Clone()
                .IncrementWorldMapTileX(relativeX)
                .IncrementWorldMapTileY(relativeY);
        }


    }
}
