﻿namespace mike_and_conquer_simulation.events
{
    public class MinigunnerCreateEventData : UnitCreateEventData
    {

        public const string EventType = "MinigunnerCreated";

        public MinigunnerCreateEventData(int unitId, string player, int x, int y, int maxHealth, int health) : base(unitId, player, x, y, maxHealth, health)
        {
        }
    }
}



