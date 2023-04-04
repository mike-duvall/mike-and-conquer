namespace mike_and_conquer_simulation.events
{
    public class JeepCreateEventData : UnitCreateEventData
    {

        public const string EventType = "JeepCreated";

        // TODO: Add non-harded values for health and maxhealth
        public JeepCreateEventData(int unitId, string player, int x, int y) : base(unitId, player,x, y, 50, 50)
        {
        }
    }
}



