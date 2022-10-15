using mike_and_conquer_simulation.main;

namespace mike_and_conquer_simulation.commands
{
    internal class RemoveUnitCommand : AsyncSimulationCommand
    {

        public const string CommandName = "RemoveUnit";


        public int UnitId { get; set; }


        protected override void ProcessImpl()
        {
            SimulationMain.instance.RemoveUnit(UnitId);
        }

    }
}