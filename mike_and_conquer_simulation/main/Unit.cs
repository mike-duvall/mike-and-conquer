
using System;
using mike_and_conquer_simulation.events;
using Newtonsoft.Json;

namespace mike_and_conquer_simulation.main
{
    internal abstract class Unit
    {

        protected GameWorldLocation gameWorldLocation;

        public GameWorldLocation GameWorldLocation
        {
            get { return gameWorldLocation; }
        }

        public int UnitId { get; set; }

        public abstract void Update();

        public abstract void OrderMoveToDestination(int destinationXInWorldCoordinates,
            int destinationYInWorldCoordinates);

        protected void PublishUnitArrivedAtDestinationEvent()
        {
            UnitArrivedAtDestinationEventData eventData = 
                new UnitArrivedAtDestinationEventData(
                    this.UnitId,
                    (int)Math.Round(this.gameWorldLocation.X, 0),
                    (int)Math.Round(this.gameWorldLocation.Y, 0));

            string serializedEventData = JsonConvert.SerializeObject(eventData);
            SimulationStateUpdateEvent simulationStateUpdateEvent =
                new SimulationStateUpdateEvent(
                    UnitArrivedAtDestinationEventData.EventType,
                    serializedEventData);


            SimulationMain.instance.PublishEvent(simulationStateUpdateEvent);
        }


        protected void PublishUnitPositionChangedEventData()
        {
            UnitPositionChangedEventData eventData = 
                new UnitPositionChangedEventData(
                    this.UnitId,
                    (int)Math.Round(this.gameWorldLocation.X, 0),
                    (int)Math.Round(this.gameWorldLocation.Y, 0)
                    );

            string serializedEventData = JsonConvert.SerializeObject(eventData);

            SimulationStateUpdateEvent simulationStateUpdateEvent = 
                new SimulationStateUpdateEvent(
                    UnitPositionChangedEventData.EventType,
                    serializedEventData);

            SimulationMain.instance.PublishEvent(simulationStateUpdateEvent);
        }


        public void PublishUnitMoveOrderEvent(int unitId, int destinationXInWorldCoordinates, int destinationYInWorldCoordinates)
        {
            UnitMoveOrderEventData eventData = new UnitMoveOrderEventData(
                unitId,
                destinationXInWorldCoordinates,
                destinationYInWorldCoordinates);


            string serializedEventData = JsonConvert.SerializeObject(eventData);
            SimulationStateUpdateEvent simulationStateUpdateEvent =
                new SimulationStateUpdateEvent(
                    UnitMoveOrderEventData.EventType,
                    serializedEventData);



            SimulationMain.instance.PublishEvent(simulationStateUpdateEvent);

        }


    }
}
