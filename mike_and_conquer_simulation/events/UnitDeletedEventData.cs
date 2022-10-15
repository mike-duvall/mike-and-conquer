namespace mike_and_conquer_simulation.events
{
    public class UnitDeletedEventData
    {

        public const string EventType = "UnitDeleted";

        public int UnitId { get;  }


        public UnitDeletedEventData(int unitId)
        {
            UnitId = unitId;
        }



    }
}



