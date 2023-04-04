using System;

namespace mike_and_conquer_simulation.events
{
    public class UnitCreateEventData
    {

        public int UnitId { get;  }

        public string Player { get;  }

        public int X { get;  }
        public int Y { get;  }

        public int MaxHealth { get; }

        public int Health { get; }

        public UnitCreateEventData(int unitId, string player, int x, int y, int maxHealth, int health)
        {
            UnitId = unitId;
            Player = player;
            X = x;
            Y = y;
            MaxHealth = maxHealth;
            Health = health;
        }



    }
}



