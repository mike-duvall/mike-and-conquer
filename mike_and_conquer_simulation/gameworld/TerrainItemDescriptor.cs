using System;
using System.Collections.Generic;
// using Point = Microsoft.Xna.Framework.Point;
using Point = System.Drawing.Point;
// using Vector2 = System.Numerics.Vector2;

namespace mike_and_conquer_simulation.gameworld
{ 

    internal class TerrainItemDescriptor
    {

        private String terrainItemType;
        private int width;
        private int height;


        public String TerrainItemType
        {
            get { return terrainItemType; }
        }

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }

        // TODO:  Consider making this in array instead of list of Points
        private List<Point> blockedMapTileRelativeCoordinates;

        protected TerrainItemDescriptor()
        {
        }


        public TerrainItemDescriptor(String terrainItemType, int width, int height, List<Point> blockedMapTileRelativeCoordinates)
        {
            this.terrainItemType = terrainItemType;
            this.width = width;
            this.height = height;
            this.blockedMapTileRelativeCoordinates = blockedMapTileRelativeCoordinates;
        }


        public List<Point> GetBlockMapTileRelativeCoordinates()
        {

            return this.blockedMapTileRelativeCoordinates;
        }
    }


}
