
// using Microsoft.Xna.Framework;


using mike_and_conquer_simulation.util;
using Point = System.Drawing.Point;
using Vector2 = System.Numerics.Vector2;

namespace mike_and_conquer_simulation.gameworld
{
    internal class MapTileLocation
    {
        private int xInWorldMapTileCoordinates;
        private int yInWorldMapTileCoordinates;


        public int XInWorldMapTileCoordinates
        {
            get { return xInWorldMapTileCoordinates; }
        }



        public int YInWorldMapTileCoordinates
        {
            get { return yInWorldMapTileCoordinates; }
        }




        private MapTileLocation(int x, int y)
        {
            this.xInWorldMapTileCoordinates = x;
            this.yInWorldMapTileCoordinates = y;
        }

        public static MapTileLocation CreateFromWorldMapTileCoordinates(int x, int y)
        {
            return new MapTileLocation(x,y);
        }

        public static MapTileLocation CreateFromWorldCoordinatesInVector2(Vector2 worldCoordinatesInVector2)
        {
            System.Drawing.Point worldCoordinatesInPoint = PointUtil.ConvertVector2ToPoint(worldCoordinatesInVector2);
            Point mapTileCoordinates = ConvertWorldCoordinatesToMapTileCoordinates(worldCoordinatesInPoint);
            return new MapTileLocation(mapTileCoordinates.X, mapTileCoordinates.Y);
        }

        public static MapTileLocation CreateFromWorldCoordinates(int x, int y)
        {
            Point worldCoordinatesInPoint = new Point(x,y);
            Point mapTileCoordinates = ConvertWorldCoordinatesToMapTileCoordinates(worldCoordinatesInPoint);
            return new MapTileLocation(mapTileCoordinates.X, mapTileCoordinates.Y);
        }

        public Point WorldMapTileCoordinatesAsPoint
        {
            get { return new Point(xInWorldMapTileCoordinates, yInWorldMapTileCoordinates); }
        }



        public Point WorldCoordinatesAsPoint
        {
            get
            {
                return MapTileLocation.ConvertMapTileCoordinatesToWorldCoordinates(new Point(xInWorldMapTileCoordinates, yInWorldMapTileCoordinates));
            }
        }


        public Vector2 WorldCoordinatesAsVector2
        {
            get
            {
                Point point = WorldCoordinatesAsPoint;
                return new Vector2(point.X, point.Y);
            }
        }


        public static Point ConvertMapTileCoordinatesToWorldCoordinates(Point pointInWorldMapSquareCoordinates)
        {

            int xInWorldCoordinates = (pointInWorldMapSquareCoordinates.X * GameWorld.MAP_TILE_WIDTH) +
                                      (GameWorld.MAP_TILE_WIDTH / 2);
            int yInWorldCoordinates = pointInWorldMapSquareCoordinates.Y * GameWorld.MAP_TILE_HEIGHT +
                                      (GameWorld.MAP_TILE_HEIGHT / 2);

            return new Point(xInWorldCoordinates, yInWorldCoordinates);
        }


        private static Point ConvertWorldCoordinatesToMapTileCoordinates(Point pointInWorldCoordinates)
        {

            int destinationRow = pointInWorldCoordinates.Y;
            int destinationColumn = pointInWorldCoordinates.X;

            int destinationX = destinationColumn / GameWorld.MAP_TILE_WIDTH;
            int destinationY = destinationRow / GameWorld.MAP_TILE_HEIGHT;

            return new Point(destinationX, destinationY);
        }

        public MapTileLocation Clone()
        {
            return new MapTileLocation(xInWorldMapTileCoordinates, yInWorldMapTileCoordinates);
        }

        public MapTileLocation IncrementWorldMapTileX(int relativeX)
        {
            this.xInWorldMapTileCoordinates += relativeX;
            return this;
        }

        public MapTileLocation IncrementWorldMapTileY(int relativeY)
        {
            this.yInWorldMapTileCoordinates += relativeY;
            return this;
        }


        public MapTileLocation XInWorldCoordinates(int newX)
        {
            xInWorldMapTileCoordinates = newX / GameWorld.MAP_TILE_WIDTH;
            return this;
        }

        public MapTileLocation YInWorldCoordinates(int newY)
        {
            yInWorldMapTileCoordinates = newY / GameWorld.MAP_TILE_HEIGHT;
            return this;
        }



        public MapTileLocation CreateAdjacentMapTileLocation(GameWorld.TILE_LOCATION tileLocation)
        {
            MapTileLocation adjacentMapTileLocation = this.Clone();

            int xOffset = 0;
            int yOffset = 0;

            if (tileLocation == GameWorld.TILE_LOCATION.WEST)
            {
                xOffset = -1;
            }
            else if (tileLocation == GameWorld.TILE_LOCATION.NORTH_WEST)
            {
                xOffset = -1;
                yOffset = -1;
            }
            else if (tileLocation == GameWorld.TILE_LOCATION.NORTH)
            {
                yOffset = -1;
            }
            else if (tileLocation == GameWorld.TILE_LOCATION.NORTH_EAST)
            {
                xOffset = 1;
                yOffset = -1;
            }
            else if (tileLocation == GameWorld.TILE_LOCATION.EAST)
            {
                xOffset = 1;

            }
            else if (tileLocation == GameWorld.TILE_LOCATION.SOUTH_EAST)
            {
                xOffset = 1;
                yOffset = 1;
            }
            else if (tileLocation == GameWorld.TILE_LOCATION.SOUTH)
            {
                yOffset = 1;
            }
            else if (tileLocation == GameWorld.TILE_LOCATION.SOUTH_WEST)
            {
                xOffset = -1;
                yOffset = 1;
            }

            adjacentMapTileLocation
                .IncrementWorldMapTileX(xOffset)
                .IncrementWorldMapTileY(yOffset);

            return adjacentMapTileLocation;


        }

        //        public int XInWorldCoordinates()
        //        {
        //            int xInWorldCoordinates = (xInWorldMapTileCoordinates * GameWorld.MAP_TILE_WIDTH) +
        //                                      (GameWorld.MAP_TILE_WIDTH / 2);
        //
        //            return xInWorldCoordinates;
        //        }


    }
}
