

using mike_and_conquer_simulation.events;
using SimulationStateListener = mike_and_conquer_simulation.events.SimulationStateListener;
using SimulationStateUpdateEvent = mike_and_conquer_simulation.events.SimulationStateUpdateEvent;


using NotifyMinigunnerStartedBuildingCommand = mike_and_conquer_monogame.commands.NotifyMinigunnerStartedBuildingCommand;

using MikeAndConquerGame = mike_and_conquer_monogame.main.MikeAndConquerGame;


namespace mike_and_conquer_monogame.eventhandler
{
    public class UpdateBarracksViewWhenBarracksStartsBuildingMinigunner : SimulationStateListener
    {
        private MikeAndConquerGame mikeAndConquerGame = null;


        public UpdateBarracksViewWhenBarracksStartsBuildingMinigunner(MikeAndConquerGame aGame)
        {
            this.mikeAndConquerGame = aGame;
        }


        public override void Update(SimulationStateUpdateEvent anEvent)
        {
            if (anEvent.EventType.Equals(StartedBuildingMinigunnerEventData.EventType))
            {
                NotifyMinigunnerStartedBuildingCommand command = new NotifyMinigunnerStartedBuildingCommand();
                mikeAndConquerGame.PostCommand(command);
            }

        }


    }
}
