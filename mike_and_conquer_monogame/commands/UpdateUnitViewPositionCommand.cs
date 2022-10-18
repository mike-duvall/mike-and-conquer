
using mike_and_conquer_monogame.main;


namespace mike_and_conquer_monogame.commands
{
    public class UpdateUnitViewPositionCommand : AsyncViewCommand
    {

        private readonly int unitId;
        private readonly int xInWorldCoordinates;
        private readonly int yInWorldCoordinates;

        public UpdateUnitViewPositionCommand(int unitId, int xInWorldCoordinates, int yInWorldCoordinates)
        {
            this.unitId = unitId;
            this.xInWorldCoordinates = xInWorldCoordinates;
            this.yInWorldCoordinates = yInWorldCoordinates;
        }

        protected override void ProcessImpl()
        {
            MikeAndConquerGame.instance.UpdateUnitViewPosition(unitId, xInWorldCoordinates, yInWorldCoordinates);
        }
    }
}
