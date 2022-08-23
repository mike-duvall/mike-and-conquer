using System.Collections.Generic;
using mike_and_conquer_simulation.events;
using mike_and_conquer_simulation.main;

namespace mike_and_conquer_simulation.commands
{
    internal class GetCopyOfEventHistoryCommand : AsyncSimulationCommand
    {


        protected override void ProcessImpl()
        {
            result = SimulationMain.instance.GetCopyOfEventHistory();

        }

        public List<SimulationStateUpdateEvent> GetCopyOfEventHistory()
        {
            return (List<SimulationStateUpdateEvent>)GetResult();

        }

    }
}
