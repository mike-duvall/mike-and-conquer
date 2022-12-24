namespace mike_and_conquer_simulation.events
{
    public class BuildingMinigunnerPercentCompletedEventData
    {

        public const string EventType = "BuildingMinigunnerPercentCompleted";

        public int PercentCompleted { get;  }

        public BuildingMinigunnerPercentCompletedEventData(int percentCompleted)
        {
            this.PercentCompleted = percentCompleted;
        }



    }
}



