using mike_and_conquer_simulation.main;

namespace mike_and_conquer_simulation.commands
{
    internal class ApplyDamageToUnitCommand : AsyncSimulationCommand
    {
        public const string CommandName = "ApplyDamageToUnit";


        public int UnitId { get; set; }
        public int DamageAmount { get; set; }


        protected override void ProcessImpl()
        {
            result = SimulationMain.instance.ApplyDamageToUnit(UnitId, DamageAmount);
        }

        // public Unit GetMinigunner()
        // {
        //     return (Minigunner)GetResult();
        // }

    }
}
