using System.Collections.Generic;
using mike_and_conquer_simulation.events;
using mike_and_conquer_simulation.main;
using mike_and_conquer_simulation.simulationstate;

namespace mike_and_conquer_simulation.commands
{
    internal class GetCurrentSimulationStateCommand : AsyncSimulationCommand
    {


        protected override void ProcessImpl()
        {
            result = SimulationMain.instance.GetCurrentSimulationState();

        }

        public SimulationState GetCurrentSimulationState()
        {
            return (SimulationState)GetResult();

        }

    }
}
