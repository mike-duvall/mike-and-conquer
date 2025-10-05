using System;

namespace mike_and_conquer_simulation.rest.domain
{
    internal class RestSimulationState(String simulationState)
    {

        public string SimulationState { get; } = simulationState;
    }
}
