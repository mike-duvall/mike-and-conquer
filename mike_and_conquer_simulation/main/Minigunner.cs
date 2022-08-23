using System;
using System.Collections.Generic;
using mike_and_conquer_simulation.events;
using mike_and_conquer_simulation.gameworld;
using mike_and_conquer_simulation.pathfinding;
using Newtonsoft.Json;


using MapTileInstance = mike_and_conquer_simulation.gameworld.MapTileInstance;

using Point = System.Drawing.Point;

namespace mike_and_conquer_simulation.main
{
    internal class Minigunner : Unit
    {

        public enum State { IDLE, MOVING, ATTACKING, LANDING_AT_MAP_SQUARE };
        public State state;

        public enum Command { NONE, ATTACK_TARGET, FOLLOW_PATH };
        public Command currentCommand;

        double movementDistanceEpsilon;
        private float movementDelta;

        private List<Point> path;

        private int destinationX;
        private int destinationY;


        public Minigunner()
        {
            state = State.IDLE;
            currentCommand = Command.NONE;
            this.movementDistanceEpsilon = 0.1f;
            float speedFromCncInLeptons = 12;  // 12 leptons, for MCV, MPH_MEDIUM_SLOW = 12
            // float speedFromCncInLeptons = 30;  // 30 leptons, for Jeep, MPH_MEDIUM_FAST = 30


            float pixelsPerSquare = 24;
            float leptonsPerSquare = 256;
            float pixelsPerLepton = 0.09375f;
            float leptonsPerPixel = 10.66666666666667f;


            this.movementDelta = speedFromCncInLeptons * pixelsPerLepton;
            // this.movementDistanceEpsilon = 0.5f;  // worked for MCV
            this.movementDistanceEpsilon = 1.5f;  

            this.gameWorldLocation = GameWorldLocation.CreateFromWorldCoordinates(0, 0);
            this.UnitId = SimulationMain.globalId++;
        }

        // public override void OrderMoveToDestination(int destinationXInWorldCoordinates, int destinationYInWorldCoordinates)
        // {
        //     currentCommand = Command.FOLLOW_PATH;
        //     state = State.MOVING;
        //     this.destinationXInWorldCoordinates = destinationXInWorldCoordinates;
        //     this.destinationYInWorldCoordinates = destinationYInWorldCoordinates;
        // }


        // private void PublishUnitMoveOrderEvent(int destinationXInWorldCoordinates, int destinationYInWorldCoordinates)
        // {
        //     SimulationStateUpdateEvent simulationStateUpdateEvent = new SimulationStateUpdateEvent();
        //     simulationStateUpdateEvent.EventType = UnitMoveOrderEventData.EventType;
        //     UnitMoveOrderEventData eventData = new UnitMoveOrderEventData(
        //         this.UnitId,
        //         destinationXInWorldCoordinates,
        //         destinationYInWorldCoordinates);
        //
        //     simulationStateUpdateEvent.EventData = JsonConvert.SerializeObject(eventData);
        //
        //     SimulationMain.instance.PublishEvent(simulationStateUpdateEvent);
        //
        // }

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

        private List<PathStep> ConvertWorldCoordinatePointsToMapTilePathSteps(List<Point> listOfPoints)
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

