
using System;
using mike_and_conquer_simulation.events;
using Newtonsoft.Json;

using MapTileInstance = mike_and_conquer_simulation.gameworld.MapTileInstance;

using NumericsVector2 = System.Numerics.Vector2;
using GameWorld = mike_and_conquer_simulation.gameworld.GameWorld;

using MapTileLocation = mike_and_conquer_simulation.gameworld.MapTileLocation;



namespace mike_and_conquer_simulation.main
{
    internal abstract class Unit
    {

        protected GameWorldLocation gameWorldLocation;

        protected MapTileInstance currentMapTileInstance;


        public GameWorldLocation GameWorldLocation
        {
            get { return gameWorldLocation; }
        }

        protected int health;

        public int UnitId { get; set; }

        public int Health
        {
            get { return health; }
        }

        public abstract void Update();

        public abstract void OrderMoveToDestination(int destinationXInWorldCoordinates,
            int destinationYInWorldCoordinates);

        public abstract void OrderToAttackEnemyUnit(Unit targetUnit);

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

        public void PublishAttackCommandBeganEvent(int attackerUnitId, int targetUnitId)
        {

            BeganMissionAttackEventData eventData = new BeganMissionAttackEventData(
                attackerUnitId,
                targetUnitId);


            string serializedEventData = JsonConvert.SerializeObject(eventData);
            SimulationStateUpdateEvent simulationStateUpdateEvent =
                new SimulationStateUpdateEvent(
                    BeganMissionAttackEventData.EventType,
                    serializedEventData);

            SimulationMain.instance.PublishEvent(simulationStateUpdateEvent);
        }

        public void PublishNoneCommandBeganEvent(int unitId)
        {

            NoneCommandBeganEventData eventData = new NoneCommandBeganEventData(unitId);


            string serializedEventData = JsonConvert.SerializeObject(eventData);
            SimulationStateUpdateEvent simulationStateUpdateEvent =
                new SimulationStateUpdateEvent(
                    NoneCommandBeganEventData.EventType,
                    serializedEventData);

            SimulationMain.instance.PublishEvent(simulationStateUpdateEvent);
        }


        public void PublishFiredOnUnitEvent(int attackerUnitId, int targetUnitId)
        {
            FireOnUnitEventData eventData = new FireOnUnitEventData(
                attackerUnitId,
                targetUnitId);

            string serializedEventData = JsonConvert.SerializeObject(eventData);
            SimulationStateUpdateEvent simulationStateUpdateEvent =
                new SimulationStateUpdateEvent(
                    FireOnUnitEventData.EventType,
                    serializedEventData);

            SimulationMain.instance.PublishEvent(simulationStateUpdateEvent);
        }

        public void PublishBulletHitTargetEvent(int attackerUnitId, int targetUnitId)
        {
            BulletHitTargetEventData eventData = new BulletHitTargetEventData(
                attackerUnitId,
                targetUnitId);

            string serializedEventData = JsonConvert.SerializeObject(eventData);
            SimulationStateUpdateEvent simulationStateUpdateEvent =
                new SimulationStateUpdateEvent(
                    BulletHitTargetEventData.EventType,
                    serializedEventData);

            SimulationMain.instance.PublishEvent(simulationStateUpdateEvent);
        }


        



        protected void UpdateNearbyMapTileVisibility(int xOffset, int yOffset, MapTileInstance.MapTileVisibility mapTileVisibility)
        {
            MapTileInstance mapTileInstance = FindNearbyMapTileByOffset(xOffset, yOffset);

            if (mapTileInstance != null && mapTileInstance.Visibility != MapTileInstance.MapTileVisibility.Visible)
            {
                mapTileInstance.Visibility = mapTileVisibility;
                PublishMapTileVisibilityUpdatedEvent(mapTileInstance.MapTileInstanceId, mapTileVisibility);
            }

        }


        protected MapTileInstance FindNearbyMapTileByOffset(int xOffset, int yOffset)
        {
            return FindNearbyMapTileByOffset(
                this.currentMapTileInstance.MapTileLocation.WorldCoordinatesAsVector2,
                xOffset,
                yOffset);
        }

        protected MapTileInstance FindNearbyMapTileByOffset(NumericsVector2 basePosition, int xOffset, int yOffset)
        {
            MapTileLocation offsetMapTileLocation =
                MapTileLocation.CreateFromWorldCoordinatesInVector2(basePosition)
                    .IncrementWorldMapTileX(xOffset)
                    .IncrementWorldMapTileY(yOffset);

            MapTileInstance mapTileInstance = GameWorld.instance.FindMapTileInstanceAllowNull(offsetMapTileLocation);
            return mapTileInstance;
        }



        private void PublishMapTileVisibilityUpdatedEvent(int mapTileInstanceId, MapTileInstance.MapTileVisibility mapTileVisibility)
        {

            MapTileVisibilityUpdatedEventData eventData = new MapTileVisibilityUpdatedEventData(
                mapTileInstanceId,
                mapTileVisibility.ToString()
            );

            string serializedEventData = JsonConvert.SerializeObject(eventData);
            SimulationStateUpdateEvent simulationStateUpdateEvent =
                new SimulationStateUpdateEvent(
                    MapTileVisibilityUpdatedEventData.EventType,
                    serializedEventData);

            SimulationMain.instance.PublishEvent(simulationStateUpdateEvent);
        }


        private void PublishUnitTookDamageEvent()
        {
            UnitTookDamageEventData eventData = new UnitTookDamageEventData(this.UnitId);

            string serializedEventData = JsonConvert.SerializeObject(eventData);
            SimulationStateUpdateEvent simulationStateUpdateEvent =
                new SimulationStateUpdateEvent(
                    UnitTookDamageEventData.EventType,
                    serializedEventData);

            SimulationMain.instance.PublishEvent(simulationStateUpdateEvent);
        }

        private void PublishUnitDestroyedEvent()
        {
            UnitDestroyedEventData eventData = new UnitDestroyedEventData(this.UnitId);

            string serializedEventData = JsonConvert.SerializeObject(eventData);
            SimulationStateUpdateEvent simulationStateUpdateEvent =
                new SimulationStateUpdateEvent(
                    UnitDestroyedEventData.EventType,
                    serializedEventData);

            SimulationMain.instance.PublishEvent(simulationStateUpdateEvent);
        }



        

        protected void PublishUnitReloadedWeaponEvent()
        {

            UnitReloadedWeaponEventData eventData = new UnitReloadedWeaponEventData(this.UnitId);

            string serializedEventData = JsonConvert.SerializeObject(eventData);
            SimulationStateUpdateEvent simulationStateUpdateEvent =
                new SimulationStateUpdateEvent(
                    UnitReloadedWeaponEventData.EventType,
                    serializedEventData);

            SimulationMain.instance.PublishEvent(simulationStateUpdateEvent);
        }




        public bool ApplyDamage(int amount)
        {
            health -= amount;
            PublishUnitTookDamageEvent();
            Boolean destroyed = health <= 0;
            if (destroyed)
            {
                PublishUnitDestroyedEvent();
            }

            return destroyed;
        }



    }
}
