using mike_and_conquer_monogame.gameview;

using mike_and_conquer_simulation.events;

namespace mike_and_conquer_monogame.commands
{
    public class UpdateUnitStateBeganMovingCommand : AsyncViewCommand
    {

        private UnitBeganMovingEventData eventData;

        public UpdateUnitStateBeganMovingCommand(UnitBeganMovingEventData eventData)
        {
            this.eventData = eventData;
        }

        protected override void ProcessImpl()
        {
            GameWorldView.instance.UpdateUnitStateToMoving(eventData.UnitId);


        }
    }
}
