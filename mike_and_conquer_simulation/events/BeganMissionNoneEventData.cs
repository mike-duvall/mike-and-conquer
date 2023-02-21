
using System;



namespace mike_and_conquer_simulation.events
{
    public class BeganMissionNoneEventData
    {

        public const string EventType = "BeganMissionNone";


        public int UnitId { get; }
        public long Timestamp { get; }


        public BeganMissionNoneEventData(int unitId)
        {
            this.UnitId = unitId;
            this.Timestamp = DateTime.Now.Ticks;
        }

    }
}
