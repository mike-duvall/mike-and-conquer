using System;

namespace mike_and_conquer_simulation.events
{
    public class BeganMissionAttackEventData
    {

        public const string EventType = "BeganMissionAttack";

        public int AttackerUnitId { get; }
        public int TargetUnitId { get; }
        public long Timestamp { get; }




        public BeganMissionAttackEventData(int attackerUnitId, int targetUnitId)
        {
            this.AttackerUnitId = attackerUnitId;
            this.TargetUnitId = targetUnitId;
            this.Timestamp = DateTime.Now.Ticks;
        }

    }
}
