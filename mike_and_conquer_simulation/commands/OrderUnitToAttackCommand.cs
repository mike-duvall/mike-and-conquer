using mike_and_conquer_simulation.main;

namespace mike_and_conquer_simulation.commands
{
    public class OrderUnitToAttackCommand : AsyncSimulationCommand
    {

        public const string CommandName = "OrderUnitAttack";

        public int AttackerUnitId { get; set; }
        public int TargetUnitId { get; set; }



        protected override void ProcessImpl()
        {
            SimulationMain.instance.OrderUnitToAttack(AttackerUnitId, TargetUnitId);

            result = true;

        }

        // public Minigunner GetMinigunner()
        // {
        //     return (Minigunner)GetResult();
        //
        // }

    }
}
