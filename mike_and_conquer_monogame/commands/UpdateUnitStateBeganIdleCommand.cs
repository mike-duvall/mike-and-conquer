using mike_and_conquer_monogame.gameview;

using mike_and_conquer_simulation.events;

namespace mike_and_conquer_monogame.commands
{
    public class UpdateUnitStateBeganIdleCommand : AsyncViewCommand
    {

        private BeganMissionIdleEventData eventData;

        public UpdateUnitStateBeganIdleCommand(BeganMissionIdleEventData eventData)
        {
            this.eventData = eventData;
        }

        protected override void ProcessImpl()
        {
            GameWorldView.instance.NotifyBeganMissionIdle(eventData.UnitId);


        }
    }
}
