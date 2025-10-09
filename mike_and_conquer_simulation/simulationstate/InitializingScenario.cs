
using mike_and_conquer_simulation.gameworld;
using mike_and_conquer_simulation.main;

namespace mike_and_conquer_simulation.simulationstate
{
    internal class InitializingScenario : SimulationState
    {


        internal InitializingScenario()
        {
            SimulationMain.instance.ResetScenario();
            SimulationMain.instance.TemporaryHackPublishInitializeScenarioEvent();
        }

        protected override void UpdateImpl()
        {

        }

    }


}
