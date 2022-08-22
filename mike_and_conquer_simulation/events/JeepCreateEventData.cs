namespace mike_and_conquer_simulation.events
{
    public class JeepCreateEventData : UnitCreateEventData
    {

        public const string EventType = "JeepCreated";

        public JeepCreateEventData(int unitId, int x, int y) : base(unitId, x, y)
        {
        }
    }
}



