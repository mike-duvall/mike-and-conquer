
using System;



namespace mike_and_conquer_simulation.events
{
    public class UnitBeganMovingEventData
    {

        public const string EventType = "UnitBeganMoving";


        public int UnitId { get; }
        public long Timestamp { get; }


        public UnitBeganMovingEventData(int unitId)
        {
            this.UnitId = unitId;
            this.Timestamp = DateTime.Now.Ticks;
        }

    }
}
