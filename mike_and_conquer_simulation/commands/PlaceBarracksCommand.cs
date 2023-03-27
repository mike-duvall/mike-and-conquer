// using Microsoft.Extensions.Logging;
using mike_and_conquer_simulation.main;

namespace mike_and_conquer_simulation.commands
{
    public class PlaceBarracksCommand : AsyncSimulationCommand
    {

        public const string CommandName = "PlaceBarracks";

        public int XInWorldCoordinates { get; set; }
        public int YInWorldCoordinates { get; set; }



        protected override void ProcessImpl()
        {
            // SimulationMain.logger.LogWarning("Running PlaceBarracksCommand");
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
