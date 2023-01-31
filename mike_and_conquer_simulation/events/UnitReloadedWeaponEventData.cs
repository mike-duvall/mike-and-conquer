using System;


namespace mike_and_conquer_simulation.events
{
    public class UnitReloadedWeaponEventData
    {

        public const string EventType = "UnitReloadedWeapon";

        public int UnitId { get; }
        public long Timestamp { get; }


        public UnitReloadedWeaponEventData(int unitId)
        {
            this.Timestamp = DateTime.Now.Ticks;
            UnitId = unitId;
        }

    }
}
