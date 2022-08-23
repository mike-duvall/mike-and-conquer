namespace mike_and_conquer_simulation.events
{
    public class SimulationStateUpdateEvent
    {

        public string EventType { get;  }

        public string EventData { get;  }

        public SimulationStateUpdateEvent(string eventType, string eventData)
        {
            EventType = eventType;
            EventData = eventData;
        }


    }
}
