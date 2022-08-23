namespace mike_and_conquer_simulation.events
{
    public class UnitCreateEventData
    {

        public int UnitId { get;  }

        public int X { get;  }
        public int Y { get;  }

        public UnitCreateEventData(int unitId, int x, int y)
        {
            UnitId = unitId;
            X = x;
            Y = y;
        }



    }
}



