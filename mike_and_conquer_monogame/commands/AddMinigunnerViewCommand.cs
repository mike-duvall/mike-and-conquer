using mike_and_conquer_monogame.main;

using MinigunnerCreateEventData = mike_and_conquer_simulation.events.MinigunnerCreateEventData;

namespace mike_and_conquer_monogame.commands
{
    public class AddMinigunnerViewCommand : AsyncViewCommand
    {



        private MinigunnerCreateEventData eventData;

        public AddMinigunnerViewCommand(MinigunnerCreateEventData eventData)
        {
            this.eventData = eventData;
        }

        protected override void ProcessImpl()
        {
            MikeAndConquerGame.instance.AddMinigunnerView(
                eventData.UnitId, eventData.X, eventData.Y);

        }
    }
}
