using mike_and_conquer_simulation.main;

namespace mike_and_conquer_simulation.commands
{
    internal class CreateMinigunnerCommand : AsyncSimulationCommand
    {

        public const string CommandName = "CreateMinigunner";


        public int X { get; set; }
        public int Y { get; set; }


        protected override void ProcessImpl()
        {
            result = SimulationMain.instance.CreateMinigunner(X, Y);
        }

        public Minigunner GetMinigunner()
        {
            return (Minigunner) GetResult();
        }
    }
}