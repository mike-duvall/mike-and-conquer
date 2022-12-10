namespace mike_and_conquer_simulation.events
{
    public class BuildingBarracksPercentCompletedEventData
    {

        public const string EventType = "BuildingBarracksPercentCompleted";

        public int PercentCompleted { get;  }

        public BuildingBarracksPercentCompletedEventData(int percentCompleted)
        {
            this.PercentCompleted = percentCompleted;
        }



    }
}



