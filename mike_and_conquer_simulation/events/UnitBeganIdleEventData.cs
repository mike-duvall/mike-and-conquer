
using System;



namespace mike_and_conquer_simulation.events
{
    public class UnitBeganIdleEventData
    {

        public const string EventType = "UnitBeganIdle";


        public int UnitId { get; }
        public long Timestamp { get; }


        public UnitBeganIdleEventData(int unitId)
        {
            this.UnitId = unitId;
            this.Timestamp = DateTime.Now.Ticks;
        }

    }
}
