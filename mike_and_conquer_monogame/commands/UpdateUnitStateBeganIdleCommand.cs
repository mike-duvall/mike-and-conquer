using mike_and_conquer_monogame.gameview;

using mike_and_conquer_simulation.events;

namespace mike_and_conquer_monogame.commands
{
    public class UpdateUnitStateBeganIdleCommand : AsyncViewCommand
    {

        private BeganMissionNoneEventData eventData;

        public UpdateUnitStateBeganIdleCommand(BeganMissionNoneEventData eventData)
        {
            this.eventData = eventData;
        }

        protected override void ProcessImpl()
        {
            GameWorldView.instance.NotifyBeganMissionIdle(eventData.UnitId);


        }
    }
}
