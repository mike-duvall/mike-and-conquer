namespace mike_and_conquer_simulation.events
{
    public class MCVCreateEventData : UnitCreateEventData
    {
        public const string EventType = "MCVCreated";

        public MCVCreateEventData(int unitId, string player, int x, int y) : base(unitId,player, x, y)
        {
        }
    }
}



