
using mike_and_conquer_simulation.gameworld;

namespace mike_and_conquer_simulation.main
{
    public class GameWorldLocationBuilder
    {

        private float worldCoordinatesX;
        private float worldCoordinatesY;

        private static int halfMapSquareWidth = GameWorld.MAP_TILE_WIDTH / 2;


        public float WorldCoordinatesX()
        {
            return worldCoordinatesX;
        }

        public float WorldCoordinatesY()
        {
            return worldCoordinatesY;
        }


        public GameWorldLocationBuilder WorldMapTileCoordinatesX(int x)
        {
            this.worldCoordinatesX = (x * GameWorld.MAP_TILE_WIDTH) + halfMapSquareWidth;
            return this;
        }

        public GameWorldLocationBuilder WorldMapTileCoordinatesY(int y)
        {
            this.worldCoordinatesY = (y * GameWorld.MAP_TILE_WIDTH) + halfMapSquareWidth;
            return this;
        }
        
        public GameWorldLocationBuilder WorldCoordinatesX(int x)
        {
            this.worldCoordinatesX = x;
            return this;
        }
        
        public GameWorldLocationBuilder WorldCoordinatesY(int y)
        {
            this.worldCoordinatesY = y;
            return this;
        }


        public GameWorldLocation build()
        {
            GameWorldLocation location = new GameWorldLocation(this);
            return location;
        }

    }
}
