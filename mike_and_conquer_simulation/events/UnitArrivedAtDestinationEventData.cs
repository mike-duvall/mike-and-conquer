
using System;

namespace mike_and_conquer_simulation.events
{
    public class UnitArrivedAtDestinationEventData
    {

        public const string EventType = "UnitArrivedAtDestination";


        public int UnitId { get;  }

        public long Timestamp { get;  }

        public int XInWorldCoordinates { get;  }
        public int YInWorldCoordinates { get;  }


        public UnitArrivedAtDestinationEventData(int unitId, int xInWorldCoordinates, int yInWorldCoordinates)
        {
            UnitId = unitId;
            Timestamp = DateTime.Now.Ticks;
            XInWorldCoordinates = xInWorldCoordinates;
            YInWorldCoordinates = yInWorldCoordinates;
        }



    }
}
