using System;

namespace mike_and_conquer_simulation.events
{
    public class AttackCommandBeganEventData
    {

        public const string EventType = "AttackCommandBegan";

        public int AttackerUnitId { get; }
        public int TargetUnitId { get; }
        public long Timestamp { get; }




        public AttackCommandBeganEventData(int attackerUnitId, int targetUnitId)
        {
            this.AttackerUnitId = attackerUnitId;
            this.TargetUnitId = targetUnitId;
            this.Timestamp = DateTime.Now.Ticks;
        }

    }
}
