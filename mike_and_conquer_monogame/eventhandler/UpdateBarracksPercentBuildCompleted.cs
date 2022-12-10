using mike_and_conquer_monogame.commands;
using mike_and_conquer_monogame.main;
using mike_and_conquer_simulation.events;



namespace mike_and_conquer_monogame.eventhandler
{
    public class UpdateBarracksPercentBuildCompleted : SimulationStateListener
    {
        private MikeAndConquerGame mikeAndConquerGame = null;


        public UpdateBarracksPercentBuildCompleted(MikeAndConquerGame aGame)
        {
            this.mikeAndConquerGame = aGame;
        }

        public override void Update(SimulationStateUpdateEvent anEvent)
        {
            if (anEvent.EventType.Equals(BuildingBarracksPercentCompletedEventData.EventType))
            {
                // MCVCreateEventData eventData =
                //     JsonConvert.DeserializeObject<MCVCreateEventData>(anEvent.EventData);

                // AddMCVViewCommand viewCommand = new AddMCVViewCommand(eventData.UnitId, eventData.X, eventData.Y);

                NotifyBarracksStartedBuildingCommand command = new NotifyBarracksStartedBuildingCommand();

                mikeAndConquerGame.PostCommand(command);

            }


        }
    }
}
