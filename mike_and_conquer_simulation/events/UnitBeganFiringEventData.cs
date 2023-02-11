
using System;



namespace mike_and_conquer_simulation.events
{
    public class UnitBeganFiringEventData
    {

        public const string EventType = "UnitBeganFiring";


        public int UnitId { get; }
        public long Timestamp { get; }


        public UnitBeganFiringEventData(int unitId)
        {
            this.UnitId = unitId;
            this.Timestamp = DateTime.Now.Ticks;
        }

    }
}
