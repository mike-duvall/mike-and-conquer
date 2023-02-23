using System;


namespace mike_and_conquer_simulation.events
{
    public class UnitTookDamageEventData
    {

        public const string EventType = "UnitTookDamage";

        public int UnitId { get; }
        public int AmountOfDamage { get; }
        public int NewHealthAmount { get; }
        public long Timestamp { get; }


        public UnitTookDamageEventData(int unitId, int amountOfDamage, int newHealthAmount)
        {
            this.Timestamp = DateTime.Now.Ticks;
            UnitId = unitId;
            AmountOfDamage = amountOfDamage;
            NewHealthAmount = newHealthAmount;  
        }

    }
}
