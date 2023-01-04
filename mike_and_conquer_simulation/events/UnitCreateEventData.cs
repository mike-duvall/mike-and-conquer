using System;

namespace mike_and_conquer_simulation.events
{
    public class UnitCreateEventData
    {

        public int UnitId { get;  }

        public string Player { get;  }

        public int X { get;  }
        public int Y { get;  }

        public UnitCreateEventData(int unitId, string player, int x, int y)
        {
            UnitId = unitId;
            Player = player;
            X = x;
            Y = y;
        }



    }
}



