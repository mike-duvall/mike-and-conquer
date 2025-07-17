using System;
using System.Collections.Generic;

using mike_and_conquer_simulation.events;
using mike_and_conquer_simulation.gameworld;
using mike_and_conquer_simulation.pathfinding;
using Newtonsoft.Json;
using Serilog;


using MapTileInstance = mike_and_conquer_simulation.gameworld.MapTileInstance;

using Point = System.Drawing.Point;

namespace mike_and_conquer_simulation.main
{
    internal class Minigunner : Unit
    {
        private static readonly ILogger Logger = Log.ForContext<Minigunner>();

        public enum State { IDLE, MOVING, FIRING, LANDING_AT_MAP_SQUARE };
        public State currentState;

        // private bool isMoving;
        //
        // private bool isFiring;


        // public enum MovementState
        // {
        //     NOT_MOVING,
        //     FOLLOWING_PATH,
        //     LANDING_AT_MAP_SQUARE
        // };
        //
        // public enum AttackState
        // {
        //     NOT_ATTACKING,
        //
        // }
        //
        // public enum TargetingState
        // {
        //     NO_TARGET,
        //     
        // }


        public enum Mission { NONE, ATTACK_TARGET, MOVE_TO_DESTINATION };
        public Mission CurrentMission;

        double movementDistanceEpsilon;
        private float movementDelta;

        private List<Point> path;

        private int destinationX;
        private int destinationY;

        private Unit currentAttackTarget;

        // private MapTileInstance currentMapTileInstance;


        private int reloadTimer;
        private bool weaponIsLoaded;

//         private long reloadStartTimeTicks; // uncomment out this to log reload times

        private static int MAX_HEALTH = 50;

        public Minigunner()
        {
            currentState = State.IDLE;
            CurrentMission = Mission.NONE;
            this.movementDistanceEpsilon = 0.1f;
            float speedFromCncInLeptons = 12;  // 12 leptons, for MCV, MPH_MEDIUM_SLOW = 12
            // float speedFromCncInLeptons = 30;  // 30 leptons, for Jeep, MPH_MEDIUM_FAST = 30

            reloadTimer = 0;
            weaponIsLoaded = true;
            health = MAX_HEALTH;
            maxHealth = MAX_HEALTH;

            // isMoving = false;

            // float pixelsPerSquare = 24;
            // float leptonsPerSquare = 256;
            float pixelsPerLepton = 0.09375f;
            // float leptonsPerPixel = 10.66666666666667f;


            this.movementDelta = speedFromCncInLeptons * pixelsPerLepton;
            // this.movementDistanceEpsilon = 0.5f;  // worked for MCV
            this.movementDistanceEpsilon = 1.5f;  

            this.gameWorldLocation = GameWorldLocation.CreateFromWorldCoordinates(0, 0);
            this.UnitId = SimulationMain.globalId++;
        }

        private void PublishUnitMovementPlanCreatedEvent(List<Point> plannedPathAsPoints)
        {
            List<PathStep> plannedPathAsPathSteps = ConvertWorldCoordinatePointsToMapTilePathSteps(plannedPathAsPoints);

            UnitMovementPlanCreatedEventData eventData =
                new UnitMovementPlanCreatedEventData(this.UnitId, plannedPathAsPathSteps);

            string serializedEventData = JsonConvert.SerializeObject(eventData);
            

            SimulationStateUpdateEvent simulationStateUpdateEvent = 
                new SimulationStateUpdateEvent(
                    UnitMovementPlanCreatedEventData.EventType,
                    serializedEventData);

            SimulationMain.instance.PublishEvent(simulationStateUpdateEvent);
        }

