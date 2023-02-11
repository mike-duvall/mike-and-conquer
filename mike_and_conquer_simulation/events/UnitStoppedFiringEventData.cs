
using System;



namespace mike_and_conquer_simulation.events
{
    public class UnitStoppedFiringEventData
    {

        public const string EventType = "UnitStoppedFiring";


        public int UnitId { get; }
        public long Timestamp { get; }


        public UnitStoppedFiringEventData(int unitId)
        {
            this.UnitId = unitId;
            this.Timestamp = DateTime.Now.Ticks;
        }

    }
}
