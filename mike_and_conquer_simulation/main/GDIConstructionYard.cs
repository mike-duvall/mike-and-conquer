


using MapTileLocation = mike_and_conquer_simulation.gameworld.MapTileLocation;
using Point = System.Drawing.Point;
using Rectangle = System.Drawing.Rectangle;

namespace mike_and_conquer_simulation.main
{
    public class GDIConstructionYard
    {
        private MapTileLocation mapTileLocation;
        public MapTileLocation MapTileLocation
        {
            get { return mapTileLocation; }
        }

        private bool isBuildingBarracks;

        private float buildBarracksPercentComplete;
        private bool isBarracksReadyToPlace;

        private float scaledBuildSpeed;
        private float baseBuildSpeed = 0.65f;

        public bool IsBuildingBarracks
        {
            get { return isBuildingBarracks; }
        }

        public int PercentBarracksBuildComplete
        {
            get { return (int)buildBarracksPercentComplete; }
        }


        public bool IsBarracksReadyToPlace
        {
            get { return isBarracksReadyToPlace; }
        }

        protected GDIConstructionYard()
        {
        }

        public GDIConstructionYard(MapTileLocation mapTileLocation)
        {
            this.mapTileLocation = mapTileLocation;
        }

        public bool ContainsPoint(Point aPoint)
        {
            int width = 72;
            int height = 48;

            int leftX = (int)mapTileLocation.WorldCoordinatesAsVector2.X - (width / 2);
            int topY = (int)mapTileLocation.WorldCoordinatesAsVector2.Y - (height / 2);

            Rectangle boundRectangle = new Rectangle(leftX, topY, width, height);
            return boundRectangle.Contains(aPoint);
        }


        public void StartBuildingBarracks()
        {
            isBuildingBarracks = true;
            buildBarracksPercentComplete = 0.0f;
        }

        // public void Update(GameTime gameTime)
        // {
        //     scaledBuildSpeed = baseBuildSpeed / GameOptions.instance.GameSpeedDelayDivisor;
        //
        //     if (isBuildingBarracks)
        //     {
        //         double buildIncrement = gameTime.ElapsedGameTime.TotalMilliseconds * scaledBuildSpeed;
        //
        //         buildBarracksPercentComplete += (float)buildIncrement;
        //         if (buildBarracksPercentComplete >= 100.0f)
        //         {
        //             isBarracksReadyToPlace = true;
        //             isBuildingBarracks = false;
        //         }
        //     }
        // }


        // public void CreateBarracksAtPosition(MapTileLocation mapTileLocation)
        // {
        //
        //     MikeAndConquerGame.instance.AddGDIBarracks(mapTileLocation);
        //     isBarracksReadyToPlace = false;
        //     isBuildingBarracks = false;
        // }


    }


}
