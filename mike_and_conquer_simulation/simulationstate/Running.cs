using mike_and_conquer_simulation.main;

namespace mike_and_conquer_simulation.simulationstate
{
    internal class Running : SimulationState
    {
        protected override void UpdateImpl()
        {
            SimulationMain.instance.gameWorld.Update();
        }

    }


}