        private void SetPathToDestination(int destinationXInWorldCoordinates, int destinationYInWorldCoordinates)
        {
            MapTileInstance currentMapTileInstanceLocation =
                GameWorld.instance.FindMapTileInstance(
                    MapTileLocation.CreateFromWorldCoordinates((int)this.GameWorldLocation.X, (int)this.GameWorldLocation.Y));

            //     currentMapTileInstanceLocation.ClearSlotForMinigunner(this);
            int startColumn = (int)this.GameWorldLocation.X / GameWorld.MAP_TILE_WIDTH;
            int startRow = (int)this.GameWorldLocation.Y / GameWorld.MAP_TILE_HEIGHT;
            Point startPoint = new Point(startColumn, startRow);


            AStar aStar = new AStar();

            Point destinationSquare = new Point();
            destinationSquare.X = destinationXInWorldCoordinates / GameWorld.MAP_TILE_WIDTH;
            destinationSquare.Y = destinationYInWorldCoordinates / GameWorld.MAP_TILE_HEIGHT;

            Path foundPath = aStar.FindPath(GameWorld.instance.navigationGraph, startPoint, destinationSquare);


            List<Point> plannedPathAsPoints = new List<Point>();
            List<Node> plannedPathAsNodes = foundPath.nodeList;
            foreach (Node node in plannedPathAsNodes)
            {
                Point point = GameWorld.instance.ConvertMapSquareIndexToWorldCoordinate(node.id);
                plannedPathAsPoints.Add(point);
            }

            this.SetPath(plannedPathAsPoints);
            SetDestination(plannedPathAsPoints[0].X, plannedPathAsPoints[0].Y);

            PublishUnitMovementPlanCreatedEvent(plannedPathAsPoints);

        }

        public override void OrderMoveToDestination(int destinationXInWorldCoordinates, int destinationYInWorldCoordinates)
        {
            this.CurrentMission = Mission.MOVE_TO_DESTINATION;
            SetPathToDestination(destinationXInWorldCoordinates, destinationYInWorldCoordinates);
            PublishBeganMissionMoveToDestinationEvent(this.UnitId, destinationXInWorldCoordinates, destinationYInWorldCoordinates);
        }


        public override void OrderToAttackEnemyUnit(Unit targetUnit)
        {
            int destinationXInWorldCoordinates = (int) targetUnit.GameWorldLocation.X;
            int destinationYInWorldCoordinates = (int)targetUnit.GameWorldLocation.Y;

            this.CurrentMission = Mission.ATTACK_TARGET;
            currentAttackTarget = targetUnit;
            
            SetPathToDestination(destinationXInWorldCoordinates,destinationYInWorldCoordinates);
            PublishAttackCommandBeganEvent(this.UnitId, targetUnit.UnitId);
        }


        private void UpdateState(State newState)
        {
            if (newState == State.MOVING && this.currentState != State.MOVING)
            {
                PublishUnitBeganMovingEvent();
            }

            if (newState == State.FIRING && this.currentState != State.FIRING)
            {
                PublishUnitBeganFiringEvent();
            }

            // if (newState == State.IDLE && this.currentState != State.IDLE)
            // {
            //     PublishBeganMissionNoneEvent();
            // }

            this.currentState = newState;
        }


        private void SetPath(List<Point> listOfPoints)
        {
            this.path = listOfPoints;
        }

        private void SetDestination(int x, int y)
        {
            destinationX = x;
            destinationY = y;
        }


        public override void Update()
        {
            UpdateVisibleMapTiles();

            if (!weaponIsLoaded)
            {
                reloadTimer--;
                if (reloadTimer <= 0)
                {
                    // uncomment out this to log reload times
                    // long reloadEndTimeTicks = DateTime.Now.Ticks;
                    // long reloadDurationTicks = reloadEndTimeTicks - reloadStartTimeTicks;
                    // double reloadDurationMilliseconds = reloadDurationTicks / (double)TimeSpan.TicksPerMillisecond;
                    //
                    // Logger.Information("Minigunner {UnitId} completed reloading at {EndTime} ticks. " +
                    //     "Reload duration: {DurationTicks} ticks ({DurationMs:F2} milliseconds)", 
                    //     this.UnitId, reloadEndTimeTicks, reloadDurationTicks, reloadDurationMilliseconds);
                    
                    PublishUnitReloadedWeaponEvent();
                    weaponIsLoaded = true;
                }
            }

            if (this.CurrentMission == Mission.NONE)
            {
                HandleMissionNone();
            }
            else if (this.CurrentMission == Mission.MOVE_TO_DESTINATION)
            {
                HandleMissionMoveToDestination();
            }
            else if (this.CurrentMission == Mission.ATTACK_TARGET)
            {
                HandleMissionAttackTarget();
            }

        }


        private void HandleMissionNone()
        {
            UpdateState(State.IDLE);
        }

