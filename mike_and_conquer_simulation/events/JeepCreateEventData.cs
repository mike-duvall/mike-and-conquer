﻿namespace mike_and_conquer_simulation.events
{
    public class JeepCreateEventData : UnitCreateEventData
    {

        public const string EventType = "JeepCreated";

        public JeepCreateEventData(int unitId, string player, int x, int y, int maxHealth, int health) : base(unitId, player,x, y, maxHealth, health)
        {
        }
    }
}



