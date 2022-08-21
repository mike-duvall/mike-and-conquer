using mike_and_conquer_monogame.commands;
using mike_and_conquer_monogame.commands.mapper;
using mike_and_conquer_monogame.main;
using mike_and_conquer_simulation.events;

namespace mike_and_conquer_monogame.eventhandler
{
    public class CreatePlannedPathViewWhenUnitMovementPlanCreatedEventHandler : SimulationStateListener
    {
        private MikeAndConquerGame mikeAndConquerGame = null;


        public CreatePlannedPathViewWhenUnitMovementPlanCreatedEventHandler(MikeAndConquerGame aGame)
        {
            this.mikeAndConquerGame = aGame;
        }

        public override void Update(SimulationStateUpdateEvent anEvent)
        {
            if (anEvent.EventType.Equals(UnitMovementPlanCreatedEventData.EventType))
            {

                CreatePlannedPathViewCommand command =
                    HandleUnitMovementPlanCreatedCommandMapper.ConvertToCommand(anEvent.EventData);

                mikeAndConquerGame.PostCommand(command);
            }


        }
    }
}
