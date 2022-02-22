﻿using mike_and_conquer_simulation.main;

namespace mike_and_conquer_simulation.commands
{
    internal class ResetScenarioCommand : AsyncSimulationCommand
    {


        public const string CommandName = "ResetScenario";

        public int DestinationXInWorldCoordinates { get; set; }
        public int DestinationYInWorldCoordinates { get; set; }

        public int UnitId { get; set; }


        protected override void ProcessImpl()
        {
//            result = SimulationMain.instance.CreateMinigunner(X, Y);
            // SimulationMain.instance.OrderUnitToMove(UnitId, DestinationXInWorldCoordinates,
            //     DestinationYInWorldCoordinates);

            SimulationMain.instance.ResetScenario();
            result = true;

        }


        // public Minigunner GetMinigunner()
        // {
        //     return (Minigunner)GetResult();
        //
        // }

    }
}
