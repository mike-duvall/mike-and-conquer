
using System;
// using mike_and_conquer.gameworld;
// using mike_and_conquer.main;
// using Vector2 = Microsoft.Xna.Framework.Vector2;
// using GameTime = Microsoft.Xna.Framework.GameTime;
// using Point = Microsoft.Xna.Framework.Point;

using MapTileLocation = mike_and_conquer_simulation.gameworld.MapTileLocation;

namespace mike_and_conquer_simulation.main
{ 

    public class GDIBarracks
    {


        private MapTileLocation mapTileLocation;

        public MapTileLocation MapTileLocation
        {
            get { return mapTileLocation; }
        }

        private Boolean isBuildingMinigunner;

        private float buildMinigunnerPercentComplete;

        private float scaledBuildSpeed;
        private float baseBuildSpeed = 1.25f;


        public Boolean IsBuildingMinigunner
        {
            get { return isBuildingMinigunner; }
        }
        
        public int PercentMinigunnerBuildComplete
        {
            get { return (int) buildMinigunnerPercentComplete; }
        }


        protected GDIBarracks()
        {
        }

        public GDIBarracks(MapTileLocation mapTileLocation)
        {
            this.mapTileLocation = mapTileLocation;
        }



//        public bool ContainsPoint(Point aPoint)
//        {
//            int width = GameWorld.MAP_TILE_WIDTH;
//            int height = GameWorld.MAP_TILE_HEIGHT;
//
//            int leftX = (int)positionInWorldCoordinates.X - (width / 2);
//            int topY = (int)positionInWorldCoordinates.Y - (height / 2);
//            Rectangle boundRectangle = new Rectangle(leftX, topY, width, height);
//            return boundRectangle.Contains(aPoint);
//        }


        public void StartBuildingMinigunner()
        {
            isBuildingMinigunner = true;
            buildMinigunnerPercentComplete = 0.0f;
        }



        // public void Update()
        // {
        //     scaledBuildSpeed = baseBuildSpeed / GameOptions.instance.GameSpeedDelayDivisor;
        //
        //     if (isBuildingMinigunner)
        //     {
        //         double buildIncrement = gameTime.ElapsedGameTime.TotalMilliseconds * scaledBuildSpeed;
        //
        //         buildMinigunnerPercentComplete += (float)buildIncrement;
        //         if (buildMinigunnerPercentComplete >= 100.0f)
        //         {
        //             CreateMinigunnerFromBarracks();
        //             isBuildingMinigunner = false;
        //         }
        //
        //     }
        // }

        // private void CreateMinigunnerFromBarracks()
        // {
        //     Point gdiMinigunnderPosition = mapTileLocation.WorldCoordinatesAsPoint;
        //     Minigunner builtMinigunner = MikeAndConquerGame.instance.AddGdiMinigunner(gdiMinigunnderPosition);
        //
        //     Point destinationInWC = new Point(gdiMinigunnderPosition.X, gdiMinigunnderPosition.Y + 40);
        //     builtMinigunner.OrderToMoveToDestination(destinationInWC);
        //
        // }
    }


}
