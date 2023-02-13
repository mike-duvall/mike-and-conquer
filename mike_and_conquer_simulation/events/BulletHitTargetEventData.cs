using System;

namespace mike_and_conquer_simulation.events
{
    public class BulletHitTargetEventData
    {

        public const string EventType = "BulletHitTarget";

        public int AttackerUnitId { get; }
        public int TargetUnitId { get; }
        public long Timestamp { get; }




        public BulletHitTargetEventData(int attackerUnitId, int targetUnitId)
        {
            this.AttackerUnitId = attackerUnitId;
            this.TargetUnitId = targetUnitId;
            this.Timestamp = DateTime.Now.Ticks;
        }

    }
}
