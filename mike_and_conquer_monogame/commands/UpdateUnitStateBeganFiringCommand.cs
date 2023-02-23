using mike_and_conquer_monogame.gameview;

using mike_and_conquer_simulation.events;

namespace mike_and_conquer_monogame.commands
{
    public class UpdateUnitStateBeganFiringCommand : AsyncViewCommand
    {

        private UnitBeganFiringEventData eventData;

        public UpdateUnitStateBeganFiringCommand(UnitBeganFiringEventData eventData)
        {
            this.eventData = eventData;
        }

        protected override void ProcessImpl()
        {
            GameWorldView.instance.UpdateUnitStateToFiring(eventData.UnitId);


        }
    }
}
