using System;
using mike_and_conquer_simulation.events;


namespace mike_and_conquer_simulation.main
{
    internal class SimulationStateHistoryListener : SimulationStateListener
    {

        private SimulationMain simulationMain;

        public SimulationStateHistoryListener(SimulationMain simulationMain)
        {
            this.simulationMain = simulationMain;
        }
        public override void Update(SimulationStateUpdateEvent anEvent)
        {
            simulationMain.AddHistoryEvent(anEvent);
        }
    }
}
