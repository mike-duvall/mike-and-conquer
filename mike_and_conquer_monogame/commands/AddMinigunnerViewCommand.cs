using mike_and_conquer_monogame.main;
using Serilog;
using MinigunnerCreateEventData = mike_and_conquer_simulation.events.MinigunnerCreateEventData;

namespace mike_and_conquer_monogame.commands
{
    public class AddMinigunnerViewCommand : AsyncViewCommand
    {

        private static readonly ILogger Logger = Log.ForContext<AddMinigunnerViewCommand>();


        private MinigunnerCreateEventData eventData;

        public AddMinigunnerViewCommand(MinigunnerCreateEventData eventData)
        {
            this.eventData = eventData;
        }

        protected override void ProcessImpl()
        {
            Logger.Information("ProcessImpl called");
            MikeAndConquerGame.instance.AddMinigunnerView(
                eventData.UnitId, eventData.Player ,eventData.X, eventData.Y);

        }
    }
}
