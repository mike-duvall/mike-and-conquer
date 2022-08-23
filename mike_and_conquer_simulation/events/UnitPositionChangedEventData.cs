namespace mike_and_conquer_simulation.events
{
    public class UnitPositionChangedEventData
    {

        public const string EventType = "UnitPositionChanged";

        public int UnitId { get;  }
        public int XInWorldCoordinates { get;  }
        public int YInWorldCoordinates { get;  }


        public UnitPositionChangedEventData(int unitId, int xInWorldCoordinates, int yInWorldCoordinates)
        {
            UnitId = unitId;
            XInWorldCoordinates = xInWorldCoordinates;
            YInWorldCoordinates = yInWorldCoordinates;
        }



    }
}
