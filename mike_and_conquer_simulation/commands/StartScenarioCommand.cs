using mike_and_conquer_simulation.gameworld;
using mike_and_conquer_simulation.main;

namespace mike_and_conquer_simulation.commands
{
    public class StartScenarioCommand : AsyncSimulationCommand
    {
        public const string CommandName = "StartScenario";

        public PlayerController GDIPlayerController { get; set; }

        protected override void ProcessImpl()
        {
            SimulationMain.instance.StartScenario(GDIPlayerController);
            result = true;

        }



    }
}
