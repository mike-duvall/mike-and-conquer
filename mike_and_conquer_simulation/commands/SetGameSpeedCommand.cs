using mike_and_conquer_simulation.main;

namespace mike_and_conquer_simulation.commands
{
    internal class SetGameSpeedCommand : AsyncSimulationCommand
    {

        public const string CommandName = "SetOptions";

        public SimulationOptions.GameSpeed GameSpeed { get; set; }


        protected override void ProcessImpl()
        {
            SimulationMain.instance.SetGameSpeed( GameSpeed);

            result = true;


        }

        // public Minigunner GetMinigunner()
        // {
        //     return (Minigunner)GetResult();
        //
        // }

    }
}
