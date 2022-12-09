using mike_and_conquer_simulation.main;

namespace mike_and_conquer_simulation.commands
{
    public class BeginBuildingBarracksCommand : AsyncSimulationCommand
    {

        public const string CommandName = "BeginBuildingBarracks";


        protected override void ProcessImpl()
        {
            SimulationMain.instance.BeginBuildingBarracks();

            result = true;

        }

        // public Minigunner GetMinigunner()
        // {
        //     return (Minigunner)GetResult();
        //
        // }

    }
}
