using mike_and_conquer_monogame.main;
using mike_and_conquer_simulation.events;
using Serilog;

namespace mike_and_conquer_monogame.commands;

public class AddMCVViewCommand : AsyncViewCommand
{
    private static readonly ILogger Logger = Log.ForContext<AddMinigunnerViewCommand>();


    private MCVCreateEventData eventData;

    public AddMCVViewCommand(MCVCreateEventData eventData)
    {
        this.eventData = eventData;
    }

    protected override void ProcessImpl()
    {
        Logger.Information("ProcessImpl called");
        MikeAndConquerGame.instance.AddMCVView(
            eventData.UnitId,  eventData.X, eventData.Y, eventData.MaxHealth, eventData.Health);

    }

}