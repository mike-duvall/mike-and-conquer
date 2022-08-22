using System;

namespace mike_and_conquer_simulation.events
{
    public class UnitMoveOrderEventData
    {

        public const string EventType = "UnitOrderedToMove";

        public int UnitId { get; }
        public long Timestamp { get; }
        public int DestinationXInWorldCoordinates { get;  }
        public int DestinationYInWorldCoordinates { get;  }
        



        public UnitMoveOrderEventData(int unitId, int destinationXInWorldCoordinates, int destinationYInWorldCoordinates)
        {
            this.UnitId = unitId;
            this.Timestamp = DateTime.Now.Ticks;
            this.DestinationXInWorldCoordinates = destinationXInWorldCoordinates;
            this.DestinationYInWorldCoordinates = destinationYInWorldCoordinates;
        }

    }
}