        private void HandleMissionMoveToDestination()
        {
            if (path.Count > 1)
            {
                MoveTowardsCurrentDestinationInPath();
            }
            else if (path.Count == 1)
            {

                // TODO:  Currently waiting until units almost arrive to assign
                // them slots on the destination square, but when
                // handling more than 5 units, will probably need to assign slots
                // when the move is initiated, rather than up on arrival
                LandOnFinalDestinationMapSquare();
            }
            else
            {
                this.CurrentMission = Mission.NONE;
                PublishBeganMissionNoneEvent();
            }

        }

        private void HandleMissionAttackTarget()
        {
            if (currentAttackTarget.Health <= 0)
            {
                this.CurrentMission = Mission.NONE;
                PublishBeganMissionNoneEvent();
            }

            if (IsInAttackRange())
            {
                UpdateState(State.FIRING);
                if (weaponIsLoaded)
                {
                    weaponIsLoaded = false;
                    reloadTimer = 17;  // In real Cnc code, value in table is 20
                    // reloadStartTimeTicks = DateTime.Now.Ticks;
                    // Logger.Information("Minigunner {UnitId} started reloading at {StartTime} ticks", 
                    //     this.UnitId, reloadStartTimeTicks);
                    PublishBulletHitTargetEvent(this.UnitId, currentAttackTarget.UnitId);
                    int amountOfDamage = 15;
                    bool destroyed = currentAttackTarget.ApplyDamage(amountOfDamage);
                    PublishFiredOnUnitEvent(this.UnitId, currentAttackTarget.UnitId);
                    if (destroyed)
                    {
                        GameWorld.instance.UnitDestroyed(currentAttackTarget.UnitId);
                    }

                }
            }
            else
            {

                if (path.Count > 0)
                {
                    MoveTowardsCurrentDestinationInPath();
                }
                else
                {

                    int destinationXInWorldCoordinates = (int)currentAttackTarget.GameWorldLocation.X;
                    int destinationYInWorldCoordinates = (int)currentAttackTarget.GameWorldLocation.Y;

                    SetPathToDestination(destinationXInWorldCoordinates, destinationYInWorldCoordinates);
                }
            }
        }


        private double Distance(double dX0, double dY0, double dX1, double dY1)
        {
            return Math.Sqrt((dX1 - dX0) * (dX1 - dX0) + (dY1 - dY0) * (dY1 - dY0));
        }


        private int CalculateDistanceToTarget()
        {
            return (int)Distance(GameWorldLocation.X, GameWorldLocation.Y, currentAttackTarget.GameWorldLocation.X, currentAttackTarget.GameWorldLocation.Y);
        }

