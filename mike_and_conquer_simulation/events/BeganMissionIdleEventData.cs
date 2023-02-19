
using System;



namespace mike_and_conquer_simulation.events
{
    public class BeganMissionIdleEventData
    {

        public const string EventType = "BeganMissionIdle";


        public int UnitId { get; }
        public long Timestamp { get; }


        public BeganMissionIdleEventData(int unitId)
        {
            this.UnitId = unitId;
            this.Timestamp = DateTime.Now.Ticks;
        }

    }
}
