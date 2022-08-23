
using System;



namespace mike_and_conquer_simulation.events
{
    public class UnitArrivedAtPathStepEventData
    {

        public const string EventType = "UnitArrivedAtPathStep";


        public int UnitId { get; }
        public long Timestamp { get; }

        public PathStep PathStep { get;  }


        public UnitArrivedAtPathStepEventData(int unitId, PathStep pathStep)
        {
            this.UnitId = unitId;
            this.Timestamp = DateTime.Now.Ticks;
            this.PathStep = pathStep;

        }

    }
}
