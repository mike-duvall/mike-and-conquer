using mike_and_conquer_simulation.main;
using mike_and_conquer_simulation.simulationstate;

namespace mike_and_conquer_simulation.commands
{
    internal class StartScenario : AsyncSimulationCommand
    {
        public const string CommandName = "StartScenario";


        protected override void ProcessImpl()
        {
            SimulationMain.instance.SetNextState(new Running());
        }


    }
}
