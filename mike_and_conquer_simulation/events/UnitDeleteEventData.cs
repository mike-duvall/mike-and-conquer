namespace mike_and_conquer_simulation.events
{
    public class UnitDeleteEventData
    {

        public const string EventType = "UnitDelete";

        public int UnitId { get;  }


        public UnitDeleteEventData(int unitId)
        {
            UnitId = unitId;
        }



    }
}



