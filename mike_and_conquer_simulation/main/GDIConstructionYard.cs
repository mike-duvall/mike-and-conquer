

using mike_and_conquer_simulation.gameworld;
using MapTileLocation = mike_and_conquer_simulation.gameworld.MapTileLocation;
using Point = System.Drawing.Point;
using Rectangle = System.Drawing.Rectangle;
using SimulationStateUpdateEvent = mike_and_conquer_simulation.events.SimulationStateUpdateEvent;
using StartedBuildingBarracksEventData = mike_and_conquer_simulation.events.StartedBuildingBarracksEventData;


using BuildingBarracksPercentCompletedEventData = mike_and_conquer_simulation.events.BuildingBarracksPercentCompletedEventData;
using CompletedBuildingBarracksEventData = mike_and_conquer_simulation.events.CompletedBuildingBarracksEventData;

using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace mike_and_conquer_simulation.main
{
    internal class GDIConstructionYard
    {
        private MapTileLocation mapTileLocation;
        public MapTileLocation MapTileLocation
        {
            get { return mapTileLocation; }
        }

        private bool isBuildingBarracks;

        private float buildBarracksPercentComplete;
        private bool isBarracksReadyToPlace;

        // private float scaledBuildSpeed;
        // private float baseBuildSpeed = 0.65f;

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

        private void PublishCompletedBuildingBarracksEvent()
        {
            SimulationStateUpdateEvent simulationStateUpdateEvent =
                new SimulationStateUpdateEvent(
                    CompletedBuildingBarracksEventData.EventType,
                    null);


            SimulationMain.instance.PublishEvent(simulationStateUpdateEvent);
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

        public void Update()
        {
            // scaledBuildSpeed = baseBuildSpeed / GameOptions.instance.GameSpeedDelayDivisor;
        
            if (isBuildingBarracks)
            {
                // double buildIncrement = gameTime.ElapsedGameTime.TotalMilliseconds * scaledBuildSpeed;
                double buildIncrement = 0.4;

                buildBarracksPercentComplete += (float)buildIncrement;


                if (buildBarracksPercentComplete >= 100.0f)
                {
                    isBarracksReadyToPlace = true;
                    isBuildingBarracks = false;
                    PublishCompletedBuildingBarracksEvent();
                }
                else
                {
                    PublishBuildingBarracksPercentCompleteEvent();
                }
            }
        }


        private void PublishBuildingBarracksPercentCompleteEvent()
        {

            BuildingBarracksPercentCompletedEventData eventData =
                new BuildingBarracksPercentCompletedEventData(this.PercentBarracksBuildComplete);

            string serializedEventData = JsonConvert.SerializeObject(eventData);

            SimulationStateUpdateEvent simulationStateUpdateEvent =
                new SimulationStateUpdateEvent(
                    BuildingBarracksPercentCompletedEventData.EventType,
                    serializedEventData);

            SimulationMain.instance.PublishEvent(simulationStateUpdateEvent);

        }




        public GDIBarracks CreateGDIBarracksAtPosition(MapTileLocation mapTileLocation)
        {
            isBarracksReadyToPlace = false;
            isBuildingBarracks = false;

            // return SimulationMain.instance.AddGDIBarracks(mapTileLocation);
            return GameWorld.instance.AddGDIBarracks(mapTileLocation);
        }


    }


}