        public override void OrderMoveToDestination(int destinationXInWorldCoordinates, int destinationYInWorldCoordinates)
        {


            MapTileInstance currentMapTileInstanceLocation =
                GameWorld.instance.FindMapTileInstance(
                    MapTileLocation.CreateFromWorldCoordinates((int) this.GameWorldLocation.X, (int) this.GameWorldLocation.Y));

            //     currentMapTileInstanceLocation.ClearSlotForMinigunner(this);
            int startColumn = (int)this.GameWorldLocation.X / GameWorld.MAP_TILE_WIDTH;
            int startRow = (int)this.GameWorldLocation.Y / GameWorld.MAP_TILE_HEIGHT;
            Point startPoint = new Point(startColumn, startRow);
            

            AStar aStar = new AStar();
            
            Point destinationSquare = new Point();
            destinationSquare.X = destinationXInWorldCoordinates / GameWorld.MAP_TILE_WIDTH;
            destinationSquare.Y = destinationYInWorldCoordinates / GameWorld.MAP_TILE_HEIGHT;
            
            Path foundPath = aStar.FindPath(GameWorld.instance.navigationGraph, startPoint, destinationSquare);
            
            this.currentCommand = Command.FOLLOW_PATH;
            this.state = State.MOVING;
            
            List<Point> plannedPathAsPoints = new List<Point>();
            List<Node> plannedPathAsNodes = foundPath.nodeList;
            foreach (Node node in plannedPathAsNodes)
            {
                Point point = GameWorld.instance.ConvertMapSquareIndexToWorldCoordinate(node.id);
                plannedPathAsPoints.Add(point);
            }
            
            this.SetPath(plannedPathAsPoints);
            SetDestination(plannedPathAsPoints[0].X, plannedPathAsPoints[0].Y);


            PublishUnitMoveOrderEvent(this.UnitId, destinationXInWorldCoordinates, destinationYInWorldCoordinates);
            PublishUnitMovementPlanCreatedEvent(plannedPathAsPoints);

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





        // public void OrderToMoveToDestination(Point destination)
        // {
        //     MapTileInstance currentMapTileInstanceLocation =
        //         gameWorld.FindMapTileInstance(
        //             MapTileLocation.CreateFromWorldCoordinatesInVector2(
        //                 this.GameWorldLocation.WorldCoordinatesAsVector2));
        //
        //     currentMapTileInstanceLocation.ClearSlotForMinigunner(this);
        //     int startColumn = (int)this.GameWorldLocation.WorldCoordinatesAsVector2.X / GameWorld.MAP_TILE_WIDTH;
        //     int startRow = (int)this.GameWorldLocation.WorldCoordinatesAsVector2.Y / GameWorld.MAP_TILE_HEIGHT;
        //     Point startPoint = new Point(startColumn, startRow);
        //
        //     AStar aStar = new AStar();
        //
        //     Point destinationSquare = new Point();
        //     destinationSquare.X = destination.X / GameWorld.MAP_TILE_WIDTH;
        //     destinationSquare.Y = destination.Y / GameWorld.MAP_TILE_HEIGHT;
        //
        //     Path foundPath = aStar.FindPath(gameWorld.navigationGraph, startPoint, destinationSquare);
        //
        //     this.currentCommand = Command.FOLLOW_PATH;
        //     this.state = State.MOVING;
        //
        //     List<Point> plannedPathAsPoints = new List<Point>();
        //     List<Node> nodeList = foundPath.nodeList;
        //     foreach (Node node in nodeList)
        //     {
        //         Point point = gameWorld.ConvertMapSquareIndexToWorldCoordinate(node.id);
        //         plannedPathAsPoints.Add(point);
        //     }
        //
        //     this.SetPath(plannedPathAsPoints);
        //     SetDestination(plannedPathAsPoints[0].X, plannedPathAsPoints[0].Y);
        // }




        // public override void Update()
        // {
        //
        //
        //     if (currentCommand == Command.FOLLOW_PATH)
        //     {
        //         if (IsAtDestination(destinationXInWorldCoordinates, destinationYInWorldCoordinates))
        //         {
        //             currentCommand = Command.NONE;
        //             state = State.IDLE;
        //
        //             SimulationStateUpdateEvent simulationStateUpdateEvent = new SimulationStateUpdateEvent();
        //             simulationStateUpdateEvent.EventType = UnitArrivedAtDestinationEventData.EventType;
        //             UnitArrivedAtDestinationEventData eventData = new UnitArrivedAtDestinationEventData();
        //             eventData.UnitId = this.UnitId;
        //             eventData.Timestamp = DateTime.Now.Ticks;
        //
        //
        //             eventData.XInWorldCoordinates = (int) Math.Round(this.gameWorldLocation.X, 0);
        //             eventData.YInWorldCoordinates = (int) Math.Round(this.gameWorldLocation.Y, 0);
        //
        //             simulationStateUpdateEvent.EventData = JsonConvert.SerializeObject(eventData);
        //
        //             SimulationMain.instance.PublishEvent(simulationStateUpdateEvent);
        //
        //
        //         }
        //         else
        //         {
        //             if (gameWorldLocation.X < destinationXInWorldCoordinates)
        //             {
        //                 gameWorldLocation.X += movementDelta;
        //             }
        //             else if (gameWorldLocation.X > destinationXInWorldCoordinates)
        //             {
        //                 gameWorldLocation.X -= movementDelta;
        //             }
        //
        //             if (gameWorldLocation.Y < destinationYInWorldCoordinates)
        //             {
        //                 gameWorldLocation.Y += movementDelta;
        //             }
        //             else if (gameWorldLocation.Y > destinationYInWorldCoordinates)
        //             {
        //                 gameWorldLocation.Y -= movementDelta;
        //             }
        //
        //             SimulationStateUpdateEvent simulationStateUpdateEvent = new SimulationStateUpdateEvent();
        //             simulationStateUpdateEvent.EventType = UnitPositionChangedEventData.EventType;
        //             UnitPositionChangedEventData eventData = new UnitPositionChangedEventData();
        //             eventData.UnitId = this.UnitId;
        //
        //
        //             eventData.XInWorldCoordinates = (int)Math.Round(this.gameWorldLocation.X, 0);
        //             eventData.YInWorldCoordinates = (int)Math.Round(this.gameWorldLocation.Y, 0);
        //
        //             simulationStateUpdateEvent.EventData = JsonConvert.SerializeObject(eventData);
        //
        //             SimulationMain.instance.PublishEvent(simulationStateUpdateEvent);
        //
        //
        //         }
        //
        //     }
        //
        // }


        public override void Update()
        {
            // UpdateVisibleMapTiles();
            if (this.currentCommand == Command.NONE)
            {
                HandleCommandNone();
            }
            else if (this.currentCommand == Command.FOLLOW_PATH)
            {
                HandleCommandFollowPath();
            }
            // else if (this.currentCommand == Command.ATTACK_TARGET)
            // {
            //     HandleCommandAttackTarget(gameTime);
            // }


        }


        private void HandleCommandNone()
        {
            this.state = State.IDLE;
        }

        private void HandleCommandFollowPath()
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
                this.currentCommand = Command.NONE;
            }

        }


        private void MoveTowardsCurrentDestinationInPath()
        {
            this.state = State.MOVING;
            Point currentDestinationPoint = path[0];
            SetDestination(currentDestinationPoint.X, currentDestinationPoint.Y);
            MoveTowardsDestination(currentDestinationPoint.X, currentDestinationPoint.Y);

            if (IsAtDestination(currentDestinationPoint.X, currentDestinationPoint.Y))
            {
                PublishUnitArrivedAtPathStep(currentDestinationPoint);
                path.RemoveAt(0);
            }

        }

        private void PublishUnitArrivedAtPathStep(Point pathStepPoint)
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

            if (this.state == State.MOVING)
            {
                Point centerOfDestinationSquare = path[0];

                Point currentDestinationPoint = centerOfDestinationSquare;

                SetDestination(currentDestinationPoint.X, currentDestinationPoint.Y);

            }

            this.state = State.LANDING_AT_MAP_SQUARE;

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




    }
}

