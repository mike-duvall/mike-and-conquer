using mike_and_conquer_simulation.gameworld;
using mike_and_conquer_simulation.main;

namespace mike_and_conquer_simulation.commands
{
    public class StartScenarioCommand : AsyncSimulationCommand
    {


        public const string CommandName = "StartScenario";


        public PlayerController GDIPlayerController { get; set; }

        public int DestinationXInWorldCoordinates { get; set; }
        public int DestinationYInWorldCoordinates { get; set; }

        public int UnitId { get; set; }


        protected override void ProcessImpl()
        {
            //            result = SimulationMain.instance.CreateMinigunner(X, Y);
            // SimulationMain.instance.OrderUnitToMove(UnitId, DestinationXInWorldCoordinates,
            //     DestinationYInWorldCoordinates);

            SimulationMain.instance.StartScenario(GDIPlayerController);
            result = true;

        }


        // public Minigunner GetMinigunner()
        // {
        //     return (Minigunner)GetResult();
        //
        // }

    }
}
