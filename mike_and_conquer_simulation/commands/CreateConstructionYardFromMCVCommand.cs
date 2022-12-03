using mike_and_conquer_simulation.main;

namespace mike_and_conquer_simulation.commands
{
    internal class CreateConstructionYardFromMCVCommand : AsyncSimulationCommand
    {


        public int X { get; set; }
        public int Y { get; set; }


        protected override void ProcessImpl()
        {
            result = SimulationMain.instance.CreateMCV(X, Y);
        }

    }
}