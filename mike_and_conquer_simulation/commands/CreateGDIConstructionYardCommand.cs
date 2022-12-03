using mike_and_conquer_simulation.main;

namespace mike_and_conquer_simulation.commands
{
    public class CreateGDIConstructionYardCommand : AsyncSimulationCommand
    {

        protected override void ProcessImpl()
        {
            SimulationMain.instance.CreateConstructionYardFromMCV();

            result = true;

        }


    }
}
