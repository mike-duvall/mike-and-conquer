

using SimulationStateListener = mike_and_conquer_simulation.events.SimulationStateListener;
using SimulationStateUpdateEvent = mike_and_conquer_simulation.events.SimulationStateUpdateEvent;

using StartedBuildingBarracksEventData = mike_and_conquer_simulation.events.StartedBuildingBarracksEventData;
using NotifyBarracksStartedBuildingCommand = mike_and_conquer_monogame.commands.NotifyBarracksStartedBuildingCommand;

using MikeAndConquerGame = mike_and_conquer_monogame.main.MikeAndConquerGame;


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
            if (anEvent.EventType.Equals(StartedBuildingBarracksEventData.EventType))
            {
                NotifyBarracksStartedBuildingCommand command = new NotifyBarracksStartedBuildingCommand();
                mikeAndConquerGame.PostCommand(command);
            }

        }


    }
}
