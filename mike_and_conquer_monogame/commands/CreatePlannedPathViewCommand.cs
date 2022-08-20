
using System.Collections.Generic;
using mike_and_conquer_monogame.main;

using mike_and_conquer_simulation.events;

namespace mike_and_conquer_monogame.commands
{
    public class CreatePlannedPathViewCommand : AsyncViewCommand
    {


        private int unitId;
        private List<PathStep> pathStepList;

        public CreatePlannedPathViewCommand(int unitId, List<PathStep> pathStepList)
        {
            this.unitId = unitId;
            this.pathStepList = pathStepList;
        }

        protected override void ProcessImpl()
        {
            MikeAndConquerGame.instance.CreatePlannedPathView(unitId, pathStepList);
        }
    }
}
