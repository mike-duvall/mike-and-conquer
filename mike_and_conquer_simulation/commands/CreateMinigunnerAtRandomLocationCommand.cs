using mike_and_conquer_simulation.main;

namespace mike_and_conquer_simulation.commands
{
    internal class CreateMinigunnerAtRandomLocationCommand : AsyncSimulationCommand
    {

        public const string CommandName = "CreateMinigunnerAtRandomLocation";


        protected override void ProcessImpl()
        {
            result = SimulationMain.instance.CreateMinigunnerAtRandomLocation();
        }

        public Minigunner GetMinigunner()
        {
            return (Minigunner) GetResult();
        }
    }
}