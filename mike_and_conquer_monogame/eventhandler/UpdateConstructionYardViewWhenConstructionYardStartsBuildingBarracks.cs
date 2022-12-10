using mike_and_conquer_monogame.commands;
using mike_and_conquer_monogame.main;
using mike_and_conquer_simulation.events;
using Newtonsoft.Json;

using UpdateBarracksPercentCompletedCommand = mike_and_conquer_monogame.commands.UpdateBarracksPercentCompletedCommand;


namespace mike_and_conquer_monogame.eventhandler
{
    public class UpdateConstructionYardViewWhenConstructionYardStartsBuildingBarracks : SimulationStateListener
    {
        private MikeAndConquerGame mikeAndConquerGame = null;


        public UpdateConstructionYardViewWhenConstructionYardStartsBuildingBarracks(MikeAndConquerGame aGame)
        {
            this.mikeAndConquerGame = aGame;
        }

        public override void Update(SimulationStateUpdateEvent anEvent)
        {
            if (anEvent.EventType.Equals(BuildingBarracksPercentCompletedEventData.EventType))
            {
                BuildingBarracksPercentCompletedEventData eventData =
                    JsonConvert.DeserializeObject<BuildingBarracksPercentCompletedEventData>(anEvent.EventData);

                UpdateBarracksPercentCompletedCommand command = new
                    UpdateBarracksPercentCompletedCommand(eventData.PercentCompleted);

                mikeAndConquerGame.PostCommand(command);

            }


        }
    }
}
