﻿using mike_and_conquer_simulation.events;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using GameWorld = mike_and_conquer_simulation.gameworld.GameWorld;
using MapTileInstance = mike_and_conquer_simulation.gameworld.MapTileInstance;
using MapTileLocation = mike_and_conquer_simulation.gameworld.MapTileLocation;
using NumericsVector2 = System.Numerics.Vector2;



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

        public int UnitId { get; set; }


        protected int health;

        public int Health
        {
            get { return health; }
        }

        protected int maxHealth;

        public int MaxHealth
        {
            get { return maxHealth; }
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


        public void PublishBeganMissionMoveToDestinationEvent(int unitId, int destinationXInWorldCoordinates, int destinationYInWorldCoordinates)
        {
            BeganMissionMoveToDestinationEventData eventData = new BeganMissionMoveToDestinationEventData(
                unitId,
                destinationXInWorldCoordinates,
                destinationYInWorldCoordinates);


            string serializedEventData = JsonConvert.SerializeObject(eventData);
            SimulationStateUpdateEvent simulationStateUpdateEvent =
                new SimulationStateUpdateEvent(
                    BeganMissionMoveToDestinationEventData.EventType,
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


        private void PublishUnitTookDamageEvent(int amountOfDamage, int newHealthAmount)
        {
            UnitTookDamageEventData eventData = new UnitTookDamageEventData(this.UnitId, amountOfDamage, newHealthAmount);

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






        public bool ApplyDamage(int amountOfDamage)
        {
            health -= amountOfDamage;
            if (health < 0)
            {
                health = 0;
            }
            PublishUnitTookDamageEvent(amountOfDamage,health);
            Boolean destroyed = health <= 0;
            if (destroyed)
            {
                PublishUnitDestroyedEvent();
            }

            return destroyed;
        }


        protected List<PathStep> ConvertWorldCoordinatePointsToMapTilePathSteps(List<Point> listOfPoints)
        {
            List<PathStep> listOfPathSteps = new List<PathStep>();

            foreach (Point point in listOfPoints)
            {

                MapTileLocation mapTileLocation = MapTileLocation.CreateFromWorldCoordinates(point.X, point.Y);

                PathStep pathStep = new PathStep(
                    mapTileLocation.XInWorldMapTileCoordinates,
                    mapTileLocation.YInWorldMapTileCoordinates);

                listOfPathSteps.Add(pathStep);
            }

            return listOfPathSteps;

        }

        protected void PublishUnitArrivedAtPathStep(Point pathStepPoint)
        {

            PathStep pathStep = new PathStep(pathStepPoint.X, pathStepPoint.Y);


            UnitArrivedAtPathStepEventData eventData =
                new UnitArrivedAtPathStepEventData(this.UnitId, pathStep);

            string serializedEventData = JsonConvert.SerializeObject(eventData);


            SimulationStateUpdateEvent simulationStateUpdateEvent =
                new SimulationStateUpdateEvent(
                    UnitArrivedAtPathStepEventData.EventType,
                    serializedEventData);

            SimulationMain.instance.PublishEvent(simulationStateUpdateEvent);
        }



    }
}
