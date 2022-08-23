using System;
using mike_and_conquer_simulation.events;
using Newtonsoft.Json;

namespace mike_and_conquer_simulation.main
{
    internal class MCV : Unit
    {
        public enum State { IDLE, MOVING, ATTACKING, LANDING_AT_MAP_SQUARE };
        public State state;

        public enum Command { NONE, ATTACK_TARGET, FOLLOW_PATH };
        public Command currentCommand;


        private int destinationXInWorldCoordinates;
        private int destinationYInWorldCoordinates;


        double movementDistanceEpsilon;
        private float movementDelta;

        public MCV()
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

        public override void OrderMoveToDestination(int destinationXInWorldCoordinates, int destinationYInWorldCoordinates)
        {
            currentCommand = Command.FOLLOW_PATH;
            state = State.MOVING;
            this.destinationXInWorldCoordinates = destinationXInWorldCoordinates;
            this.destinationYInWorldCoordinates = destinationYInWorldCoordinates;

            PublishUnitMoveOrderEvent(this.UnitId, destinationXInWorldCoordinates, destinationYInWorldCoordinates);

        }

        public override void Update()
        {
            if (currentCommand == Command.FOLLOW_PATH)
            {
                if (IsAtDestination(destinationXInWorldCoordinates, destinationYInWorldCoordinates))
                {
                    currentCommand = Command.NONE;
                    state = State.IDLE;

                    PublishUnitArrivedAtDestinationEvent();

                }
                else
                {
                    if (gameWorldLocation.X < destinationXInWorldCoordinates)
                    {
                        gameWorldLocation.X += movementDelta;
                    }
                    else if (gameWorldLocation.X > destinationXInWorldCoordinates)
                    {
                        gameWorldLocation.X -= movementDelta;
                    }

                    if (gameWorldLocation.Y < destinationYInWorldCoordinates)
                    {
                        gameWorldLocation.Y += movementDelta;
                    }
                    else if (gameWorldLocation.Y > destinationYInWorldCoordinates)
                    {
                        gameWorldLocation.Y -= movementDelta;
                    }

                    PublishUnitPositionChangedEventData();



                }

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

