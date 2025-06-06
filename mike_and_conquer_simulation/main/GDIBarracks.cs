

using MapTileLocation = mike_and_conquer_simulation.gameworld.MapTileLocation;
using SimulationStateUpdateEvent = mike_and_conquer_simulation.events.SimulationStateUpdateEvent;
using StartedBuildingMinigunnerEventData = mike_and_conquer_simulation.events.StartedBuildingMinigunnerEventData;
using BuildingMinigunnerPercentCompletedEventData = mike_and_conquer_simulation.events.BuildingMinigunnerPercentCompletedEventData;

using CompletedBuildingMinigunnerEventData = mike_and_conquer_simulation.events.CompletedBuildingMinigunnerEventData;

using JsonConvert = Newtonsoft.Json.JsonConvert;

using Point = System.Drawing.Point;

namespace mike_and_conquer_simulation.main
{ 

    internal class GDIBarracks
    {


        private MapTileLocation mapTileLocation;

        internal MapTileLocation MapTileLocation
        {
            get { return mapTileLocation; }
        }

        private bool isBuildingMinigunner;

        private float buildMinigunnerPercentComplete;

        private float scaledBuildSpeed;
        private float baseBuildSpeed = 1.25f;


        public bool IsBuildingMinigunner
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

            if (!isBuildingMinigunner)
            {
                isBuildingMinigunner = true;
                buildMinigunnerPercentComplete = 0.0f;
                PublishStartBuildingMinigunnerEvent();
            }

        }

        private void PublishStartBuildingMinigunnerEvent()
        {
            SimulationStateUpdateEvent simulationStateUpdateEvent =
                new SimulationStateUpdateEvent(
                    StartedBuildingMinigunnerEventData.EventType,
                    null);


            SimulationMain.instance.PublishEvent(simulationStateUpdateEvent);
        }




        public void Update()
        {
            // scaledBuildSpeed = baseBuildSpeed / GameOptions.instance.GameSpeedDelayDivisor;

            if (isBuildingMinigunner)
            {
                // double buildIncrement = gameTime.ElapsedGameTime.TotalMilliseconds * scaledBuildSpeed;
                double buildIncrement = 0.4;

                buildMinigunnerPercentComplete += (float)buildIncrement;


                if (buildMinigunnerPercentComplete >= 100.0f)
                {
                    isBuildingMinigunner = false;
                    PublishCompletedBuildingMinigunnerEvent();
                    CreateMinigunnerFromBarracks();
                }
                else
                {
                    PublishBuildingMinigunnerPercentCompleteEvent();
                }
            }
        }


        private void CreateMinigunnerFromBarracks()
        {
            Point gdiMinigunnderPosition = mapTileLocation.WorldCoordinatesAsPoint;
            Minigunner builtMinigunner = SimulationMain.instance.CreateGDIMinigunner(
                gdiMinigunnderPosition.X,
                gdiMinigunnderPosition.Y);

            Point destinationInWC = new Point(gdiMinigunnderPosition.X, gdiMinigunnderPosition.Y + 40);
            builtMinigunner.OrderMoveToDestination(
                destinationInWC.X,
                destinationInWC.Y);

        }


        private void PublishCompletedBuildingMinigunnerEvent()
        {
            SimulationStateUpdateEvent simulationStateUpdateEvent =
                new SimulationStateUpdateEvent(
                    CompletedBuildingMinigunnerEventData.EventType,
                    null);


            SimulationMain.instance.PublishEvent(simulationStateUpdateEvent);
        }



        private void PublishBuildingMinigunnerPercentCompleteEvent()
        {

            BuildingMinigunnerPercentCompletedEventData eventData =
                new BuildingMinigunnerPercentCompletedEventData(this.PercentMinigunnerBuildComplete);

            string serializedEventData = JsonConvert.SerializeObject(eventData);

            SimulationStateUpdateEvent simulationStateUpdateEvent =
                new SimulationStateUpdateEvent(
                    BuildingMinigunnerPercentCompletedEventData.EventType,
                    serializedEventData);

            SimulationMain.instance.PublishEvent(simulationStateUpdateEvent);

        }

    }


}
