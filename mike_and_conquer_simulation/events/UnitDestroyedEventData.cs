using System;


namespace mike_and_conquer_simulation.events
{
    public class UnitDestroyedEventData
    {

        public const string EventType = "UnitDestroyed";

        public int UnitId { get; }
        public long Timestamp { get; }


        public UnitDestroyedEventData(int unitId)
        {
            this.Timestamp = DateTime.Now.Ticks;
            UnitId = unitId;
        }

    }
}
