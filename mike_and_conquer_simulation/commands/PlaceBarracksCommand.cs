// using Microsoft.Extensions.Logging;
using mike_and_conquer_simulation.main;
using Serilog;

namespace mike_and_conquer_simulation.commands
{
    public class PlaceBarracksCommand : AsyncSimulationCommand
    {

        private static readonly ILogger Logger = Log.ForContext<PlaceBarracksCommand>();

        public const string CommandName = "PlaceBarracks";

        public int XInWorldCoordinates { get; set; }
        public int YInWorldCoordinates { get; set; }



        protected override void ProcessImpl()
        {
            Logger.Information("Running PlaceBarracksCommand");
            SimulationMain.instance.CreateGDIBarracksViaConstructionYard(XInWorldCoordinates, YInWorldCoordinates);

            result = true;

        }

        // public Minigunner GetMinigunner()
        // {
        //     return (Minigunner)GetResult();
        //
        // }

    }
}
