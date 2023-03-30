using mike_and_conquer_simulation.main;
using Serilog;

namespace mike_and_conquer_simulation.commands
{
    internal class CreateMCVCommand : AsyncSimulationCommand
    {

        private static readonly ILogger Logger = Log.ForContext<CreateMCVCommand>();

        public const string CommandName = "CreateMCV";


        public int X { get; set; }
        public int Y { get; set; }


        protected override void ProcessImpl()
        {
            Logger.Information("ProcessImpl called");
            result = SimulationMain.instance.CreateMCV(X, Y);
        }

        public Minigunner GetMinigunner()
        {
            return (Minigunner) GetResult();
        }
    }
}