using mike_and_conquer_simulation.gameworld;
using mike_and_conquer_simulation.main;

namespace mike_and_conquer_simulation.commands
{
    public class StartScenarioInitializationCommand : AsyncSimulationCommand
    {
        public const string CommandName = "StartScenarioInitialization";


        protected override void ProcessImpl()
        {
            SimulationMain.instance.StartScenarioInitialization();
            result = true;

        }



    }
}
