using System;

namespace mike_and_conquer_simulation.events
{
    public class NoneCommandBeganEventData
    {

        public const string EventType = "NoneCommandBegan";

        public int UnitId { get; }
        public long Timestamp { get; }




        public NoneCommandBeganEventData(int unitId)
        {
            this.UnitId = unitId;
            this.Timestamp = DateTime.Now.Ticks;
        }

    }
}
