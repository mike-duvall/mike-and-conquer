using mike_and_conquer_monogame.gameview;
using mike_and_conquer_monogame.main;
using mike_and_conquer_simulation.events;

namespace mike_and_conquer_monogame.commands
{
    public class StartAttackAnimationCommand : AsyncViewCommand
    {

        private BeganMissionAttackEventData eventData;

        public StartAttackAnimationCommand(BeganMissionAttackEventData eventData)
        {
            this.eventData = eventData;
        }

        protected override void ProcessImpl()
        {
            GameWorldView.instance.NotifyUnitAttackBegan(eventData.AttackerUnitId);

        }
    }
}
