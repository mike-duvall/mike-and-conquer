using mike_and_conquer_simulation.main;

namespace mike_and_conquer_simulation.commands
{
    internal class CreateGDIMinigunnerAtRandomLocationCommand : AsyncSimulationCommand
    {

        public const string CommandName = "CreateGDIMinigunnerAtRandomLocation";


        protected override void ProcessImpl()
        {
            result = SimulationMain.instance.CreateGDIMinigunnerAtRandomLocation();
        }

        public Minigunner GetMinigunner()
        {
            return (Minigunner) GetResult();
        }
    }
}