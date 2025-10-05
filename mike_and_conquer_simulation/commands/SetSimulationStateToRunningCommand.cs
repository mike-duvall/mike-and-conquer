using mike_and_conquer_simulation.main;
using mike_and_conquer_simulation.simulationstate;

namespace mike_and_conquer_simulation.commands
{
    internal class SetSimulationStateToRunningCommand : AsyncSimulationCommand
    {
        public const string CommandName = "SetSimulationStateToRunning";


        protected override void ProcessImpl()
        {
            SimulationMain.instance.SetNextState(new RunningScenario());
        }


    }
}
