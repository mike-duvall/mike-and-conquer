

using SimulationStateListener = mike_and_conquer_simulation.events.SimulationStateListener;
using SimulationStateUpdateEvent = mike_and_conquer_simulation.events.SimulationStateUpdateEvent;

using CompletedBuildingBarracksEventData = mike_and_conquer_simulation.events.CompletedBuildingBarracksEventData;
using NotifyBarracksCompletedBuildingCommand = mike_and_conquer_monogame.commands.NotifyBarracksCompletedBuildingCommand;

using MikeAndConquerGame = mike_and_conquer_monogame.main.MikeAndConquerGame;


namespace mike_and_conquer_monogame.eventhandler
{
    public class UpdateConstructionYardViewWhenConstructionYardCompletesBuildingBarracks : SimulationStateListener
    {
        private MikeAndConquerGame mikeAndConquerGame = null;


        public UpdateConstructionYardViewWhenConstructionYardCompletesBuildingBarracks(MikeAndConquerGame aGame)
        {
            this.mikeAndConquerGame = aGame;
        }


        public override void Update(SimulationStateUpdateEvent anEvent)
        {
            if (anEvent.EventType.Equals(CompletedBuildingBarracksEventData.EventType))
            {
                NotifyBarracksCompletedBuildingCommand command = new NotifyBarracksCompletedBuildingCommand();
                mikeAndConquerGame.PostCommand(command);
            }

        }


    }
}
