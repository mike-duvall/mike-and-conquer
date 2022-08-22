namespace mike_and_conquer_simulation.events
{
    public class MinigunnerCreateEventData : UnitCreateEventData
    {

        public const string EventType = "MinigunnerCreated";

        public MinigunnerCreateEventData(int unitId, int x, int y) : base(unitId, x, y)
        {
        }
    }
}



