using mike_and_conquer_simulation.main;

namespace mike_and_conquer_simulation.commands
{
    public class BeginBuildingMinigunnerCommand : AsyncSimulationCommand
    {

        public const string CommandName = "BeginBuildingMinigunner";


        protected override void ProcessImpl()
        {
            SimulationMain.instance.BeginBuildingMinigunner();

            result = true;

        }

        // public Minigunner GetMinigunner()
        // {
        //     return (Minigunner)GetResult();
        //
        // }

    }
}
