using mike_and_conquer_simulation.main;

namespace mike_and_conquer_simulation.commands
{
    public class OrderUnitToMoveCommand : AsyncSimulationCommand
    {

        public const string CommandName = "OrderUnitMove";

        public int DestinationXInWorldCoordinates { get; set; }
        public int DestinationYInWorldCoordinates { get; set; }

        public int UnitId { get; set; }


        protected override void ProcessImpl()
        {
            SimulationMain.instance.OrderUnitToMove(UnitId, DestinationXInWorldCoordinates,
                DestinationYInWorldCoordinates);

            result = true;

        }

        // public Minigunner GetMinigunner()
        // {
        //     return (Minigunner)GetResult();
        //
        // }

    }
}
