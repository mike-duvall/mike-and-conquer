using mike_and_conquer_monogame.main;
using mike_and_conquer_simulation.events;
using Serilog;

namespace mike_and_conquer_monogame.commands;

public class AddMCVViewCommand : AsyncViewCommand
{
    // private int unitId;
    // private int x;
    // private int y;
    //
    // public AddMCVViewCommand(int unitId, int x, int y)
    // {
    //     this.unitId = unitId;
    //     this.x = x;
    //     this.y = y;
    // }
    //
    // protected override void ProcessImpl()
    // {
    //     MikeAndConquerGame.instance.AddMCVView(unitId, x, y);
    // }
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