        private bool IsInAttackRange()
        {
            int distanceToTarget = CalculateDistanceToTarget();

            if (distanceToTarget < 35)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        private void MoveTowardsCurrentDestinationInPath()
        {
            UpdateState(State.MOVING);

            Point currentDestinationPoint = path[0];
            SetDestination(currentDestinationPoint.X, currentDestinationPoint.Y);
            MoveTowardsDestination(currentDestinationPoint.X, currentDestinationPoint.Y);

            if (IsAtDestination(currentDestinationPoint.X, currentDestinationPoint.Y))
            {
                PublishUnitArrivedAtPathStep(currentDestinationPoint);
                path.RemoveAt(0);
            }

        }

        private void PublishUnitBeganMovingEvent()
        {
            UnitBeganMovingEventData eventData =
                new UnitBeganMovingEventData(this.UnitId);

            string serializedEventData = JsonConvert.SerializeObject(eventData);


            SimulationStateUpdateEvent simulationStateUpdateEvent =
                new SimulationStateUpdateEvent(
                    UnitBeganMovingEventData.EventType,
                    serializedEventData);

            SimulationMain.instance.PublishEvent(simulationStateUpdateEvent);
        }


        private void PublishUnitBeganFiringEvent()
        {
            UnitBeganFiringEventData eventData =
                new UnitBeganFiringEventData(this.UnitId);

            string serializedEventData = JsonConvert.SerializeObject(eventData);


            SimulationStateUpdateEvent simulationStateUpdateEvent =
                new SimulationStateUpdateEvent(
                    UnitBeganFiringEventData.EventType,
                    serializedEventData);

            SimulationMain.instance.PublishEvent(simulationStateUpdateEvent);
        }

        private void PublishBeganMissionNoneEvent()
        {
            BeganMissionNoneEventData eventData =
                new BeganMissionNoneEventData(this.UnitId);

            string serializedEventData = JsonConvert.SerializeObject(eventData);


            SimulationStateUpdateEvent simulationStateUpdateEvent =
                new SimulationStateUpdateEvent(
                    BeganMissionNoneEventData.EventType,
                    serializedEventData);

            SimulationMain.instance.PublishEvent(simulationStateUpdateEvent);
        }

        void MoveTowardsDestination(int destinationX, int destinationY)
        {

            float newX = GameWorldLocation.X;
            float newY = GameWorldLocation.Y;

            // double delta = gameTime.ElapsedGameTime.TotalMilliseconds * scaledMovementSpeed;

            float remainingDistanceX = Math.Abs(destinationX - GameWorldLocation.X);
            float remainingDistanceY = Math.Abs(destinationY - GameWorldLocation.Y);
            double deltaX = movementDelta;
            double deltaY = movementDelta;

            if (remainingDistanceX < deltaX)
            {
                deltaX = remainingDistanceX;
            }

            if (remainingDistanceY < deltaY)
            {
                deltaY = remainingDistanceY;
            }

            if (!IsFarEnoughRight(destinationX))
            {
                newX += (float)deltaX;
            }
            else if (!IsFarEnoughLeft(destinationX))
            {
                newX -= (float)deltaX;
            }

            if (!IsFarEnoughDown(destinationY))
            {
                newY += (float)deltaY;
            }
            else if (!IsFarEnoughUp(destinationY))
            {
                newY -= (float)deltaY;
            }

            // TODO:  Leaving in this commented out code for debugging movement issues.
            // Should remove it later if end up not needing it
            //            float xChange = Math.Abs(positionInWorldCoordinates.X - newX);
            //            float yChange = Math.Abs(positionInWorldCoordinates.Y - newY);
            //            float changeThreshold = 0.10f;
            //
            //            if (xChange < changeThreshold && yChange < changeThreshold)
            //            {
            //                MikeAndConquerGame.instance.log.Information("delta:" + delta);
            //                Boolean isFarEnoughRight = IsFarEnoughRight(destinationX);
            //                Boolean isFarEnoughLeft = IsFarEnoughLeft(destinationX);
            //                Boolean isFarEnoughDown = IsFarEnoughDown(destinationY);
            //                Boolean isFarEnoughUp = IsFarEnoughUp(destinationY);
            //
            //                MikeAndConquerGame.instance.log.Information("isFarEnoughRight:" + isFarEnoughRight);
            //                MikeAndConquerGame.instance.log.Information("isFarEnoughLeft:" + isFarEnoughLeft);
            //                MikeAndConquerGame.instance.log.Information("isFarEnoughDown:" + isFarEnoughDown);
            //                MikeAndConquerGame.instance.log.Information("isFarEnoughUp:" + isFarEnoughUp);
            //                MikeAndConquerGame.instance.log.Information("old:positionInWorldCoordinates=" + positionInWorldCoordinates);
            //                positionInWorldCoordinates = new Vector2(newX, newY);
            //                MikeAndConquerGame.instance.log.Information("new:positionInWorldCoordinates=" + positionInWorldCoordinates);
            //            }
            //            else
            //            {
            //                positionInWorldCoordinates = new Vector2(newX, newY);
            //            }

            gameWorldLocation = GameWorldLocation.CreateFromWorldCoordinates(newX, newY);

            PublishUnitPositionChangedEventData();
        }



        // TODO:  Update this to make minigunner land on specific minigunner slot
        // rather than center of the square
        private void LandOnFinalDestinationMapSquare()
        {

            if (this.currentState == State.MOVING)
            {
                Point centerOfDestinationSquare = path[0];

                Point currentDestinationPoint = centerOfDestinationSquare;

                SetDestination(currentDestinationPoint.X, currentDestinationPoint.Y);

            }

            this.currentState = State.LANDING_AT_MAP_SQUARE;

            MoveTowardsDestination( destinationX, destinationY);

            if (IsAtDestination(destinationX, destinationY))
            {

                Point centerOfDestinationSquare = path[0];

                Point currentDestinationPoint = centerOfDestinationSquare;

                PublishUnitArrivedAtPathStep(currentDestinationPoint);
                PublishUnitArrivedAtDestinationEvent();
                path.RemoveAt(0);
            }


        }

        private bool IsAtDestination(int destinationX, int destinationY)
        {
            return IsAtDestinationX(destinationX) && IsAtDestinationY(destinationY);
        }


        private bool IsAtDestinationY(int destinationY)
        {
            return (
                IsFarEnoughDown(destinationY) &&
                IsFarEnoughUp(destinationY)
            );

        }

        private bool IsAtDestinationX(int destinationX)
        {
            return (
                IsFarEnoughRight(destinationX) &&
                IsFarEnoughLeft(destinationX)
            );

        }

        private bool IsFarEnoughRight(int destinationX)
        {
            // return (GameWorldLocation.WorldCoordinatesAsVector2.X > (destinationX - movementDistanceEpsilon));
            return (this.gameWorldLocation.X > (destinationX - movementDistanceEpsilon));
        }


        private bool IsFarEnoughLeft(int destinationX)
        {
            // return (GameWorldLocation.WorldCoordinatesAsVector2.X < (destinationX + movementDistanceEpsilon));
            return (gameWorldLocation.X < (destinationX + movementDistanceEpsilon));

        }

        private bool IsFarEnoughDown(int destinationY)
        {
            // return (GameWorldLocation.WorldCoordinatesAsVector2.Y > (destinationY - movementDistanceEpsilon));
            return (gameWorldLocation.Y > (destinationY - movementDistanceEpsilon));
        }

        private bool IsFarEnoughUp(int destinationY)
        {
            // return (GameWorldLocation.WorldCoordinatesAsVector2.Y < (destinationY + movementDistanceEpsilon));
            return (gameWorldLocation.Y < (destinationY + movementDistanceEpsilon));
        }

        private void UpdateVisibleMapTiles()
        {

            // TODO: Consider removing this if statement once map shroud is fully working
            // if (GameOptions.instance.DrawShroud == false)
            // {
            //     return;
            // }

            MapTileInstance possibleNewMapTileInstance =
                GameWorld.instance.FindMapTileInstance(
                    MapTileLocation.CreateFromWorldCoordinates(
                        (int)this.GameWorldLocation.X,
                        (int)this.GameWorldLocation.Y)
                    );

            if (possibleNewMapTileInstance == currentMapTileInstance)
            {
                return;
            }

            currentMapTileInstance = possibleNewMapTileInstance;

            // TODO:  Code south needs to handle literal edge cases where minigunner is near edge of 
            // map and there is NO east or west tile, etc
            UpdateNearbyMapTileVisibility(0, 0, MapTileInstance.MapTileVisibility.Visible);


            // east side (mother f**ker) 
            UpdateNearbyMapTileVisibility(1, -1, MapTileInstance.MapTileVisibility.Visible);
            UpdateNearbyMapTileVisibility(1, 0, MapTileInstance.MapTileVisibility.Visible);
            UpdateNearbyMapTileVisibility(1, 1, MapTileInstance.MapTileVisibility.Visible);

            // UpdateNearbyMapTileVisibility(2, 1, MapTileInstance.MapTileVisibility.Visible);


            // west side
            UpdateNearbyMapTileVisibility(-1, -1, MapTileInstance.MapTileVisibility.Visible);
            UpdateNearbyMapTileVisibility(-1, 0, MapTileInstance.MapTileVisibility.Visible);
            UpdateNearbyMapTileVisibility(-1, 1, MapTileInstance.MapTileVisibility.Visible);

            // north side
            UpdateNearbyMapTileVisibility(-1, -1, MapTileInstance.MapTileVisibility.Visible);
            UpdateNearbyMapTileVisibility(0, -1, MapTileInstance.MapTileVisibility.Visible);
            UpdateNearbyMapTileVisibility(1, -1, MapTileInstance.MapTileVisibility.Visible);


            // south side
            UpdateNearbyMapTileVisibility(-1, 1, MapTileInstance.MapTileVisibility.Visible);
            UpdateNearbyMapTileVisibility(0, 1, MapTileInstance.MapTileVisibility.Visible);
            UpdateNearbyMapTileVisibility(1, 1, MapTileInstance.MapTileVisibility.Visible);

        }


        private bool MapTileHasVisibility(MapTileInstance mapTileInstance, MapTileInstance.MapTileVisibility expectedVisbility)
        {
            return mapTileInstance != null && mapTileInstance.Visibility == expectedVisbility;
        }



    }
}

