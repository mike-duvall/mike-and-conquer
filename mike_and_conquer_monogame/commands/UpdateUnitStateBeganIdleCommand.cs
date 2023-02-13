using mike_and_conquer_monogame.gameview;

using mike_and_conquer_simulation.events;

namespace mike_and_conquer_monogame.commands
{
    public class UpdateUnitStateBeganIdleCommand : AsyncViewCommand
    {

        private UnitBeganIdleEventData eventData;

        public UpdateUnitStateBeganIdleCommand(UnitBeganIdleEventData eventData)
        {
            this.eventData = eventData;
        }

        protected override void ProcessImpl()
        {
            GameWorldView.instance.NotifyUnitBeganIdle(eventData.UnitId);


        }
    }
}
