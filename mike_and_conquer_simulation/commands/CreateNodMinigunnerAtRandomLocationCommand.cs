using mike_and_conquer_simulation.main;

namespace mike_and_conquer_simulation.commands
{
    internal class CreateNodMinigunnerAtRandomLocationCommand : AsyncSimulationCommand
    {

        public const string CommandName = "CreateNodMinigunnerAtRandomLocation";


        protected override void ProcessImpl()
        {
            result = SimulationMain.instance.CreateNodMinigunnerAtRandomLocation();
        }

        public Minigunner GetMinigunner()
        {
            return (Minigunner) GetResult();
        }
    }
}