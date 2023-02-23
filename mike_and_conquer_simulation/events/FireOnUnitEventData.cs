using System;

namespace mike_and_conquer_simulation.events
{
    public class FireOnUnitEventData
    {

        public const string EventType = "FiredOnUnit";

        public int AttackerUnitId { get; }
        public int TargetUnitId { get; }
        public long Timestamp { get; }




        public FireOnUnitEventData(int attackerUnitId, int targetUnitId)
        {
            this.AttackerUnitId = attackerUnitId;
            this.TargetUnitId = targetUnitId;
            this.Timestamp = DateTime.Now.Ticks;
        }

    }
}
