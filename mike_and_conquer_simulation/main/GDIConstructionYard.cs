


using MapTileLocation = mike_and_conquer_simulation.gameworld.MapTileLocation;
using Point = System.Drawing.Point;
using Rectangle = System.Drawing.Rectangle;
using SimulationStateUpdateEvent = mike_and_conquer_simulation.events.SimulationStateUpdateEvent;
using StartedBuildingBarracksEventData = mike_and_conquer_simulation.events.StartedBuildingBarracksEventData;

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
            if (!isBuildingBarracks)
            {
                isBuildingBarracks = true;
                buildBarracksPercentComplete = 0.0f;
                PublishStartBuildingBarracksEvent();
            }
        }

        private void PublishStartBuildingBarracksEvent()
        {
            SimulationStateUpdateEvent simulationStateUpdateEvent =
                new SimulationStateUpdateEvent(
                    StartedBuildingBarracksEventData.EventType,
                    null);


            SimulationMain.instance.PublishEvent(simulationStateUpdateEvent);


        }

        // private void PublishUnitArrivedAtPathStep(Point pathStepPoint)
        // {
        //
        //     PathStep pathStep = new PathStep(pathStepPoint.X, pathStepPoint.Y);
        //
        //
        //     UnitArrivedAtPathStepEventData eventData =
        //         new UnitArrivedAtPathStepEventData(this.UnitId, pathStep);
        //
        //     string serializedEventData = JsonConvert.SerializeObject(eventData);
        //
        //
        //     SimulationStateUpdateEvent simulationStateUpdateEvent =
        //         new SimulationStateUpdateEvent(
        //             UnitArrivedAtPathStepEventData.EventType,
        //             serializedEventData);
        //
        //     SimulationMain.instance.PublishEvent(simulationStateUpdateEvent);
        // }


        // private void PublishUnitArrivedAtPathStep(Point pathStepPoint)
        // {
        //
        //     PathStep pathStep = new PathStep(pathStepPoint.X, pathStepPoint.Y);
        //
        //
        //     UnitArrivedAtPathStepEventData eventData =
        //         new UnitArrivedAtPathStepEventData(this.UnitId, pathStep);
        //
        //     string serializedEventData = JsonConvert.SerializeObject(eventData);
        //
        //
        //     SimulationStateUpdateEvent simulationStateUpdateEvent =
        //         new SimulationStateUpdateEvent(
        //             UnitArrivedAtPathStepEventData.EventType,
        //             serializedEventData);
        //
        //     SimulationMain.instance.PublishEvent(simulationStateUpdateEvent);
        // }


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

        public void Update()
        {
            // scaledBuildSpeed = baseBuildSpeed / GameOptions.instance.GameSpeedDelayDivisor;
        
            if (isBuildingBarracks)
            {
                // double buildIncrement = gameTime.ElapsedGameTime.TotalMilliseconds * scaledBuildSpeed;
                double buildIncrement = .08;

                buildBarracksPercentComplete += (float)buildIncrement;
                if (buildBarracksPercentComplete >= 100.0f)
                {
                    isBarracksReadyToPlace = true;
                    isBuildingBarracks = false;
                }
            }
        }



        // public void CreateBarracksAtPosition(MapTileLocation mapTileLocation)
        // {
        //
        //     MikeAndConquerGame.instance.AddGDIBarracks(mapTileLocation);
        //     isBarracksReadyToPlace = false;
        //     isBuildingBarracks = false;
        // }


    }


}
