
using System;



namespace mike_and_conquer_simulation.events
{
    public class UnitStoppedMovingEventData
    {

        public const string EventType = "UnitStoppedMoving";


        public int UnitId { get; }
        public long Timestamp { get; }


        public UnitStoppedMovingEventData(int unitId)
        {
            this.UnitId = unitId;
            this.Timestamp = DateTime.Now.Ticks;
        }

    }
}
